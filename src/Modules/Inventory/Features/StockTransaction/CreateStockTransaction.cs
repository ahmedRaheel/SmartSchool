using SmartSchool.Modules.Inventory.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Inventory.Features.StockTransaction;

public static class CreateStockTransaction
{
	/// <summary>
	/// Represents the response returned by this StockTransactionEntity feature.
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
		string Name) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface ICreateStockTransaction
	{
		Task AddAsync(
				StockTransactionEntity entity,
				CancellationToken cancellationToken);
}

	internal sealed class CreateStockTransactionPersistence(IInventoryDbContext dbContext) : ICreateStockTransaction
	{
		public async Task AddAsync(
				StockTransactionEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext.StockTransactions
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	}

	public sealed class Handler(ICreateStockTransaction dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{


			var entity = StockTransactionEntity.Create(
				request.TenantId,
				Guid.NewGuid().ToString("N").ToUpperInvariant(),
				request.Name);

			await dataAccess.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "stock-transaction"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateStockTransaction")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(StockTransactionEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.StockTransactionId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
