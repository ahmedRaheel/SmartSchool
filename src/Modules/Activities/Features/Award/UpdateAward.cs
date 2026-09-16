using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Award;

public static class UpdateAward
{
    public sealed record Request(
        Guid Id,
        Guid? TenantId,
        string AwardTypeCode,
        string Title,
        string? Description,
        DateOnly AwardDate,
        Guid? ApprovedBy) : IRequest<Result>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.AwardTypeCode).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(180);
            RuleFor(x => x.Description).MaximumLength(2000);
        }
    }

    public interface IUpdateAward
    {
        Task<AwardEntity?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }

    internal sealed class UpdateAwardCommand(IActivitiesDbContext dbContext) : IUpdateAward
    {
        public Task<AwardEntity?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken) =>
            dbContext.Awards.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.StudentAwardId == id && x.IsActive, cancellationToken);

        public Task SaveAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
    }

    public sealed class Handler(IUpdateAward command, ITenantScope tenantScope, ICurrentUser currentUser)
        : IRequestHandler<Request, Result>
    {
        public async Task<Result> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result.Failure(Error.Validation("Tenant context is required."));
            }

            var entity = await command.GetAsync(tenantId.Value, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result.Failure(Error.NotFound("Award was not found."));
            }

            entity.UpdateDetails(
                request.AwardTypeCode,
                request.Title,
                request.Description,
                request.AwardDate,
                request.ApprovedBy ?? currentUser.EmployeeId);

            await command.SaveAsync(cancellationToken);
            return Result.Success();
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "award"),
                async (Guid id, Request body, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result>(body with { Id = id }, cancellationToken)).ToHttpResult())
            .WithName("UpdateAward")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        return endpoints;
    }
}
