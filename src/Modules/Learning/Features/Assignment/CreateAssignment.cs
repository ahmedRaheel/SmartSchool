using Microsoft.EntityFrameworkCore;
using Dapper;
using SmartSchool.Application.Persistence;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.Assignment;

public static class CreateAssignment
{
	/// <summary>
	/// Represents the response returned by this AssignmentEntity feature.
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

	public interface ICreateAssignment
	{
		Task AddAsync(
				AssignmentEntity entity,
				CancellationToken cancellationToken);

	}

	internal sealed class CreateAssignmentDataAccess(
		IApplicationDbContext dbContext) : ICreateAssignment
	{
		public async Task AddAsync(
				AssignmentEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext
					.Set<AssignmentEntity>()
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	}

	public sealed class Handler(IBusinessNumberGenerator numberGenerator,
		IDbConnectionFactory connectionFactory,
		ICreateAssignment dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var branchCode = await connection.ExecuteScalarAsync<string>(new CommandDefinition(
                "SELECT code FROM org.campus WHERE tenant_id=@TenantId AND campus_id=@BranchId",
                new { request.TenantId, request.BranchId }, cancellationToken: cancellationToken));
            if (string.IsNullOrWhiteSpace(branchCode)) return Result<Response>.Failure(Error.Validation("A valid branch is required."));
            var code = await numberGenerator.NextAsync("ASSIGNMENT:" + request.BranchId, $"{branchCode}-ASG-", request.TenantId, 7, cancellationToken);

			var entity = AssignmentEntity.Create(
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
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "assignment"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateAssignment")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(
		AssignmentEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.AcademicAssignmentId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
