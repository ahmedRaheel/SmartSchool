using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Academics.Features.Subject;

public static class CreateSubject
{
	/// <summary>
	/// Represents the response returned by this SubjectEntity feature.
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
        Guid BranchId,
		string Name) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.BranchId).NotEmpty();
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface ICreateSubject
	{
		Task AddAsync(
				SubjectEntity entity,
				CancellationToken cancellationToken);

		Task<string?> GetBranchCodeAsync(
				Guid tenantId,
				Guid branchId,
				CancellationToken cancellationToken);

	}

	internal sealed class CreateSubjectDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : ICreateSubject
	{
		public async Task AddAsync(
				SubjectEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext
					.Set<SubjectEntity>()
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<string?> GetBranchCodeAsync(
				Guid tenantId,
				Guid branchId,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT code
					FROM org.campus
					WHERE tenant_id = @TenantId
					  AND campus_id = @BranchId
					  AND is_active = TRUE;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken);
		
				return await connection.ExecuteScalarAsync<string?>(
					new CommandDefinition(
						sql,
						new { TenantId = tenantId, BranchId = branchId },
						cancellationToken: cancellationToken)).ConfigureAwait(false);
			}
	}

	public sealed class Handler(IBusinessNumberGenerator numberGenerator,
		ICreateSubject dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
            var branchCode = await dataAccess.GetBranchCodeAsync(
                request.TenantId,
                request.BranchId,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(branchCode))
            {
                return Result<Response>.Failure(
                    Error.Validation("A valid branch is required."));
            }
            var code = await numberGenerator.NextAsync("SUBJECT:" + request.BranchId, $"{branchCode}-SB-", request.TenantId, 5, cancellationToken);

			var entity = SubjectEntity.Create(
				request.TenantId,
                request.BranchId,
				code,
				request.Name);

			await dataAccess.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "subject"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateSubject")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
		return endpoints;
	}

	private static Response MapResponse(
		SubjectEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.SubjectId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
