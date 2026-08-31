using SmartSchool.Modules.Academics.Persistence;
using Microsoft.EntityFrameworkCore;
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

    public sealed class Handler(IAcademicsDbContext dbContext)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var validity = await dbContext.Database.SqlQueryRaw<Validity>(
                "SELECT EXISTS(SELECT 1 FROM org.department WHERE tenant_id={0} AND department_id={1} AND is_active=TRUE) AS \"DepartmentExists\", EXISTS(SELECT 1 FROM academic.subject WHERE tenant_id={0} AND subject_id={2} AND is_active=TRUE) AS \"SubjectExists\", EXISTS(SELECT 1 FROM hr.employee WHERE tenant_id={0} AND employee_id={3} AND is_active=TRUE) AS \"TeacherExists\"",
                request.TenantId, request.DepartmentId, request.SubjectId, request.TeacherId).SingleAsync(cancellationToken);
            if (!validity.DepartmentExists) return Result<Response>.Failure(Error.Validation("Department does not belong to the tenant."));
            if (!validity.SubjectExists) return Result<Response>.Failure(Error.Validation("Subject does not belong to the tenant."));
            if (!validity.TeacherExists) return Result<Response>.Failure(Error.Validation("Teacher does not belong to the tenant."));

            var id = Guid.NewGuid();
            await dbContext.Database.ExecuteSqlRawAsync(
                "INSERT INTO academic.department_subject_teacher (department_subject_teacher_id, tenant_id, department_id, subject_id, teacher_id, is_primary, effective_from, effective_to) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                new { id, request.TenantId, request.DepartmentId, request.SubjectId, request.TeacherId, request.IsPrimary, request.EffectiveFrom, request.EffectiveTo },
                cancellationToken);
            return Result<Response>.Success(new Response(id, request.TenantId, request.DepartmentId, request.SubjectId, request.TeacherId, request.IsPrimary, request.EffectiveFrom, request.EffectiveTo));
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
