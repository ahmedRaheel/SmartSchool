using SmartSchool.SharedKernel.Constants;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.Modules.Tenancy.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Features.Tenant;

/// <summary>
/// Creates a tenant and its initial master administrator account.
/// This is a platform operation and is available only to SuperAdmin.
/// </summary>
public static class CreateTenant
{
	public sealed record Request(
		string Code,
		string Name,
		string AdminFirstName,
		string AdminLastName,
		string AdminEmail,
		string? AdminPhoneNumber) : IRequest<Result<Response>>;

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
			RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
			RuleFor(x => x.AdminFirstName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.AdminLastName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.AdminEmail).NotEmpty().EmailAddress().MaximumLength(256);
			RuleFor(x => x.AdminPhoneNumber).MaximumLength(50);
		}
	}

	public sealed class Handler(
		ITenantQuery tenantQuery,
		ITenantCommand tenantCommand,
		IIdentityAccountService identityAccountService)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
			var tenantId = Guid.NewGuid();

			var duplicateCode = await tenantQuery.ExistsByCodeAsync(
				tenantId,
				request.Code,
				null,
				cancellationToken);

			if (duplicateCode)
			{
				return Result<Response>.Failure(
					Error.Conflict($"A tenant with code '{request.Code}' already exists."));
			}

			var tenant = TenantEntity.Create(
				tenantId,
				request.Code,
				request.Name);

			await tenantCommand.AddAsync(tenant, cancellationToken);

			try
			{
				var account = await identityAccountService.CreateAccountAsync(
					tenantId,
					tenant.TenantId,
					"Admin",
					request.AdminEmail,
					request.AdminFirstName,
					request.AdminLastName,
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
