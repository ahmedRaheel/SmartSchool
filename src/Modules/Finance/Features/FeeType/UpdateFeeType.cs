using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.FeeType;

public static class UpdateFeeType
{
	/// <summary>
	/// Represents the response returned by this FeeTypeEntity feature.
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
	string Frequency,
	bool IsActive,
	string? Description);

	public sealed record Request(
		Guid TenantId,
		Guid Id,
		string Name,
		string Frequency = "Monthly",
		bool IsActive = true,
		string? Description = null) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface IUpdateFeeType
	{
		Task UpdateAsync(
				FeeTypeEntity entity,
				CancellationToken cancellationToken);
Task<FeeTypeEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class UpdateFeeTypePersistence(IApplicationDbContext dbContext) : IUpdateFeeType
	{
		public async Task UpdateAsync(
				FeeTypeEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<FeeTypeEntity>()
					.Update(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<FeeTypeEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				return await dbContext
					.Set<FeeTypeEntity>()
					.FirstOrDefaultAsync(
						x => x.TenantId == tenantId
							&& x.FeeTypeId == id,
						cancellationToken);
			}
	}

	public sealed class Handler(IUpdateFeeType dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
			var entity = await dataAccess.GetByIdAsync(
				request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(FeeTypeEntity))));
			}


			entity.UpdateDetails(request.Name, request.Frequency, request.IsActive, request.Description);
			await dataAccess.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "fee-type"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(
						command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateFeeType")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(FeeTypeEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.FeeTypeId,
			entity.Code,
			entity.Name,
			entity.Frequency,
			entity.IsActive,
			entity.Description);
	}
}
