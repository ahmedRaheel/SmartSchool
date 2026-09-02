using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Organization;

public static class DeleteOrganization
{
    public sealed record Command(Guid Id) : IRequest<Result<Response>>;
    public sealed record Response(Guid TenantId);

    public interface IDeleteOrganization
    {
        Task<TenantEntity?> GetAsync(Guid tenantId, CancellationToken cancellationToken);
        Task<bool> HasSchoolsAsync(Guid tenantId, CancellationToken cancellationToken);
        Task DeleteAsync(TenantEntity tenant, CancellationToken cancellationToken);
    }

    internal sealed class Persistence(IOrganizationDbContext dbContext) : IDeleteOrganization
    {
        public Task<TenantEntity?> GetAsync(Guid tenantId, CancellationToken cancellationToken) =>
            dbContext.Tenants.FirstOrDefaultAsync(
                x => x.TenantId == tenantId && x.IsActive,
                cancellationToken);

        public Task<bool> HasSchoolsAsync(Guid tenantId, CancellationToken cancellationToken) =>
            dbContext.Schools.AnyAsync(
                x => x.TenantId == tenantId && x.IsActive,
                cancellationToken);

        public async Task DeleteAsync(TenantEntity tenant, CancellationToken cancellationToken)
        {
            dbContext.Tenants.Remove(tenant);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(IDeleteOrganization persistence)
        : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            var tenant = await persistence.GetAsync(request.Id, cancellationToken);
            if (tenant is null)
            {
                return Result<Response>.Failure(Error.NotFound(ErrorMessages.EntityNotFound(nameof(TenantEntity))));
            }

            if (await persistence.HasSchoolsAsync(request.Id, cancellationToken))
            {
                return Result<Response>.Failure(
                    Error.Conflict("Organization cannot be deleted because it has one or more schools."));
            }

            await persistence.DeleteAsync(tenant, cancellationToken);
            return Result<Response>.Success(new Response(request.Id));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/tenancy/tenant/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Command, Result<Response>>(
                        new Command(id), cancellationToken)).ToHttpResult())
            .WithName("DeleteOrganization")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);

        return endpoints;
    }
}
