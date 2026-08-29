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
		string? Email,
		string? Phone,
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

	internal sealed class GetEmployeePageDataAccess(		
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
					FROM hr.employee
					WHERE tenant_id = @TenantId
					  AND is_active = TRUE;
					""";
		
				const string pageSql = """
					SELECT
					tenant_id AS "TenantId",
					employee_id AS "Id",
					employee_number AS "EmployeeNumber",
					first_name AS "FirstName",
					last_name AS "LastName",
					cnic_number AS "CnicNumber",
					email AS "Email",
					phone AS "Phone",
					hire_date AS "HireDate",
					employment_type_code AS "EmploymentTypeCode",
					staff_type AS "StaffType",
					status AS "Status"
					FROM hr.employee
					WHERE tenant_id = @TenantId
					  AND is_active = TRUE
					ORDER BY employee_id
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
