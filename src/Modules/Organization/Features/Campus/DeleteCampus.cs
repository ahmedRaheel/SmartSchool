using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class DeleteCampus
{
	public sealed record Command(
		Guid TenantId,
		Guid Id) : IRequest<Result<Response>>;

	public sealed record Response(
		Guid TenantId,
		Guid Id);

	public interface IDeleteCampus
	{
		Task DeleteAsync(
				CampusEntity entity,
				CancellationToken cancellationToken);

		Task<CampusEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

	}

	internal sealed class DeleteCampusPersistence(
		IApplicationDbContext dbContext) : IDeleteCampus
	{
		public async Task DeleteAsync(
				CampusEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<CampusEntity>()
					.Remove(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	
		public Task<CampusEntity?> GetByIdAsync(
			Guid tenantId, Guid id, CancellationToken cancellationToken)
		{
			return dbContext.Set<CampusEntity>()
				.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.CampusId == id, cancellationToken);
		}
}

	public sealed class Handler(IDeleteCampus dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(CampusEntity))));
			}
			await dataAccess.DeleteAsync(entity, cancellationToken);
			return Result<Response>.Success(new Response(request.TenantId, request.Id));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapDelete(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "campus"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Command(tenantId, id);
					var result = await mediator.SendAsync<Command, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("DeleteCampus")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
		return endpoints;
	}
}
