using SmartSchool.Modules.HR.Persistence;
using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Employee;

/// <summary>
/// Executes database reads for <see cref="EmployeeEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class EmployeeQuery(
	IHRDbContext dbContext,
	IDbConnectionFactory connectionFactory) : IEmployeeQuery
{
	public Task<EmployeeEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext.Employees
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.EmployeeId == id,
				cancellationToken);
	}

	public async Task<PagedResult<EmployeeEntity>> GetPageAsync(
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

		var items = (await connection.QueryAsync<EmployeeEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)))
			.AsList();

		return new PagedResult<EmployeeEntity>(
			items,
			page,
			pageSize,
			totalCount);
	}

	public async Task<string?> GetBranchCodeAsync(
		Guid tenantId,
		Guid branchId,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT code
			FROM org.campus
			WHERE tenant_id = @TenantId
			  AND campus_id = @BranchId
			  AND is_active = TRUE;
			""";

		await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
		return await connection.ExecuteScalarAsync<string?>(
			new CommandDefinition(sql, new { TenantId = tenantId, BranchId = branchId }, cancellationToken: cancellationToken));
	}

	public Task<bool> ExistsByEmployeeNumberAsync(
		Guid tenantId,
		string employeeNumber,
		Guid? excludingId,
		CancellationToken cancellationToken)
	{
		return dbContext.Employees
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId && entity.EmployeeNumber == employeeNumber
					&& (!excludingId.HasValue || (excludingId.HasValue && entity.EmployeeId != excludingId.Value)),
				cancellationToken);
	}
}
