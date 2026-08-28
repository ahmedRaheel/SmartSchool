using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.DepartmentSubjectTeacher;

public static class CreateDepartmentSubjectTeacher
{
    public sealed record Request(
        Guid TenantId,
        Guid DepartmentId,
        Guid SubjectId,
        Guid TeacherId,
        bool IsPrimary,
        DateOnly? EffectiveFrom,
        DateOnly? EffectiveTo) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid DepartmentSubjectTeacherId,
        Guid TenantId,
        Guid DepartmentId,
        Guid SubjectId,
        Guid TeacherId,
        bool IsPrimary,
        DateOnly? EffectiveFrom,
        DateOnly? EffectiveTo);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.DepartmentId).NotEmpty();
            RuleFor(x => x.SubjectId).NotEmpty();
            RuleFor(x => x.TeacherId).NotEmpty();
            RuleFor(x => x).Must(x => !x.EffectiveFrom.HasValue || !x.EffectiveTo.HasValue || x.EffectiveTo >= x.EffectiveFrom)
                .WithMessage("EffectiveTo must be on or after EffectiveFrom.");
        }
    }

    public sealed class Handler(IDbConnectionFactory connectionFactory)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            const string validateSql = """
                SELECT
                    EXISTS(SELECT 1 FROM org.department WHERE tenant_id=@TenantId AND department_id=@DepartmentId AND is_active=TRUE) AS "DepartmentExists",
                    EXISTS(SELECT 1 FROM academic.subject WHERE tenant_id=@TenantId AND subject_id=@SubjectId AND is_active=TRUE) AS "SubjectExists",
                    EXISTS(SELECT 1 FROM hr.employee WHERE tenant_id=@TenantId AND employee_id=@TeacherId AND is_active=TRUE) AS "TeacherExists";
                """;
            const string insertSql = """
                INSERT INTO academic.department_subject_teacher
                    (tenant_id, department_id, subject_id, teacher_id, is_primary, effective_from, effective_to)
                VALUES
                    (@TenantId, @DepartmentId, @SubjectId, @TeacherId, @IsPrimary, @EffectiveFrom, @EffectiveTo)
                RETURNING department_subject_teacher_id;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var validity = await connection.QuerySingleAsync<Validity>(new CommandDefinition(validateSql, request, cancellationToken: cancellationToken));
            if (!validity.DepartmentExists) return Result<Response>.Failure(Error.Validation("Department does not belong to the tenant."));
            if (!validity.SubjectExists) return Result<Response>.Failure(Error.Validation("Subject does not belong to the tenant."));
            if (!validity.TeacherExists) return Result<Response>.Failure(Error.Validation("Teacher does not belong to the tenant."));

            try
            {
                var id = await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(insertSql, request, cancellationToken: cancellationToken));
                return Result<Response>.Success(new Response(id, request.TenantId, request.DepartmentId, request.SubjectId, request.TeacherId, request.IsPrimary, request.EffectiveFrom, request.EffectiveTo));
            }
            catch (Exception ex) when (ex.Message.Contains("unique", StringComparison.OrdinalIgnoreCase))
            {
                return Result<Response>.Failure(Error.Conflict("This teacher is already assigned to the department and subject for the effective period."));
            }
        }

        private sealed record Validity(bool DepartmentExists, bool SubjectExists, bool TeacherExists);
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "department-subject-teacher"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithName("CreateDepartmentSubjectTeacher")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        return endpoints;
    }
}
