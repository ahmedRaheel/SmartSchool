using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.WorkflowStep;

public static class UpdateWorkflowStep
{
	/// <summary>
	/// Represents the response returned by this WorkflowStepEntity feature.
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

	public interface IUpdateWorkflowStep
	{
		Task UpdateAsync(
				WorkflowStepEntity entity,
				CancellationToken cancellationToken);

		Task<bool> ExistsByCodeAsync(
				Guid tenantId,
				string code,
				Guid? excludingId,
				CancellationToken cancellationToken);

		Task<WorkflowStepEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class UpdateWorkflowStepDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : IUpdateWorkflowStep
	{
		public async Task UpdateAsync(
				WorkflowStepEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<WorkflowStepEntity>()
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
						FROM workflow.workflowstep
						WHERE tenant_id = @TenantId
						  AND code = @Code
						  AND (@ExcludingId IS NULL OR workflow_step_id <> @ExcludingId)
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

		public async Task<WorkflowStepEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT *
					FROM workflow.workflowstep
					WHERE tenant_id = @TenantId
					  AND workflow_step_id = @Id
					  AND is_active = TRUE;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
		
				return await connection.QuerySingleOrDefaultAsync<WorkflowStepEntity>(
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

	public sealed class Handler(IUpdateWorkflowStep dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(WorkflowStepEntity))));
			}

			var exists = await dataAccess.ExistsByCodeAsync(
				request.TenantId, request.Code, request.Id, cancellationToken);
			if (exists)
			{
				return Result<Response>.Failure(
					Error.Conflict(
						ErrorMessages.DuplicateCode(nameof(WorkflowStepEntity), request.Code)));
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
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "workflow-step"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(
						command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateWorkflowStep")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(WorkflowStepEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.WorkflowStepId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
