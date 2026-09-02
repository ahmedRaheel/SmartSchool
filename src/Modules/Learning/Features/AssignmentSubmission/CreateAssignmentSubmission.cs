using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.AssignmentSubmission;

public static class CreateAssignmentSubmission
{
	/// <summary>
	/// Represents the response returned by this AssignmentSubmissionEntity feature.
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
		string Name) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface ICreateAssignmentSubmission
	{
		Task AddAsync(
				AssignmentSubmissionEntity entity,
				CancellationToken cancellationToken);
}

	internal sealed class CreateAssignmentSubmissionPersistence(ILearningDbContext dbContext) : ICreateAssignmentSubmission
	{
		public async Task AddAsync(
				AssignmentSubmissionEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext.AssignmentSubmissions
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	}

	public sealed class Handler(ICreateAssignmentSubmission dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{


			var entity = AssignmentSubmissionEntity.Create(
				request.TenantId,
				Guid.NewGuid().ToString("N").ToUpperInvariant(),
				request.Name);

			await dataAccess.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "assignment-submission"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateAssignmentSubmission")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(AssignmentSubmissionEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.AssignmentSubmissionId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
