using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features.Applicant;

public static class CreateApplicant
{
	/// <summary>
	/// Represents the response returned by this ApplicantEntity feature.
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
		string Name) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface ICreateApplicant
	{
		Task AddAsync(
				ApplicantEntity entity,
				CancellationToken cancellationToken);

		Task<bool> ExistsByCodeAsync(
				Guid tenantId,
				string code,
				Guid? excludingId,
				CancellationToken cancellationToken);

	}

	internal sealed class CreateApplicantDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : ICreateApplicant
	{
		public async Task AddAsync(
				ApplicantEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext
					.Set<ApplicantEntity>()
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
						FROM admission.applicant
						WHERE tenant_id = @TenantId
						  AND code = @Code
						  AND (@ExcludingId IS NULL OR applicant_id <> @ExcludingId)
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

	public sealed class Handler(ICreateApplicant dataAccess)
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
						ErrorMessages.DuplicateCode(nameof(ApplicantEntity), request.Code)));
			}

			var entity = ApplicantEntity.Create(
				request.TenantId,
				request.Code,
				request.Name);

			await dataAccess.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "applicant"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateApplicant")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(ApplicantEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.ApplicantId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
