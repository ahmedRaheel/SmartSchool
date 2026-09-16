using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.StudentActivity;

public static class UpdateStudentActivity
{
    public sealed record Request(Guid Id, Guid? TenantId, string? RoleName, DateOnly JoinedAt, DateOnly? LeftAt)
        : IRequest<Result>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.RoleName).MaximumLength(100);
            RuleFor(x => x).Must(x => !x.LeftAt.HasValue || x.LeftAt.Value >= x.JoinedAt)
                .WithMessage("Left date cannot be earlier than joined date.");
        }
    }

    public interface IUpdateStudentActivity
    {
        Task<StudentActivityEntity?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }

    internal sealed class UpdateStudentActivityCommand(IActivitiesDbContext dbContext) : IUpdateStudentActivity
    {
        public Task<StudentActivityEntity?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken) =>
            dbContext.StudentActivities.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.StudentActivityId == id && x.IsActive, cancellationToken);
        public Task SaveAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
    }

    public sealed class Handler(IUpdateStudentActivity command, ITenantScope tenantScope) : IRequestHandler<Request, Result>
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
                return Result.Failure(Error.NotFound("Student activity participation was not found."));
            }

            entity.Update(request.RoleName, request.JoinedAt, request.LeftAt);
            await command.SaveAsync(cancellationToken);
            return Result.Success();
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-activity"),
                async (Guid id, Request body, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result>(body with { Id = id }, cancellationToken)).ToHttpResult())
            .WithName("UpdateStudentActivity")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);
        return endpoints;
    }
}
