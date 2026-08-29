using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.ConversationParticipant;

public static class DeleteConversationParticipant
{
	public sealed record Command(
		Guid TenantId,
		Guid Id) : IRequest<Result<Response>>;

	public sealed record Response(
		Guid TenantId,
		Guid Id);

	public interface IDeleteConversationParticipant
	{
		Task DeleteAsync(
				ConversationParticipantEntity entity,
				CancellationToken cancellationToken);

		Task<ConversationParticipantEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class DeleteConversationParticipantDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : IDeleteConversationParticipant
	{
		public async Task DeleteAsync(
				ConversationParticipantEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<ConversationParticipantEntity>()
					.Remove(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<ConversationParticipantEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT *
					FROM communication.conversation_participant
					WHERE tenant_id = @TenantId
					  AND conversation_participant_id = @Id
					  AND is_active = TRUE;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
		
				return await connection.QuerySingleOrDefaultAsync<ConversationParticipantEntity>(
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

	public sealed class Handler(IDeleteConversationParticipant dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(ConversationParticipantEntity))));
			}
			await dataAccess.DeleteAsync(entity, cancellationToken);
			return Result<Response>.Success(new Response(request.TenantId, request.Id));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapDelete(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "conversation-participant"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Command(tenantId, id);
					var result = await mediator.SendAsync<Command, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("DeleteConversationParticipant")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}
}
