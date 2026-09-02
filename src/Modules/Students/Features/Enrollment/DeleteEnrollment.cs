using SmartSchool.Modules.Students.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Enrollment;

public static class DeleteEnrollment
{
	public sealed record Command(
		Guid TenantId,
		Guid Id) : IRequest<Result<Response>>;

	public sealed record Response(
		Guid TenantId,
		Guid Id);

	public interface IDeleteEnrollment
	{
		Task DeleteAsync(
				EnrollmentEntity entity,
				CancellationToken cancellationToken);

		Task<EnrollmentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

	}

	internal sealed class DeleteEnrollmentPersistence(
		IStudentsDbContext dbContext) : IDeleteEnrollment
	{
		public async Task DeleteAsync(
				EnrollmentEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext.Enrollments
					.Remove(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	
		public Task<EnrollmentEntity?> GetByIdAsync(
			Guid tenantId, Guid id, CancellationToken cancellationToken)
		{
			return dbContext.Enrollments
				.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.StudentEnrollmentId == id, cancellationToken);
		}
}

	public sealed class Handler(IDeleteEnrollment dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(EnrollmentEntity))));
			}
			await dataAccess.DeleteAsync(entity, cancellationToken);
			return Result<Response>.Success(new Response(request.TenantId, request.Id));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapDelete(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "enrollment"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Command(tenantId, id);
					var result = await mediator.SendAsync<Command, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("DeleteEnrollment")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
		return endpoints;
	}
}
