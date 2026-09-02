using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.FeeStructure;

public static class UpdateFeeStructure
{
	/// <summary>
	/// Represents the response returned by this FeeStructureEntity feature.
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
	string? MetadataJson);

	public sealed record Request(
		Guid TenantId,
		Guid Id,
		decimal Amount,
		string Frequency = "Monthly",
		DateOnly? EffectiveFrom = null,
		DateOnly? EffectiveTo = null,
		bool IsActive = true) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
		}
	}

	public interface IUpdateFeeStructure
	{
		Task UpdateAsync(
				FeeStructureEntity entity,
				CancellationToken cancellationToken);
Task<FeeStructureEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class UpdateFeeStructurePersistence(IFinanceDbContext dbContext) : IUpdateFeeStructure
	{
		public async Task UpdateAsync(
				FeeStructureEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext.FeeStructures
					.Update(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<FeeStructureEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				return await dbContext.FeeStructures
					.FirstOrDefaultAsync(
						x => x.TenantId == tenantId
							&& x.FeeStructureId == id,
						cancellationToken);
			}
	}

	public sealed class Handler(IUpdateFeeStructure dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(FeeStructureEntity))));
			}


			entity.Update(request.Amount, request.Frequency, request.EffectiveFrom, request.EffectiveTo, request.IsActive);
			await dataAccess.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "fee-structure"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(
						command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateFeeStructure")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(FeeStructureEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.FeeStructureId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
