using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class GetEmployeePage
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		string? EmployeeNumber,
		string FirstName,
		string? LastName,
		string? CnicNumber,
		DateOnly? DateOfBirth,
		string? Gender,
		string? JobTitle,
		string? Department,
		string? Qualification,
		string? Email,
		string? Phone,
		string? AlternatePhone,
		string? Address,
		string? EmergencyContactName,
		string? EmergencyContactPhone,
		DateOnly HireDate,
		string EmploymentTypeCode,
		string StaffType,
		string Status);

	public sealed record Query(Guid TenantId, int Page = 1, int PageSize = 25)
		: IRequest<Result<PagedResult<Response>>>;

	public interface IGetEmployeePage
	{
		Task<PagedResult<Response>> GetPageAsync(
				Guid tenantId,
				int page,
				int pageSize,
				CancellationToken cancellationToken);

	}

	internal sealed class GetEmployeePagePersistence(		
		IDbConnectionFactory connectionFactory) : IGetEmployeePage
	{
		public async Task<PagedResult<Response>> GetPageAsync(
				Guid tenantId,
				int page,
				int pageSize,
				CancellationToken cancellationToken)
			{
				const string countSql = """
					SELECT COUNT(*)
					FROM hr.employee e
					WHERE e.tenant_id = @TenantId
					  AND e.is_active = TRUE;
					""";
		
				const string pageSql = """
					SELECT
					e.tenant_id AS "TenantId",
					e.employee_id AS "Id",
					e.employee_number AS "EmployeeNumber",
					e.first_name AS "FirstName",
					e.last_name AS "LastName",
					e.cnic_number AS "CnicNumber",
					e.date_of_birth AS "DateOfBirth",
					e.gender AS "Gender",
					e.job_title AS "JobTitle",
					d.name AS "Department",
					(SELECT ee.qualification FROM hr.employee_education ee WHERE ee.tenant_id = e.tenant_id AND ee.employee_id = e.employee_id AND ee.is_active = TRUE ORDER BY ee.is_highest DESC, ee.end_date DESC NULLS LAST LIMIT 1) AS "Qualification",
					e.email AS "Email",
					e.phone AS "Phone",
					e.alternate_phone AS "AlternatePhone",
					e.address AS "Address",
					e.emergency_contact_name AS "EmergencyContactName",
					e.emergency_contact_phone AS "EmergencyContactPhone",
					e.hire_date AS "HireDate",
					e.employment_type_code AS "EmploymentTypeCode",
					e.staff_type AS "StaffType",
					e.status AS "Status"
					FROM hr.employee e
					LEFT JOIN org.department d ON d.tenant_id = e.tenant_id AND d.department_id = e.department_id AND d.is_active = TRUE
					WHERE e.tenant_id = @TenantId
					  AND e.is_active = TRUE
					ORDER BY e.employee_id
					LIMIT @PageSize OFFSET @Offset;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken);
		
				var parameters = new
				{
					TenantId = tenantId,
					PageSize = pageSize,
					Offset = (page - 1) * pageSize
				};
		
				var totalCount = await connection.ExecuteScalarAsync<long>(
					new CommandDefinition(
						countSql,
						parameters,
						cancellationToken: cancellationToken));
		
				var items = (await connection.QueryAsync<Response>(
					new CommandDefinition(
						pageSql,
						parameters,
						cancellationToken: cancellationToken)))
					.AsList();
		
				return new PagedResult<Response>(
					items,
					page,
					pageSize,
					totalCount);
			}
	}

	public sealed class Handler(IGetEmployeePage dataAccess)
		: IRequestHandler<Query, Result<PagedResult<Response>>>
	{
		public async Task<Result<PagedResult<Response>>> HandleAsync(Query request, CancellationToken cancellationToken)
		{
			var pageRequest = new PageRequest(request.Page, request.PageSize);
			var page = await dataAccess.GetPageAsync(
				request.TenantId, pageRequest.NormalizedPage, pageRequest.NormalizedPageSize, cancellationToken);
			var response = new PagedResult<Response>(
				page.Items, page.Page, page.PageSize, page.TotalCount);
			return Result<PagedResult<Response>>.Success(response);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "employee"),
				async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
						new Query(tenantId, page, pageSize), cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetEmployeePage").WithTags(ModuleConstants.Name).RequireAuthorization();
		return endpoints;
	}
}
