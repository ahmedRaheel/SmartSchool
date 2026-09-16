using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public static class DeleteAdmissionApplication
{
    public sealed record Request(Guid? TenantId, Guid Id) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            
        }
    }

    public interface IDeleteAdmissionApplication
    {
        Task<Result<Response>> ExecuteAsync(Guid tenantId, Request request, CancellationToken cancellationToken);
    }

    internal sealed class Command(IAdmissionsDbContext dbContext, ICurrentUser user) : IDeleteAdmissionApplication
    {
        public async Task<Result<Response>> ExecuteAsync(Guid tenantId, Request request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.AdmissionApplications.SingleOrDefaultAsync(
                x => x.TenantId == tenantId && x.ApplicationId == request.Id && x.IsActive && (!user.BranchId.HasValue || x.BranchId == user.BranchId), cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(Error.NotFound("Application was not found."));
            }
            if (entity.StudentId.HasValue)
            {
                return Result<Response>.Failure(Error.Conflict("Manage an admitted applicant through the student record."));
            }

            entity.Deactivate();
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<Response>.Success(new Response(entity.ApplicationId));
        }
    }

    public sealed class Handler(ITenantScope tenantScope, IDeleteAdmissionApplication command) : IRequestHandler<Request, Result<Response>>
    {
        public Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            return tenantId.HasValue
                ? command.ExecuteAsync(tenantId.Value, request, cancellationToken)
                : Task.FromResult(Result<Response>.Failure(Error.Validation("Tenant context is required.")));
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/api/admissions/workflow/applications/{id:guid}",
            async (Guid id, Guid? tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                (await mediator.SendAsync<Request, Result<Response>>(new Request(tenantId, id), cancellationToken)).ToHttpResult())
            .WithName("DeleteAdmissionApplication")
            .WithTags("Admissions")
            .RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
    }
}
