using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Organization;

public static class UpdateOrganization
{
    public sealed record Request(
        Guid Id,
        string OrganizationName,
        string FirstName,
        string LastName,
        string? MetadataJson,
        string? ContactName,
        string? ContactEmail,
        string? ContactPhoneNumber,
        string? ContactAddress) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        string Code,
        string OrganizationName,
        string FirstName,
        string LastName,
        string? MetadataJson);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.OrganizationName).NotEmpty().MaximumLength(250);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ContactEmail).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
            RuleFor(x => x.ContactPhoneNumber).MaximumLength(50);
        }
    }

    public interface IUpdateOrganization
    {
        Task<TenantEntity?> GetAsync(Guid tenantId, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }

    internal sealed class Persistence(IOrganizationDbContext dbContext) : IUpdateOrganization
    {
        public Task<TenantEntity?> GetAsync(Guid tenantId, CancellationToken cancellationToken) =>
            dbContext.Tenants
                .Include(x => x.ContactDetails)
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.IsActive, cancellationToken);

        public Task SaveAsync(CancellationToken cancellationToken) =>
            dbContext.SaveChangesAsync(cancellationToken);
    }

    public sealed class Handler(IUpdateOrganization persistence)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenant = await persistence.GetAsync(request.Id, cancellationToken);
            if (tenant is null)
            {
                return Result<Response>.Failure(Error.NotFound(ErrorMessages.EntityNotFound(nameof(TenantEntity))));
            }

            tenant.UpdateDetails(
                tenant.Code,
                request.OrganizationName,
                request.FirstName,
                request.LastName,
                request.MetadataJson);

            var primaryContact = tenant.ContactDetails.FirstOrDefault(x => x.IsPrimary);
            if (primaryContact is null &&
                (!string.IsNullOrWhiteSpace(request.ContactName) ||
                 !string.IsNullOrWhiteSpace(request.ContactEmail) ||
                 !string.IsNullOrWhiteSpace(request.ContactPhoneNumber) ||
                 !string.IsNullOrWhiteSpace(request.ContactAddress)))
            {
                tenant.AddContactDetail(TenantContactEntity.CreatePrimary(
                    tenant.TenantId,
                    request.ContactName ?? string.Empty,
                    request.ContactEmail ?? string.Empty,
                    request.ContactPhoneNumber ?? string.Empty,
                    request.ContactAddress ?? string.Empty));
            }
            else
            {
                primaryContact?.UpdatePrimary(
                    request.ContactName,
                    request.ContactEmail,
                    request.ContactPhoneNumber,
                    request.ContactAddress);
            }

            await persistence.SaveAsync(cancellationToken);

            return Result<Response>.Success(new Response(
                tenant.TenantId,
                tenant.Code,
                tenant.OrganizationName,
                tenant.FirstName,
                tenant.LastName,
                tenant.MetadataJson));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/tenancy/tenant/{id:guid}",
                async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result<Response>>(
                        request with { Id = id }, cancellationToken)).ToHttpResult())
            .WithName("UpdateOrganization")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);

        return endpoints;
    }
}
