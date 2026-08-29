using Microsoft.EntityFrameworkCore;
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

	internal sealed class CreateSubjectPersistence(IApplicationDbContext dbContext) : ICreateSubject
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

		public async Task<string?> GetBranchCodeAsync(Guid tenantId, Guid branchId, CancellationToken cancellationToken)
		{
			return await dbContext.Database.SqlQueryRaw<string>(
				"SELECT code AS \"Value\" FROM org.campus WHERE tenant_id = {0} AND campus_id = {1} AND is_active = TRUE",
				tenantId, branchId).SingleOrDefaultAsync(cancellationToken);
		}	}

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

}
