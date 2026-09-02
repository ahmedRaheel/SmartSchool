using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;


namespace SmartSchool.Modules.Organization.Features.Organization;

/// <summary>
/// Creates a tenant and its initial master administrator account.
/// This is a platform operation and is available only to SuperAdmin.
/// </summary>
public static class CreateTenant
{
	public sealed record Request(
		string OrganizationName,
		string AdminFirstName,
		string AdminLastName,
		string AdminEmail,
		string AdminPhoneNumber,
		string ContactName,
		string ContactEmail,
		string ContactPhoneNumber,
		string ContactAddress
		) : IRequest<Result<Response>>;

	public sealed record AdminAccountResponse(
		Guid UserId,
		string Email,
		string TemporaryPassword,
		bool MustChangePassword);

	public sealed record Response(
		Guid TenantId,
		Guid Id,
		string Code,
		string OrganizationName,
		AdminAccountResponse AdminAccount,
		ContactResponse Contact);
	public sealed record ContactResponse(
		 Guid TenantId,
		 string ContactName,
		 string ContactEmail,
		 string ContactPhoneNumber,
		 string ContactAddress
		);

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.OrganizationName).NotEmpty().MaximumLength(250);
			RuleFor(x => x.AdminFirstName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.AdminLastName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.AdminEmail).NotEmpty().EmailAddress().MaximumLength(256);
			RuleFor(x => x.AdminPhoneNumber).MaximumLength(50);
		}
	}
	public interface ITenantCommand
	{
		Task AddAsync(
				TenantEntity entity,
				CancellationToken cancellationToken);

		Task DeleteAsync(
				TenantEntity entity,
				CancellationToken cancellationToken);

	}
	public sealed class TenantCommand(IOrganizationDbContext dbContext) : ITenantCommand
	{
		public async Task AddAsync(
				TenantEntity entity,
				CancellationToken cancellationToken)
		{
			await dbContext.Tenants.AddAsync(entity, cancellationToken);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
		public async Task DeleteAsync(
				TenantEntity entity,
				CancellationToken cancellationToken)
		{
			dbContext.Tenants.Remove(entity);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}
	public sealed class Handler(
		ITenantCommand tenantCommand,
		IIdentityAccountService identityAccountService,
		IBusinessNumberGenerator numberGenerator)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
			var tenantId = Guid.NewGuid();

			var code = await numberGenerator.NextAsync(
				"TENANT", "TN", null, 8, cancellationToken);

			var tenant = TenantEntity.Create(
				tenantId,
				code,
				request.OrganizationName,
				request.AdminFirstName,
				request.AdminLastName);

			if (!string.IsNullOrWhiteSpace(request.ContactName))
			{
				tenant.AddContactDetail(
					 TenantContactEntity.CreatePrimary(tenant.TenantId,
					 request.ContactName,
					 request.ContactEmail,
					 request.ContactPhoneNumber,
					 request.ContactAddress)
					);
			}
			await tenantCommand.AddAsync(tenant, cancellationToken);
			try
			{
				var account = await identityAccountService.CreateAccountAsync(
					tenantId,
					tenant.TenantId,
					nameof(Role.Tenant),
					request.AdminEmail,
					request.AdminFirstName,
					request.AdminLastName,
					null,
					null,
					[nameof(Role.Tenant)],
					cancellationToken);

				return Result<Response>.Success(
					new Response(
						tenantId,
						tenant.TenantId,
						tenant.Code,
						tenant.OrganizationName,
						new AdminAccountResponse(
							account.UserId,
							account.Email,
							account.TemporaryPassword,
							account.MustChangePassword),
						new ContactResponse(
							 tenant.TenantId,
							 ContactName: request.ContactName,
							 ContactEmail: request.ContactEmail,
							 ContactPhoneNumber: request.ContactPhoneNumber,
							 ContactAddress: request.ContactAddress
							)
						));
			}
			catch
			{
				// Do not leave a tenant without its master account.
				await tenantCommand.DeleteAsync(tenant, cancellationToken);
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
