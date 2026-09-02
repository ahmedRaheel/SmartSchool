using SmartSchool.Modules.HR.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class DeleteEmployee
{
	public sealed record Command(
		Guid TenantId,
		Guid Id) : IRequest<Result<Response>>;

	public sealed record Response(
		Guid TenantId,
		Guid Id);

	public interface IDeleteEmployee
	{
		Task<EmployeeEntity?> GetByIdAsync(
			Guid tenantId,
			Guid id,
			CancellationToken cancellationToken);

		Task DeleteAsync(
				EmployeeEntity entity,
				CancellationToken cancellationToken);

	}

	internal sealed class DeleteEmployeePersistence(
		IHRDbContext dbContext) : IDeleteEmployee
	{
		public Task<EmployeeEntity?> GetByIdAsync(
			Guid tenantId,
			Guid id,
			CancellationToken cancellationToken)
		{
			return dbContext.Employees
				.SingleOrDefaultAsync(
					entity => entity.TenantId == tenantId && entity.EmployeeId == id,
					cancellationToken);
		}

		public async Task DeleteAsync(
				EmployeeEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext.Employees
					.Remove(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	}

	public sealed class Handler(IDeleteEmployee dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(EmployeeEntity))));
			}
			await dataAccess.DeleteAsync(entity, cancellationToken);
			return Result<Response>.Success(new Response(request.TenantId, request.Id));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapDelete(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "employee"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Command(tenantId, id);
					var result = await mediator.SendAsync<Command, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("DeleteEmployee")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}
}
