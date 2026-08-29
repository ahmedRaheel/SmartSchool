using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Department;

public static class CreateDepartment
{
	/// <summary>
	/// Represents the response returned by this DepartmentEntity feature.
	/// </summary>
	/// <param name="TenantId">The owning tenant identifier.</param>
	/// <param name="Id">The entity identifier.</param>
	/// <param name="Code">The business code.</param>
	/// <param name="Name">The display name.</param>
	public sealed record Response(
	Guid TenantId,
	Guid Id,
	string Code,
	string Name,
	string? Telephone,
	string? Email,
	Guid? CampusId,
	Guid? HeadOfDepartmentEmployeeId,
	string? MetadataJson);

	public sealed record Request(
		Guid TenantId,
		Guid CampusId,
		Guid? HeadOfDepartmentEmployeeId,
		string Name,
		string? Telephone,
		string? Email) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.CampusId).NotEmpty();
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
			RuleFor(x => x.Telephone).MaximumLength(50);
			RuleFor(x => x.Email).EmailAddress().MaximumLength(250).When(x => !string.IsNullOrWhiteSpace(x.Email));
		}
	}

	public interface ICreateDepartment
	{
		Task AddAsync(
				DepartmentEntity entity,
				CancellationToken cancellationToken);
}

	internal sealed class CreateDepartmentPersistence(IApplicationDbContext dbContext) : ICreateDepartment
	{
		public async Task AddAsync(
				DepartmentEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext
					.Set<DepartmentEntity>()
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	}

	public sealed class Handler(SmartSchool.Application.Persistence.IBusinessNumberGenerator businessNumberGenerator,
		ICreateDepartment dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
			var code = await businessNumberGenerator.NextAsync(
				$"DEPARTMENT:{request.CampusId}", "DEP-", request.TenantId, 4, cancellationToken);


			var entity = DepartmentEntity.Create(
				request.TenantId,
				request.CampusId,
				request.HeadOfDepartmentEmployeeId,
				code,
				request.Name,
				request.Telephone,
				request.Email);

			await dataAccess.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "department"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateDepartment")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
		return endpoints;
	}

	private static Response MapResponse(DepartmentEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.DepartmentId,
			entity.Code,
			entity.Name,
			entity.Telephone,
			entity.Email,
			entity.CampusId,
			entity.HeadOfDepartmentEmployeeId,
			entity.MetadataJson);
	}
}
