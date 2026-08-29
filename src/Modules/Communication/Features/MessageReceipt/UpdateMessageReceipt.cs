using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.MessageReceipt;

public static class UpdateMessageReceipt
{
	/// <summary>
	/// Represents the response returned by this MessageReceiptEntity feature.
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
		string Code,
		string Name) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface IUpdateMessageReceipt
	{
		Task UpdateAsync(
				MessageReceiptEntity entity,
				CancellationToken cancellationToken);

		Task<bool> ExistsByCodeAsync(
				Guid tenantId,
				string code,
				Guid? excludingId,
				CancellationToken cancellationToken);

		Task<MessageReceiptEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class UpdateMessageReceiptDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : IUpdateMessageReceipt
	{
		public async Task UpdateAsync(
				MessageReceiptEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<MessageReceiptEntity>()
					.Update(entity);
		
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
						FROM communication.message_receipt
						WHERE tenant_id = @TenantId
						  AND code = @Code
						  AND (@ExcludingId IS NULL OR message_receipt_id <> @ExcludingId)
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

		public async Task<MessageReceiptEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT *
					FROM communication.message_receipt
					WHERE tenant_id = @TenantId
					  AND message_receipt_id = @Id
					  AND is_active = TRUE;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
		
				return await connection.QuerySingleOrDefaultAsync<MessageReceiptEntity>(
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

	public sealed class Handler(IUpdateMessageReceipt dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(MessageReceiptEntity))));
			}

			var exists = await dataAccess.ExistsByCodeAsync(
				request.TenantId, request.Code, request.Id, cancellationToken);
			if (exists)
			{
				return Result<Response>.Failure(
					Error.Conflict(
						ErrorMessages.DuplicateCode(nameof(MessageReceiptEntity), request.Code)));
			}

			entity.UpdateDetails(
				request.Code,
				request.Name);
			await dataAccess.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "message-receipt"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(
						command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateMessageReceipt")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(MessageReceiptEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.MessageReceiptId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
