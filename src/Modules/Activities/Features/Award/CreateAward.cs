using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Award;

public static class CreateAward
{
    public sealed record Request(
        Guid? TenantId,
        Guid StudentId,
        string AwardTypeCode,
        string Title,
        string? Description,
        DateOnly AwardDate,
        Guid? ApprovedBy) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid StudentId,
        string StudentNumber,
        string StudentName,
        string AwardTypeCode,
        string Title,
        string? Description,
        DateOnly AwardDate,
        Guid? ApprovedBy,
        Guid? DocumentId);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.AwardTypeCode).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(180);
            RuleFor(x => x.Description).MaximumLength(2000);
        }
    }

    public interface ICreateAward
    {
        Task AddAsync(AwardEntity entity, CancellationToken cancellationToken);
    }

    internal sealed class CreateAwardCommand(IActivitiesDbContext dbContext) : ICreateAward
    {
        public async Task AddAsync(AwardEntity entity, CancellationToken cancellationToken)
        {
            await dbContext.Awards.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(
        ICreateAward command,
        IDbConnectionFactory connectionFactory,
        ITenantScope tenantScope,
        ICurrentUser currentUser)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var student = await GetStudentAsync(tenantId.Value, request.StudentId, cancellationToken);
            if (student is null)
            {
                return Result<Response>.Failure(Error.Validation("Selected student is not active in this tenant."));
            }

            var approvedBy = request.ApprovedBy ?? currentUser.EmployeeId;
            var entity = AwardEntity.Create(
                tenantId.Value,
                request.StudentId,
                request.AwardTypeCode,
                request.Title,
                request.Description,
                request.AwardDate,
                approvedBy);

            await command.AddAsync(entity, cancellationToken);

            return Result<Response>.Success(new Response(
                entity.TenantId,
                entity.StudentAwardId,
                entity.StudentId,
                student.StudentNumber,
                student.StudentName,
                entity.AwardTypeCode,
                entity.Title,
                entity.Description,
                entity.AwardDate,
                entity.ApprovedBy,
                entity.DocumentId));
        }

        private async Task<StudentSummary?> GetStudentAsync(Guid tenantId, Guid studentId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    s.student_number AS "StudentNumber",
                    trim(concat_ws(' ', s.first_name, s.last_name)) AS "StudentName"
                FROM student.student s
                WHERE s.tenant_id=@TenantId
                  AND s.student_id=@StudentId
                  AND s.is_active=TRUE;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<StudentSummary>(
                new CommandDefinition(sql, new { TenantId = tenantId, StudentId = studentId }, cancellationToken: cancellationToken));
        }

        private sealed record StudentSummary(string StudentNumber, string StudentName);
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "award"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithName("CreateAward")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        return endpoints;
    }
}
