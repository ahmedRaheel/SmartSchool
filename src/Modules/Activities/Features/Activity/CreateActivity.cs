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

public static class CreateActivity
{
    public sealed record Request(
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
        string Status = "UPCOMING") : IRequest<Result<Response>>;

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
            RuleFor(x => x.Name).NotEmpty().MaximumLength(180);
            RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Status).Must(value => AllowedStatuses.Contains(value.ToUpperInvariant()))
                .WithMessage("Status must be UPCOMING, ONGOING, COMPLETED, or CANCELLED.");
            RuleFor(x => x.MaxParticipants).GreaterThan(0).When(x => x.MaxParticipants.HasValue);
            RuleFor(x => x).Must(x => !x.StartTime.HasValue || !x.EndTime.HasValue || x.EndTime > x.StartTime)
                .WithMessage("End time must be later than start time.");
        }
    }

    public interface ICreateActivity
    {
        Task AddAsync(ActivityEntity entity, CancellationToken cancellationToken);
    }

    internal sealed class CreateActivityCommand(IActivitiesDbContext dbContext) : ICreateActivity
    {
        public async Task AddAsync(ActivityEntity entity, CancellationToken cancellationToken)
        {
            await dbContext.Activities.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(
        ICreateActivity command,
        IDbConnectionFactory connectionFactory,
        ITenantScope tenantScope,
        IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            if (request.CampusId.HasValue && !await CampusExistsAsync(tenantId.Value, request.CampusId.Value, cancellationToken))
            {
                return Result<Response>.Failure(Error.Validation("Selected branch/campus is invalid for this tenant."));
            }

            if (request.CoordinatorEmployeeId.HasValue && !await EmployeeExistsAsync(tenantId.Value, request.CoordinatorEmployeeId.Value, cancellationToken))
            {
                return Result<Response>.Failure(Error.Validation("Selected coordinator employee is invalid for this tenant."));
            }

            var code = await numberGenerator.NextAsync("Activity", "ACT", tenantId.Value, 5, cancellationToken);
            var entity = ActivityEntity.Create(
                tenantId.Value,
                code,
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

            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(Map(entity));
        }

        private async Task<bool> CampusExistsAsync(Guid tenantId, Guid campusId, CancellationToken cancellationToken)
        {
            const string sql = "SELECT EXISTS(SELECT 1 FROM org.campus WHERE tenant_id=@TenantId AND campus_id=@CampusId AND is_active=TRUE);";
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(new Dapper.CommandDefinition(sql, new { TenantId = tenantId, CampusId = campusId }, cancellationToken: cancellationToken));
        }

        private async Task<bool> EmployeeExistsAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken)
        {
            const string sql = "SELECT EXISTS(SELECT 1 FROM hr.employee WHERE tenant_id=@TenantId AND employee_id=@EmployeeId AND is_active=TRUE);";
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(new Dapper.CommandDefinition(sql, new { TenantId = tenantId, EmployeeId = employeeId }, cancellationToken: cancellationToken));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "activity"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithName("CreateActivity")
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
