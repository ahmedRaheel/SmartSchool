using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public static class DeleteAdmissionCriteria
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

    public interface IDeleteAdmissionCriteria
    {
        Task<bool> ExecuteAsync(Guid tenantId, Request request, CancellationToken cancellationToken);
    }

    internal sealed class Command(IAdmissionsDbContext dbContext) : IDeleteAdmissionCriteria
    {
        public async Task<bool> ExecuteAsync(Guid tenantId, Request request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.AdmissionCriteria.SingleOrDefaultAsync(
                x => x.TenantId == tenantId && x.AdmissionCriteriaId == request.Id && x.Status == "ACTIVE",
                cancellationToken);
            if (entity is null)
            {
                return false;
            }

            entity.Deactivate();
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public sealed class Handler(ITenantScope tenantScope, IDeleteAdmissionCriteria command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (tenantId is null)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            return await command.ExecuteAsync(tenantId.Value, request, cancellationToken)
                ? Result<Response>.Success(new Response(request.Id))
                : Result<Response>.Failure(Error.NotFound("Admission criteria were not found."));
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/api/admissions/criteria/{id:guid}",
            async (Guid id, Guid? tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                (await mediator.SendAsync<Request, Result<Response>>(new Request(tenantId, id), cancellationToken)).ToHttpResult())
            .WithName("DeleteAdmissionCriteria")
            .WithTags("Admission Criteria")
            .RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
    }
}
