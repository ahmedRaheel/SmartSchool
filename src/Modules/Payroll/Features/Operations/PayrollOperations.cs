using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Payroll.Features.Operations;

/// <summary>
/// Operational payroll workflow built on effective-dated employee
/// compensation, payroll periods, payroll runs and persisted payslip snapshots.
/// </summary>
public static class PayrollOperations
{
    public sealed record CompensationItem(
        Guid CompensationId,
        Guid EmployeeId,
        string EmployeeNumber,
        string EmployeeName,
        DateOnly EffectiveFrom,
        DateOnly? EffectiveTo,
        decimal BasicSalary,
        decimal GrossSalary,
        string CurrencyCode,
        string Status);

    public sealed record RunItem(
        Guid RunId,
        Guid PeriodId,
        int Year,
        int Month,
        string Status,
        DateTimeOffset CreatedAt,
        DateTimeOffset? ApprovedAt,
        long EmployeeCount,
        decimal GrossAmount,
        decimal NetAmount);

    public sealed record PayslipItem(
        Guid EmployeePayrollId,
        Guid RunId,
        Guid EmployeeId,
        string EmployeeNumber,
        string EmployeeName,
        int Year,
        int Month,
        decimal GrossAmount,
        decimal DeductionAmount,
        decimal NetAmount,
        string RunStatus);

    public sealed record DashboardQuery(Guid TenantId) : IRequest<Result<DashboardResponse>>;
    public sealed record DashboardResponse(
        IReadOnlyList<CompensationItem> Compensations,
        IReadOnlyList<RunItem> Runs,
        IReadOnlyList<PayslipItem> Payslips);

    public interface IPayrollDashboardQuery
    {
        Task<DashboardResponse> GetAsync(
            Guid tenantId,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class PayrollDashboardQuery(IDbConnectionFactory connectionFactory)
        : IPayrollDashboardQuery
    {
        public async Task<DashboardResponse> GetAsync(
            Guid tenantId,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    c.employee_compensation_id AS "CompensationId",
                    c.employee_id AS "EmployeeId",
                    e.employee_number AS "EmployeeNumber",
                    trim(e.first_name || ' ' || coalesce(e.last_name, '')) AS "EmployeeName",
                    c.effective_from AS "EffectiveFrom",
                    c.effective_to AS "EffectiveTo",
                    c.basic_salary AS "BasicSalary",
                    coalesce(c.gross_salary, c.basic_salary) AS "GrossSalary",
                    trim(c.currency_code) AS "CurrencyCode",
                    c.status AS "Status"
                FROM payroll.employee_compensation c
                JOIN payroll.employee_projection e
                  ON e.employee_id = c.employee_id
                 AND e.tenant_id = c.tenant_id
                WHERE c.tenant_id = @TenantId
                  AND c.is_active
                  AND e.is_active
                  AND (@CampusId IS NULL OR e.branch_id = @CampusId)
                ORDER BY e.first_name, c.effective_from DESC;

                SELECT
                    r.payroll_run_id AS "RunId",
                    r.payroll_period_id AS "PeriodId",
                    p.year AS "Year",
                    p.month AS "Month",
                    r.status_code AS "Status",
                    r.created_at AS "CreatedAt",
                    r.approved_at AS "ApprovedAt",
                    count(ep.employee_payroll_id) AS "EmployeeCount",
                    coalesce(sum(ep.gross_amount), 0) AS "GrossAmount",
                    coalesce(sum(ep.net_amount), 0) AS "NetAmount"
                FROM payroll.payroll_run r
                JOIN payroll.payroll_period p
                  ON p.payroll_period_id = r.payroll_period_id
                 AND p.tenant_id = r.tenant_id
                LEFT JOIN payroll.employee_payroll ep
                  ON ep.payroll_run_id = r.payroll_run_id
                LEFT JOIN payroll.employee_projection e
                  ON e.employee_id = ep.employee_id
                 AND e.tenant_id = r.tenant_id
                WHERE r.tenant_id = @TenantId
                  AND r.is_active
                  AND p.is_active
                  AND (@CampusId IS NULL OR ep.employee_payroll_id IS NULL OR e.branch_id = @CampusId)
                GROUP BY r.payroll_run_id, r.payroll_period_id, p.year, p.month, r.status_code, r.created_at, r.approved_at
                ORDER BY p.year DESC, p.month DESC, r.created_at DESC;

                SELECT
                    ep.employee_payroll_id AS "EmployeePayrollId",
                    r.payroll_run_id AS "RunId",
                    ep.employee_id AS "EmployeeId",
                    e.employee_number AS "EmployeeNumber",
                    trim(e.first_name || ' ' || coalesce(e.last_name, '')) AS "EmployeeName",
                    p.year AS "Year",
                    p.month AS "Month",
                    ep.gross_amount AS "GrossAmount",
                    ep.deduction_amount AS "DeductionAmount",
                    ep.net_amount AS "NetAmount",
                    r.status_code AS "RunStatus"
                FROM payroll.employee_payroll ep
                JOIN payroll.payroll_run r
                  ON r.payroll_run_id = ep.payroll_run_id
                JOIN payroll.payroll_period p
                  ON p.payroll_period_id = r.payroll_period_id
                 AND p.tenant_id = r.tenant_id
                JOIN payroll.employee_projection e
                  ON e.employee_id = ep.employee_id
                 AND e.tenant_id = r.tenant_id
                WHERE r.tenant_id = @TenantId
                  AND r.is_active
                  AND p.is_active
                  AND e.is_active
                  AND (@CampusId IS NULL OR e.branch_id = @CampusId)
                ORDER BY p.year DESC, p.month DESC, e.first_name
                LIMIT 1000;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            using var grid = await connection.QueryMultipleAsync(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, CampusId = campusId },
                    cancellationToken: cancellationToken));

            return new DashboardResponse(
                (await grid.ReadAsync<CompensationItem>()).AsList(),
                (await grid.ReadAsync<RunItem>()).AsList(),
                (await grid.ReadAsync<PayslipItem>()).AsList());
        }
    }

    public sealed class DashboardHandler(
        IPayrollDashboardQuery query,
        ICurrentUser currentUser) : IRequestHandler<DashboardQuery, Result<DashboardResponse>>
    {
        public async Task<Result<DashboardResponse>> HandleAsync(
            DashboardQuery request,
            CancellationToken cancellationToken)
        {
            return Result<DashboardResponse>.Success(
                await query.GetAsync(request.TenantId, currentUser.BranchId, cancellationToken));
        }
    }

    public sealed record SaveCompensationRequest(
        Guid TenantId,
        Guid EmployeeId,
        Guid? JobGradeId,
        DateOnly EffectiveFrom,
        decimal BasicSalary,
        decimal? GrossSalary,
        string CurrencyCode = "PKR") : IRequest<Result<SaveCompensationResponse>>;

    public sealed record SaveCompensationResponse(Guid CompensationId);

    public sealed class SaveCompensationValidator : AbstractValidator<SaveCompensationRequest>
    {
        public SaveCompensationValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.EmployeeId).NotEmpty();
            RuleFor(request => request.BasicSalary).GreaterThan(0);
            RuleFor(request => request.GrossSalary)
                .GreaterThanOrEqualTo(request => request.BasicSalary)
                .When(request => request.GrossSalary.HasValue);
            RuleFor(request => request.CurrencyCode)
                .NotEmpty()
                .Length(3);
        }
    }

    public interface ISaveCompensationQuery
    {
        Task<bool> EmployeeExistsAsync(
            SaveCompensationRequest request,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class SaveCompensationQuery(IDbConnectionFactory connectionFactory)
        : ISaveCompensationQuery
    {
        public async Task<bool> EmployeeExistsAsync(
            SaveCompensationRequest request,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (
                    SELECT 1
                    FROM payroll.employee_projection
                    WHERE tenant_id = @TenantId
                      AND employee_id = @EmployeeId
                      AND is_active
                      AND upper(status) IN ('ACTIVE', 'APPROVED')
                      AND (@CampusId IS NULL OR branch_id = @CampusId)
                );
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    sql,
                    new { request.TenantId, request.EmployeeId, CampusId = campusId },
                    cancellationToken: cancellationToken));
        }
    }

    public interface ISaveCompensationCommand
    {
        Task<SaveCompensationResponse> SaveAsync(
            SaveCompensationRequest request,
            CancellationToken cancellationToken);
    }

    internal sealed class SaveCompensationCommand(IPayrollDbContext dbContext)
        : ISaveCompensationCommand
    {
        public async Task<SaveCompensationResponse> SaveAsync(
            SaveCompensationRequest request,
            CancellationToken cancellationToken)
        {
            var compensationId = Guid.NewGuid();
            var code = $"COMP-{compensationId:N}"[..18].ToUpperInvariant();
            var currency = request.CurrencyCode.Trim().ToUpperInvariant();
            var grossSalary = request.GrossSalary ?? request.BasicSalary;

            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE payroll.employee_compensation
                SET effective_to = {request.EffectiveFrom.AddDays(-1)},
                    status = {"SUPERSEDED"},
                    updated_at = now(),
                    row_version = gen_random_bytes(8)
                WHERE tenant_id = {request.TenantId}
                  AND employee_id = {request.EmployeeId}
                  AND is_active
                  AND upper(status) = {"ACTIVE"}
                  AND effective_from < {request.EffectiveFrom}
                  AND (effective_to IS NULL OR effective_to >= {request.EffectiveFrom});
                """, cancellationToken);

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO payroll.employee_compensation
                (
                    employee_compensation_id,
                    tenant_id,
                    employee_id,
                    job_grade_id,
                    effective_from,
                    basic_salary,
                    gross_salary,
                    currency_code,
                    status,
                    code,
                    name,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {compensationId},
                    {request.TenantId},
                    {request.EmployeeId},
                    {request.JobGradeId},
                    {request.EffectiveFrom},
                    {request.BasicSalary},
                    {grossSalary},
                    {currency},
                    {"ACTIVE"},
                    {code},
                    {"Employee compensation"},
                    TRUE,
                    now(),
                    gen_random_bytes(8)
                );
                """, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return new SaveCompensationResponse(compensationId);
        }
    }

    public sealed class SaveCompensationHandler(
        ISaveCompensationQuery query,
        ISaveCompensationCommand command,
        ICurrentUser currentUser) : IRequestHandler<SaveCompensationRequest, Result<SaveCompensationResponse>>
    {
        public async Task<Result<SaveCompensationResponse>> HandleAsync(
            SaveCompensationRequest request,
            CancellationToken cancellationToken)
        {
            if (!await query.EmployeeExistsAsync(request, currentUser.BranchId, cancellationToken))
            {
                return Result<SaveCompensationResponse>.Failure(Error.NotFound("Employee not found in the current scope."));
            }

            return Result<SaveCompensationResponse>.Success(
                await command.SaveAsync(request, cancellationToken));
        }
    }

    public sealed record CreateRunRequest(
        Guid TenantId,
        int Year,
        int Month) : IRequest<Result<CreateRunResponse>>;

    public sealed record CreateRunResponse(
        Guid RunId,
        Guid PeriodId,
        int EmployeeCount,
        decimal GrossAmount,
        decimal NetAmount);

    public sealed class CreateRunValidator : AbstractValidator<CreateRunRequest>
    {
        public CreateRunValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.Year).InclusiveBetween(2000, 2200);
            RuleFor(request => request.Month).InclusiveBetween(1, 12);
        }
    }

    public interface ICreatePayrollRunOperationsQuery
    {
        Task<bool> RunExistsAsync(
            CreateRunRequest request,
            CancellationToken cancellationToken);
    }

    internal sealed class CreatePayrollRunOperationsQuery(IDbConnectionFactory connectionFactory)
        : ICreatePayrollRunOperationsQuery
    {
        public async Task<bool> RunExistsAsync(
            CreateRunRequest request,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (
                    SELECT 1
                    FROM payroll.payroll_run r
                    JOIN payroll.payroll_period p
                      ON p.payroll_period_id = r.payroll_period_id
                     AND p.tenant_id = r.tenant_id
                    WHERE r.tenant_id = @TenantId
                      AND p.year = @Year
                      AND p.month = @Month
                      AND r.is_active
                      AND p.is_active
                );
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(sql, request, cancellationToken: cancellationToken));
        }
    }

    public interface ICreatePayrollRunOperationsCommand
    {
        Task<Result<CreateRunResponse>> CreateAsync(
            CreateRunRequest request,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class CreatePayrollRunOperationsCommand(IPayrollDbContext dbContext)
        : ICreatePayrollRunOperationsCommand
    {
        public async Task<Result<CreateRunResponse>> CreateAsync(
            CreateRunRequest request,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            var startDate = new DateOnly(request.Year, request.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var periodId = Guid.NewGuid();
            var runId = Guid.NewGuid();
            var runCode = $"PAYRUN-{request.Year:D4}{request.Month:D2}";

            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO payroll.payroll_period
                (
                    payroll_period_id,
                    tenant_id,
                    year,
                    month,
                    start_date,
                    end_date,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {periodId},
                    {request.TenantId},
                    {request.Year},
                    {request.Month},
                    {startDate},
                    {endDate},
                    TRUE,
                    now(),
                    gen_random_bytes(8)
                );
                """, cancellationToken);

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO payroll.payroll_run
                (
                    payroll_run_id,
                    tenant_id,
                    payroll_period_id,
                    status_code,
                    code,
                    name,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {runId},
                    {request.TenantId},
                    {periodId},
                    {"DRAFT"},
                    {runCode},
                    {$"Payroll {request.Year:D4}-{request.Month:D2}"},
                    TRUE,
                    now(),
                    gen_random_bytes(8)
                );
                """, cancellationToken);

            var inserted = await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO payroll.employee_payroll
                (
                    employee_payroll_id,
                    payroll_run_id,
                    employee_id,
                    gross_amount,
                    deduction_amount,
                    net_amount
                )
                SELECT
                    gen_random_uuid(),
                    {runId},
                    employee.employee_id,
                    coalesce(compensation.gross_salary, compensation.basic_salary),
                    0,
                    coalesce(compensation.gross_salary, compensation.basic_salary)
                FROM payroll.employee_projection employee
                JOIN LATERAL
                (
                    SELECT c.*
                    FROM payroll.employee_compensation c
                    WHERE c.tenant_id = employee.tenant_id
                      AND c.employee_id = employee.employee_id
                      AND c.is_active
                      AND upper(c.status) = {"ACTIVE"}
                      AND c.effective_from <= {endDate}
                      AND (c.effective_to IS NULL OR c.effective_to >= {startDate})
                    ORDER BY c.effective_from DESC, c.created_at DESC
                    LIMIT 1
                ) compensation ON TRUE
                WHERE employee.tenant_id = {request.TenantId}
                  AND employee.is_active
                  AND upper(employee.status) IN ({"ACTIVE"}, {"APPROVED"})
                  AND ({campusId} IS NULL OR employee.branch_id = {campusId});
                """, cancellationToken);

            if (inserted == 0)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result<CreateRunResponse>.Failure(
                    Error.Validation("No active employees with effective compensation were found for this payroll period."));
            }

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO payroll.payroll_line_item
                (
                    payroll_line_item_id,
                    employee_payroll_id,
                    salary_component_id,
                    description,
                    amount
                )
                SELECT
                    gen_random_uuid(),
                    employee_payroll_id,
                    NULL,
                    {"Gross salary"},
                    gross_amount
                FROM payroll.employee_payroll
                WHERE payroll_run_id = {runId};
                """, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            var totals = await GetRunTotalsAsync(dbContext, runId, cancellationToken);
            return Result<CreateRunResponse>.Success(
                new CreateRunResponse(
                    runId,
                    periodId,
                    totals.EmployeeCount,
                    totals.GrossAmount,
                    totals.NetAmount));
        }

        private static async Task<RunTotals> GetRunTotalsAsync(
            IPayrollDbContext dbContext,
            Guid runId,
            CancellationToken cancellationToken)
        {
            var connection = dbContext.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            const string sql = """
                SELECT
                    count(*)::int AS "EmployeeCount",
                    coalesce(sum(gross_amount), 0) AS "GrossAmount",
                    coalesce(sum(net_amount), 0) AS "NetAmount"
                FROM payroll.employee_payroll
                WHERE payroll_run_id = @RunId;
                """;

            return await connection.QuerySingleAsync<RunTotals>(
                new CommandDefinition(sql, new { RunId = runId }, cancellationToken: cancellationToken));
        }
    }

    public sealed record RunTotals(int EmployeeCount, decimal GrossAmount, decimal NetAmount);

    public sealed class CreateRunHandler(
        ICreatePayrollRunOperationsQuery query,
        ICreatePayrollRunOperationsCommand command,
        ICurrentUser currentUser) : IRequestHandler<CreateRunRequest, Result<CreateRunResponse>>
    {
        public async Task<Result<CreateRunResponse>> HandleAsync(
            CreateRunRequest request,
            CancellationToken cancellationToken)
        {
            if (await query.RunExistsAsync(request, cancellationToken))
            {
                return Result<CreateRunResponse>.Failure(
                    Error.Conflict("A payroll run already exists for this month."));
            }

            return await command.CreateAsync(request, currentUser.BranchId, cancellationToken);
        }
    }

    public sealed record ApproveRunRequest(Guid TenantId, Guid RunId)
        : IRequest<Result<ApproveRunResponse>>;

    public sealed record ApproveRunResponse(Guid RunId, DateTimeOffset ApprovedAt);

    public interface IApprovePayrollRunCommand
    {
        Task<Result<ApproveRunResponse>> ApproveAsync(
            ApproveRunRequest request,
            Guid approvedBy,
            CancellationToken cancellationToken);
    }

    internal sealed class ApprovePayrollRunCommand(IPayrollDbContext dbContext)
        : IApprovePayrollRunCommand
    {
        public async Task<Result<ApproveRunResponse>> ApproveAsync(
            ApproveRunRequest request,
            Guid approvedBy,
            CancellationToken cancellationToken)
        {
            var approvedAt = DateTimeOffset.UtcNow;
            var updated = await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE payroll.payroll_run
                SET status_code = {"APPROVED"},
                    approved_by = {approvedBy},
                    approved_at = {approvedAt},
                    updated_at = now(),
                    row_version = gen_random_bytes(8)
                WHERE tenant_id = {request.TenantId}
                  AND payroll_run_id = {request.RunId}
                  AND is_active
                  AND upper(status_code) = {"DRAFT"};
                """, cancellationToken);

            return updated == 1
                ? Result<ApproveRunResponse>.Success(new ApproveRunResponse(request.RunId, approvedAt))
                : Result<ApproveRunResponse>.Failure(
                    Error.Conflict("Only an existing draft payroll run can be approved."));
        }
    }

    public sealed class ApproveRunHandler(
        IApprovePayrollRunCommand command,
        ICurrentUser currentUser) : IRequestHandler<ApproveRunRequest, Result<ApproveRunResponse>>
    {
        public Task<Result<ApproveRunResponse>> HandleAsync(
            ApproveRunRequest request,
            CancellationToken cancellationToken) =>
            command.ApproveAsync(request, currentUser.UserId, cancellationToken);
    }

    public sealed record MyPayslipsQuery(Guid TenantId, Guid EmployeeId)
        : IRequest<Result<IReadOnlyList<PayslipItem>>>;

    public interface IMyPayslipsQuery
    {
        Task<IReadOnlyList<PayslipItem>> GetAsync(
            Guid tenantId,
            Guid employeeId,
            CancellationToken cancellationToken);
    }

    internal sealed class MyPayslipsQueryService(IDbConnectionFactory connectionFactory)
        : IMyPayslipsQuery
    {
        public async Task<IReadOnlyList<PayslipItem>> GetAsync(
            Guid tenantId,
            Guid employeeId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    ep.employee_payroll_id AS "EmployeePayrollId",
                    r.payroll_run_id AS "RunId",
                    ep.employee_id AS "EmployeeId",
                    e.employee_number AS "EmployeeNumber",
                    trim(e.first_name || ' ' || coalesce(e.last_name, '')) AS "EmployeeName",
                    p.year AS "Year",
                    p.month AS "Month",
                    ep.gross_amount AS "GrossAmount",
                    ep.deduction_amount AS "DeductionAmount",
                    ep.net_amount AS "NetAmount",
                    r.status_code AS "RunStatus"
                FROM payroll.employee_payroll ep
                JOIN payroll.payroll_run r ON r.payroll_run_id = ep.payroll_run_id
                JOIN payroll.payroll_period p
                  ON p.payroll_period_id = r.payroll_period_id
                 AND p.tenant_id = r.tenant_id
                JOIN payroll.employee_projection e
                  ON e.employee_id = ep.employee_id
                 AND e.tenant_id = r.tenant_id
                WHERE r.tenant_id = @TenantId
                  AND ep.employee_id = @EmployeeId
                  AND r.is_active
                  AND p.is_active
                  AND upper(r.status_code) = 'APPROVED'
                ORDER BY p.year DESC, p.month DESC;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<PayslipItem>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, EmployeeId = employeeId },
                    cancellationToken: cancellationToken))).AsList();
        }
    }

    public sealed class MyPayslipsHandler(IMyPayslipsQuery query)
        : IRequestHandler<MyPayslipsQuery, Result<IReadOnlyList<PayslipItem>>>
    {
        public async Task<Result<IReadOnlyList<PayslipItem>>> HandleAsync(
            MyPayslipsQuery request,
            CancellationToken cancellationToken) =>
            Result<IReadOnlyList<PayslipItem>>.Success(
                await query.GetAsync(request.TenantId, request.EmployeeId, cancellationToken));
    }

    public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/payroll/operations",
                async (
                    Guid? tenantId,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var resolvedTenantId = tenantScope.Resolve(tenantId);
                    if (!resolvedTenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<DashboardQuery, Result<DashboardResponse>>(
                        new DashboardQuery(resolvedTenantId.Value),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPayrollOperations")
            .WithTags("Payroll")
            .RequireAuthorization(SmartSchoolPolicies.PayrollManagement);

        endpoints.MapPost(
                "/api/payroll/operations/compensations",
                async (
                    SaveCompensationRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<SaveCompensationRequest, Result<SaveCompensationResponse>>(
                        request with { TenantId = tenantId.Value },
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("SaveOperationalEmployeeCompensation")
            .WithTags("Payroll")
            .RequireAuthorization(SmartSchoolPolicies.PayrollManagement);

        endpoints.MapPost(
                "/api/payroll/operations/runs",
                async (
                    CreateRunRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<CreateRunRequest, Result<CreateRunResponse>>(
                        request with { TenantId = tenantId.Value },
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateOperationalPayrollRun")
            .WithTags("Payroll")
            .RequireAuthorization(SmartSchoolPolicies.PayrollManagement);

        endpoints.MapPut(
                "/api/payroll/operations/runs/{runId:guid}/approve",
                async (
                    Guid runId,
                    Guid? tenantId,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var resolvedTenantId = tenantScope.Resolve(tenantId);
                    if (!resolvedTenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<ApproveRunRequest, Result<ApproveRunResponse>>(
                        new ApproveRunRequest(resolvedTenantId.Value, runId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("ApproveOperationalPayrollRun")
            .WithTags("Payroll")
            .RequireAuthorization(SmartSchoolPolicies.PayrollManagement);

        endpoints.MapGet(
                "/api/payroll/payslips/me",
                async (
                    Guid? tenantId,
                    ITenantScope tenantScope,
                    ICurrentUser currentUser,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var resolvedTenantId = tenantScope.Resolve(tenantId);
                    if (!resolvedTenantId.HasValue || !currentUser.EmployeeId.HasValue)
                    {
                        return Results.BadRequest(new { message = "An employee profile is required." });
                    }

                    var result = await mediator.SendAsync<MyPayslipsQuery, Result<IReadOnlyList<PayslipItem>>>(
                        new MyPayslipsQuery(resolvedTenantId.Value, currentUser.EmployeeId.Value),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetMyPayslips")
            .WithTags("Payroll")
            .RequireAuthorization(SmartSchoolPolicies.AllAuthenticatedActors);

        return endpoints;
    }
}
