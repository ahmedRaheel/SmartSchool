using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.InquiryConversation;

public static class DeleteInquiryConversation
{
	public sealed record Command(
		Guid TenantId,
		Guid Id) : IRequest<Result<Response>>;

	public sealed record Response(
		Guid TenantId,
		Guid Id);

	public interface IDeleteInquiryConversation
	{
		Task DeleteAsync(
				InquiryConversationEntity entity,
				CancellationToken cancellationToken);

		Task<InquiryConversationEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class DeleteInquiryConversationDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : IDeleteInquiryConversation
	{
		public async Task DeleteAsync(
				InquiryConversationEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<InquiryConversationEntity>()
					.Remove(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<InquiryConversationEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT *
					FROM ai_core.inquiry_conversation
					WHERE tenant_id = @TenantId
					  AND inquiry_conversation_id = @Id
					  AND is_active = TRUE;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
		
				return await connection.QuerySingleOrDefaultAsync<InquiryConversationEntity>(
					new CommandDefinition(
						sql,
						new { TenantId = tenantId, Id = id },
						cancellationToken: cancellationToken)).ConfigureAwait(false);
			}
	}

	public sealed class Handler(IDeleteInquiryConversation dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(InquiryConversationEntity))));
			}
			await dataAccess.DeleteAsync(entity, cancellationToken);
			return Result<Response>.Success(new Response(request.TenantId, request.Id));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapDelete(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "inquiry-conversation"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Command(tenantId, id);
					var result = await mediator.SendAsync<Command, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("DeleteInquiryConversation")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}
}
