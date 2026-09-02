using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.KnowledgeCollection;

public static class UpdateKnowledgeCollection
{
	/// <summary>
	/// Represents the response returned by this KnowledgeCollectionEntity feature.
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
		string Name) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface IUpdateKnowledgeCollection
	{
		Task UpdateAsync(
				KnowledgeCollectionEntity entity,
				CancellationToken cancellationToken);
Task<KnowledgeCollectionEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class UpdateKnowledgeCollectionPersistence(IAICoreDbContext dbContext) : IUpdateKnowledgeCollection
	{
		public async Task UpdateAsync(
				KnowledgeCollectionEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext.KnowledgeCollections
					.Update(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<KnowledgeCollectionEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				return await dbContext.KnowledgeCollections
					.FirstOrDefaultAsync(
						x => x.TenantId == tenantId
							&& x.KnowledgeCollectionId == id,
						cancellationToken);
			}
	}

	public sealed class Handler(IUpdateKnowledgeCollection dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(KnowledgeCollectionEntity))));
			}


			entity.UpdateDetails(
				entity.Code,
				request.Name);
			await dataAccess.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "knowledge-collection"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(
						command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateKnowledgeCollection")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(KnowledgeCollectionEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.KnowledgeCollectionId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
