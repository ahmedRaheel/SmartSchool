using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Api.Integration;

/// <summary>
/// Keeps Payroll-owned read projections synchronized from HR while SmartSchool is deployed
/// as a modular monolith. When Payroll is extracted as a service, replace this adapter with
/// HR integration-event consumers without changing the Payroll module.
/// </summary>
public sealed class PayrollHrProjectionSyncService(
    IServiceScopeFactory scopeFactory,
    ILogger<PayrollHrProjectionSyncService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var connectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
                await using var connection = await connectionFactory.OpenConnectionAsync(stoppingToken);

                const string sql = """
                    INSERT INTO payroll.employee_projection
                    (
                        employee_id,
                        tenant_id,
                        branch_id,
                        employee_number,
                        first_name,
                        last_name,
                        status,
                        is_active
                    )
                    SELECT
                        employee_id,
                        tenant_id,
                        branch_id,
                        employee_number,
                        first_name,
                        last_name,
                        status,
                        is_active
                    FROM hr.employee
                    ON CONFLICT (employee_id)
                    DO UPDATE SET
                        tenant_id = EXCLUDED.tenant_id,
                        branch_id = EXCLUDED.branch_id,
                        employee_number = EXCLUDED.employee_number,
                        first_name = EXCLUDED.first_name,
                        last_name = EXCLUDED.last_name,
                        status = EXCLUDED.status,
                        is_active = EXCLUDED.is_active;

                    INSERT INTO payroll.job_grade_projection
                    (
                        job_grade_id,
                        tenant_id,
                        code,
                        name,
                        is_active
                    )
                    SELECT
                        job_grade_id,
                        tenant_id,
                        code,
                        name,
                        is_active
                    FROM hr.job_grade
                    ON CONFLICT (job_grade_id)
                    DO UPDATE SET
                        tenant_id = EXCLUDED.tenant_id,
                        code = EXCLUDED.code,
                        name = EXCLUDED.name,
                        is_active = EXCLUDED.is_active;
                    """;

                await connection.ExecuteAsync(
                    new CommandDefinition(sql, cancellationToken: stoppingToken));
            }
            catch (Exception exception) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogWarning(
                    exception,
                    "Payroll HR projections could not be synchronized. The service will retry.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
