using Dapper;
using SmartSchool.Application.Persistence;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

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

	public sealed class Handler(		
		ISubjectCommand entityCommand,
        IBusinessNumberGenerator numberGenerator,
        IDbConnectionFactory connectionFactory)
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
            var code = await numberGenerator.NextAsync("SUBJECT:" + request.BranchId, $"{branchCode}-SB-", request.TenantId, 5, cancellationToken);

			var entity = SubjectEntity.Create(
				request.TenantId,
                request.BranchId,
				code,
				request.Name);

			await entityCommand.AddAsync(entity, cancellationToken);
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
