using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.StudentTopicMastery;

public static class UpdateStudentTopicMastery
{
	/// <summary>
	/// Represents the response returned by this StudentTopicMasteryEntity feature.
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

	public interface IUpdateStudentTopicMastery
	{
		Task UpdateAsync(
				StudentTopicMasteryEntity entity,
				CancellationToken cancellationToken);

		Task<bool> ExistsByCodeAsync(
				Guid tenantId,
				string code,
				Guid? excludingId,
				CancellationToken cancellationToken);

		Task<StudentTopicMasteryEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class UpdateStudentTopicMasteryDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : IUpdateStudentTopicMastery
	{
		public async Task UpdateAsync(
				StudentTopicMasteryEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<StudentTopicMasteryEntity>()
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
						FROM ai_tutor.student_topic_mastery
						WHERE tenant_id = @TenantId
						  AND code = @Code
						  AND (@ExcludingId IS NULL OR student_topic_mastery_id <> @ExcludingId)
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

		public async Task<StudentTopicMasteryEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT *
					FROM ai_tutor.student_topic_mastery
					WHERE tenant_id = @TenantId
					  AND student_topic_mastery_id = @Id
					  AND is_active = TRUE;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
		
				return await connection.QuerySingleOrDefaultAsync<StudentTopicMasteryEntity>(
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

	public sealed class Handler(IUpdateStudentTopicMastery dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentTopicMasteryEntity))));
			}

			var exists = await dataAccess.ExistsByCodeAsync(
				request.TenantId, request.Code, request.Id, cancellationToken);
			if (exists)
			{
				return Result<Response>.Failure(
					Error.Conflict(
						ErrorMessages.DuplicateCode(nameof(StudentTopicMasteryEntity), request.Code)));
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
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-topic-mastery"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(
						command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateStudentTopicMastery")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(StudentTopicMasteryEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.StudentTopicMasteryId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
