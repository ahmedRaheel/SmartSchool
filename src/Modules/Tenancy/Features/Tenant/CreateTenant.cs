using SmartSchool.Modules.Tenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using SmartSchool.SharedKernel.Constants;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Tenancy.Features.Tenant;

/// <summary>
/// Creates a tenant and its initial master administrator account.
/// This is a platform operation and is available only to SuperAdmin.
/// </summary>
public static class CreateTenant
{
	public sealed record Request(
		string Name,
		string AdminFirstName,
		string AdminLastName,
		string AdminEmail,
		string? AdminPhoneNumber,
		string ContactName,
		string ContactEmail,
		string ContactPhone,
		string ContactAddress) : IRequest<Result<Response>>;

	public sealed record AdminAccountResponse(
		Guid UserId,
		string Email,
		string TemporaryPassword,
		bool MustChangePassword);

	public sealed record Response(
		Guid TenantId,
		Guid Id,
		string Code,
		string Name,
		AdminAccountResponse AdminAccount);

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
			RuleFor(x => x.AdminFirstName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.AdminLastName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.AdminEmail).NotEmpty().EmailAddress().MaximumLength(256);
			RuleFor(x => x.AdminPhoneNumber).MaximumLength(50);
			RuleFor(x => x.ContactName).NotEmpty().MaximumLength(150);
			RuleFor(x => x.ContactEmail).NotEmpty().EmailAddress().MaximumLength(200);
			RuleFor(x => x.ContactPhone).NotEmpty().MaximumLength(50);
			RuleFor(x => x.ContactAddress).NotEmpty().MaximumLength(1000);
		}
	}

	public interface ICreateTenant
	{
		Task DeleteAsync(
				TenantEntity entity,
				CancellationToken cancellationToken);

		Task AddAsync(
				TenantEntity entity,
				CancellationToken cancellationToken);


		Task AddAsync(TenantContactEntity entity, CancellationToken cancellationToken);
	}

	internal sealed class CreateTenantPersistence(
		ITenancyDbContext dbContext) : ICreateTenant
	{
		public async Task DeleteAsync(
				TenantEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext.Tenants
					.Remove(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task AddAsync(
				TenantEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext.Tenants
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	
		public async Task AddAsync(TenantContactEntity entity, CancellationToken cancellationToken)
		{
			await dbContext.TenantContacts.AddAsync(entity, cancellationToken);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
}

	public sealed class Handler(IIdentityAccountService identityAccountService,
		IBusinessNumberGenerator numberGenerator,
		ICreateTenant dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
			var tenantId = Guid.NewGuid();

			var code = await numberGenerator.NextAsync(
				"TENANT", "TN", null, 4, cancellationToken);

			var tenant = TenantEntity.Create(
				tenantId,
				code,
				request.Name);

			await dataAccess.AddAsync(tenant, cancellationToken);

            var contact = TenantContactEntity.CreatePrimary(
                tenantId,
                request.ContactName,
                request.ContactEmail,
                request.ContactPhone,
                request.ContactAddress);

            await dataAccess.AddAsync(contact, cancellationToken);

			try
			{
				var account = await identityAccountService.CreateAccountAsync(
					tenantId,
					tenant.TenantId,
					"Admin",
					request.AdminEmail,
					request.AdminFirstName,
					request.AdminLastName,
                    null,
                    null,
					["Admin"],
					cancellationToken);

				return Result<Response>.Success(
					new Response(
						tenantId,
						tenant.TenantId,
						tenant.Code,
						tenant.Name,
						new AdminAccountResponse(
							account.UserId,
							account.Email,
							account.TemporaryPassword,
							account.MustChangePassword)));
			}
			catch
			{
				// Do not leave a tenant without its master account.
				await dataAccess.DeleteAsync(tenant, cancellationToken);
				throw;
			}
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				"/api/tenancy/tenant",
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request,
						cancellationToken);

					return result.ToHttpResult();
				})
			.WithName("CreateTenant")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);

		return endpoints;
	}
}
