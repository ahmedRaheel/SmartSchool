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

namespace SmartSchool.Modules.Activities.Features.Activity;

public static class UpdateActivity
{
    public sealed record Request(
        Guid Id,
        Guid? TenantId,
        Guid? CampusId,
        Guid? CoordinatorEmployeeId,
        string Name,
        string Category,
        DateOnly ActivityDate,
        TimeOnly? StartTime,
        TimeOnly? EndTime,
        string? Venue,
        string? Description,
        int? MaxParticipants,
        string Status) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id,
        string Code,
        string Name,
        string Category,
        DateOnly ActivityDate,
        TimeOnly? StartTime,
        TimeOnly? EndTime,
        string? Venue,
        string? Description,
        int? MaxParticipants,
        string Status,
        Guid? CampusId,
        Guid? CoordinatorEmployeeId);

    public sealed class Validator : AbstractValidator<Request>
    {
        private static readonly string[] AllowedStatuses = ["UPCOMING", "ONGOING", "COMPLETED", "CANCELLED"];

        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(180);
            RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Status).NotEmpty().Must(value => !string.IsNullOrWhiteSpace(value) && AllowedStatuses.Contains(value.ToUpperInvariant()));
            RuleFor(x => x.MaxParticipants).GreaterThan(0).When(x => x.MaxParticipants.HasValue);
            RuleFor(x => x).Must(x => !x.StartTime.HasValue || !x.EndTime.HasValue || x.EndTime > x.StartTime)
                .WithMessage("End time must be later than start time.");
        }
    }

    public interface IUpdateActivity
    {
        Task<ActivityEntity?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }

    internal sealed class UpdateActivityCommand(IActivitiesDbContext dbContext) : IUpdateActivity
    {
        public Task<ActivityEntity?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken) =>
            dbContext.Activities.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ActivityId == id && x.IsActive, cancellationToken);

        public Task SaveAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
    }

    public sealed class Handler(
        IUpdateActivity command,
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

            var entity = await command.GetAsync(tenantId.Value, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(Error.NotFound("Activity was not found."));
            }

            if (request.CampusId.HasValue && !await ExistsAsync("org.campus", "campus_id", tenantId.Value, request.CampusId.Value, cancellationToken))
            {
                return Result<Response>.Failure(Error.Validation("Selected branch/campus is invalid for this tenant."));
            }

            if (request.CoordinatorEmployeeId.HasValue && !await ExistsAsync("hr.employee", "employee_id", tenantId.Value, request.CoordinatorEmployeeId.Value, cancellationToken))
            {
                return Result<Response>.Failure(Error.Validation("Selected coordinator employee is invalid for this tenant."));
            }

            entity.UpdateDetails(
                request.Name,
                request.Category,
                request.ActivityDate,
                request.CampusId,
                request.CoordinatorEmployeeId,
                request.StartTime,
                request.EndTime,
                request.Venue,
                request.Description,
                request.MaxParticipants,
                request.Status);

            await command.SaveAsync(cancellationToken);
            return Result<Response>.Success(Map(entity));
        }

        private async Task<bool> ExistsAsync(string table, string idColumn, Guid tenantId, Guid id, CancellationToken cancellationToken)
        {
            var sql = $"SELECT EXISTS(SELECT 1 FROM {table} WHERE tenant_id=@TenantId AND {idColumn}=@Id AND is_active=TRUE);";
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(new Dapper.CommandDefinition(sql, new { TenantId = tenantId, Id = id }, cancellationToken: cancellationToken));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "activity"),
                async (Guid id, Request body, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = body with { Id = id };
                    return (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult();
                })
            .WithName("UpdateActivity")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        return endpoints;
    }

    private static Response Map(ActivityEntity entity) => new(
        entity.TenantId,
        entity.ActivityId,
        entity.Code,
        entity.Name,
        entity.Category,
        entity.ActivityDate,
        entity.StartTime,
        entity.EndTime,
        entity.Venue,
        entity.Description,
        entity.MaxParticipants,
        entity.Status,
        entity.CampusId,
        entity.CoordinatorEmployeeId);
}
