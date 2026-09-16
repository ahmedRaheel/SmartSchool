using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.StudentActivity;

public static class CreateStudentActivity
{
    public sealed record Request(
        Guid? TenantId,
        Guid ActivityId,
        Guid StudentId,
        string? RoleName,
        DateOnly JoinedAt) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid ActivityId,
        Guid StudentId,
        string? RoleName,
        DateOnly JoinedAt,
        DateOnly? LeftAt);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ActivityId).NotEmpty();
            RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.RoleName).MaximumLength(100);
        }
    }

    public interface ICreateStudentActivity
    {
        Task<ActivityEntity?> GetActivityAsync(Guid tenantId, Guid activityId, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid tenantId, Guid activityId, Guid studentId, CancellationToken cancellationToken);
        Task<int> ActiveCountAsync(Guid tenantId, Guid activityId, CancellationToken cancellationToken);
        Task AddAsync(StudentActivityEntity entity, CancellationToken cancellationToken);
    }

    internal sealed class CreateStudentActivityCommand(IActivitiesDbContext dbContext) : ICreateStudentActivity
    {
        public Task<ActivityEntity?> GetActivityAsync(Guid tenantId, Guid activityId, CancellationToken cancellationToken) =>
            dbContext.Activities.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ActivityId == activityId && x.IsActive, cancellationToken);

        public Task<bool> ExistsAsync(Guid tenantId, Guid activityId, Guid studentId, CancellationToken cancellationToken) =>
            dbContext.StudentActivities.AnyAsync(x => x.TenantId == tenantId && x.ActivityId == activityId && x.StudentId == studentId && x.IsActive, cancellationToken);

        public Task<int> ActiveCountAsync(Guid tenantId, Guid activityId, CancellationToken cancellationToken) =>
            dbContext.StudentActivities.CountAsync(x => x.TenantId == tenantId && x.ActivityId == activityId && x.IsActive && x.LeftAt == null, cancellationToken);

        public async Task AddAsync(StudentActivityEntity entity, CancellationToken cancellationToken)
        {
            await dbContext.StudentActivities.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(
        ICreateStudentActivity command,
        IDbConnectionFactory connectionFactory,
        ITenantScope tenantScope)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var activity = await command.GetActivityAsync(tenantId.Value, request.ActivityId, cancellationToken);
            if (activity is null)
            {
                return Result<Response>.Failure(Error.NotFound("Activity was not found."));
            }

            if (!await StudentExistsAsync(tenantId.Value, request.StudentId, activity.CampusId, cancellationToken))
            {
                return Result<Response>.Failure(Error.Validation("Student is not active in the selected branch/campus."));
            }

            if (await command.ExistsAsync(tenantId.Value, request.ActivityId, request.StudentId, cancellationToken))
            {
                return Result<Response>.Failure(Error.Conflict("Student is already registered for this activity."));
            }

            if (activity.MaxParticipants.HasValue)
            {
                var count = await command.ActiveCountAsync(tenantId.Value, request.ActivityId, cancellationToken);
                if (count >= activity.MaxParticipants.Value)
                {
                    return Result<Response>.Failure(Error.Conflict("Activity has reached its participant capacity."));
                }
            }

            var entity = StudentActivityEntity.Create(
                tenantId.Value,
                request.ActivityId,
                request.StudentId,
                request.RoleName,
                request.JoinedAt);

            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(
                entity.TenantId,
                entity.StudentActivityId,
                entity.ActivityId,
                entity.StudentId,
                entity.RoleName,
                entity.JoinedAt,
                entity.LeftAt));
        }

        private async Task<bool> StudentExistsAsync(Guid tenantId, Guid studentId, Guid? campusId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS(
                    SELECT 1
                    FROM student.student s
                    WHERE s.tenant_id=@TenantId
                      AND s.student_id=@StudentId
                      AND s.is_active=TRUE
                      AND upper(COALESCE(s.status, '')) IN ('ACTIVE','ADMITTED')
                      AND (@CampusId IS NULL OR s.branch_id=@CampusId));
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(new Dapper.CommandDefinition(sql, new { TenantId = tenantId, StudentId = studentId, CampusId = campusId }, cancellationToken: cancellationToken));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student-activity"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithName("CreateStudentActivity")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        return endpoints;
    }
}
