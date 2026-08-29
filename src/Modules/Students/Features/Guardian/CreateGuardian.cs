using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Guardian;

public static class CreateGuardian
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid? UserId,
		string FullName,
		string? CnicNumber,
		string? Email,
		string? Phone);

	public sealed record Request(
		Guid TenantId,
		Guid? UserId,
		string FullName,
		string? CnicNumber,
		string? Email,
		string? Phone) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.CnicNumber).NotEmpty();
			RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
		}
	}

	public interface ICreateGuardian
	{
		Task AddAsync(
				GuardianEntity entity,
				CancellationToken cancellationToken);

		Task<bool> ExistsByCnicNumberAsync(Guid tenantId, string cnicNumber, Guid? excludingId, CancellationToken cancellationToken);

	}

	internal sealed class CreateGuardianPersistence(
		IApplicationDbContext dbContext) : ICreateGuardian
	{
		public async Task AddAsync(
				GuardianEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext
					.Set<GuardianEntity>()
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	
		public Task<bool> ExistsByCnicNumberAsync(
			Guid tenantId, string cnicNumber, Guid? excludingId, CancellationToken cancellationToken)
		{
			return dbContext.Set<GuardianEntity>().AnyAsync(
				x => x.TenantId == tenantId && x.CnicNumber == cnicNumber
					&& (!excludingId.HasValue || x.GuardianId != excludingId.Value), cancellationToken);
		}
}

	public sealed class Handler(ICreateGuardian dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
			var exists = !string.IsNullOrWhiteSpace(request.CnicNumber)
				&& await dataAccess.ExistsByCnicNumberAsync(
					request.TenantId, request.CnicNumber, null, cancellationToken);
			if (exists)
			{
				return Result<Response>.Failure(
					Error.Conflict("Guardian with the supplied CnicNumber already exists."));
			}

			var entity = GuardianEntity.Create(
				request.TenantId,
				request.UserId,
				request.FullName,
				request.CnicNumber,
				request.Email,
				request.Phone);

			await dataAccess.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "guardian"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateGuardian").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
		return endpoints;
	}

	private static Response MapResponse(GuardianEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.GuardianId,
			entity.UserId,
			entity.FullName,
			entity.CnicNumber,
			entity.Email,
			entity.Phone);
	}
}
