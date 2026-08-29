using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.TeacherAssignment;

public static class DeleteTeacherAssignment
{
	public sealed record Command(
		Guid TenantId,
		Guid Id) : IRequest<Result<Response>>;

	public sealed record Response(
		Guid TenantId,
		Guid Id);

	public interface IDeleteTeacherAssignment
	{
		Task DeleteAsync(
				TeacherAssignmentEntity entity,
				CancellationToken cancellationToken);

		Task<TeacherAssignmentEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class DeleteTeacherAssignmentDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : IDeleteTeacherAssignment
	{
		public async Task DeleteAsync(
				TeacherAssignmentEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<TeacherAssignmentEntity>()
					.Remove(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<TeacherAssignmentEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT *
					FROM academic.teacher_course_assignment
					WHERE tenant_id = @TenantId
					  AND teacher_course_assignment_id = @Id
					  AND is_active = TRUE;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
		
				return await connection.QuerySingleOrDefaultAsync<TeacherAssignmentEntity>(
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

	public sealed class Handler(IDeleteTeacherAssignment dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(TeacherAssignmentEntity))));
			}
			await dataAccess.DeleteAsync(entity, cancellationToken);
			return Result<Response>.Success(new Response(request.TenantId, request.Id));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapDelete(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "teacher-assignment"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Command(tenantId, id);
					var result = await mediator.SendAsync<Command, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("DeleteTeacherAssignment")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
		return endpoints;
	}
}
