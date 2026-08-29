using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.ToolDefinition;

public static class DeleteToolDefinition
{
	public sealed record Command(
		Guid TenantId,
		Guid Id) : IRequest<Result<Response>>;

	public sealed record Response(
		Guid TenantId,
		Guid Id);

	public interface IDeleteToolDefinition
	{
		Task DeleteAsync(
				ToolDefinitionEntity entity,
				CancellationToken cancellationToken);

		Task<ToolDefinitionEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class DeleteToolDefinitionDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : IDeleteToolDefinition
	{
		public async Task DeleteAsync(
				ToolDefinitionEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<ToolDefinitionEntity>()
					.Remove(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<ToolDefinitionEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT *
					FROM ai_core.tool_definition
					WHERE tenant_id = @TenantId
					  AND tool_definition_id = @Id
					  AND is_active = TRUE;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
		
				return await connection.QuerySingleOrDefaultAsync<ToolDefinitionEntity>(
					new CommandDefinition(
						sql,
						new
						{
							TenantId = tenantId,
							Id = id
						},
						cancellationToken: cancellationToken)).ConfigureAwait(false);
			}
	}

	public sealed class Handler(IDeleteToolDefinition dataAccess)
		: IRequestHandler<Command, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Command request,
			CancellationToken cancellationToken)
		{
			var entity = await dataAccess.GetByIdAsync(
				request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(ToolDefinitionEntity))));
			}
			await dataAccess.DeleteAsync(entity, cancellationToken);
			return Result<Response>.Success(new Response(request.TenantId, request.Id));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapDelete(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "tool-definition"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Command(tenantId, id);
					var result = await mediator.SendAsync<Command, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("DeleteToolDefinition")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}
}
