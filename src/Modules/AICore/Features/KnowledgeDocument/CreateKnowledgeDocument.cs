using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.KnowledgeDocument;

public static class CreateKnowledgeDocument
{
	/// <summary>
	/// Represents the response returned by this KnowledgeDocumentEntity feature.
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
		string Code,
		string Name,
		string? MetadataJson = null) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface ICreateKnowledgeDocument
	{
		Task AddAsync(
				KnowledgeDocumentEntity entity,
				CancellationToken cancellationToken);

		Task<bool> ExistsByCodeAsync(
				Guid tenantId,
				string code,
				Guid? excludingId,
				CancellationToken cancellationToken);

	}

	internal sealed class CreateKnowledgeDocumentDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : ICreateKnowledgeDocument
	{
		public async Task AddAsync(
				KnowledgeDocumentEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext
					.Set<KnowledgeDocumentEntity>()
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<bool> ExistsByCodeAsync(
				Guid tenantId,
				string code,
				Guid? excludingId,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT EXISTS (
						SELECT 1
						FROM ai_core.knowledge_document
						WHERE tenant_id = @TenantId
						  AND code = @Code
						  AND (@ExcludingId IS NULL OR knowledge_document_id <> @ExcludingId)
					);
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
		
				return await connection.ExecuteScalarAsync<bool>(
					new CommandDefinition(
						sql,
						new
						{
							TenantId = tenantId,
							Code = code,
							ExcludingId = excludingId
						},
						cancellationToken: cancellationToken)).ConfigureAwait(false);
			}
	}

	public sealed class Handler(ICreateKnowledgeDocument dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
			var exists = await dataAccess.ExistsByCodeAsync(
				request.TenantId, request.Code, null, cancellationToken);
			if (exists)
			{
				return Result<Response>.Failure(
					Error.Conflict(
						ErrorMessages.DuplicateCode(nameof(KnowledgeDocumentEntity), request.Code)));
			}

			var entity = KnowledgeDocumentEntity.Create(
				request.TenantId,
				request.Code,
				request.Name,
				request.MetadataJson);

			await dataAccess.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "knowledge-document"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateKnowledgeDocument")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(KnowledgeDocumentEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.KnowledgeDocumentId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
