using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class GetStaffByCurrentCampus
{
    public sealed record Response(
        Guid EmployeeId,
        string? EmployeeNumber,
        string FirstName,
        string? LastName,
        string StaffType,
        string Status);

    public sealed record Query(
        Guid TenantId,
        Guid CampusId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetStaffByCurrentCampusQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetStaffByCurrentCampusQuery(
        IDbConnectionFactory connectionFactory)
        : IGetStaffByCurrentCampusQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    employee_id AS "EmployeeId",
                    employee_number AS "EmployeeNumber",
                    first_name AS "FirstName",
                    last_name AS "LastName",
                    staff_type AS "StaffType",
                    status AS "Status"
                FROM hr.employee
                WHERE tenant_id = @TenantId
                  AND branch_id = @CampusId
                  AND is_active = TRUE
                ORDER BY first_name, last_name;
                """;

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken);

            var employees = await connection.QueryAsync<Response>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        TenantId = tenantId,
                        CampusId = campusId
                    },
                    cancellationToken: cancellationToken));

            return employees.AsList();
        }
    }

    public sealed class Handler(
        IGetStaffByCurrentCampusQuery query)
        : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var employees = await query.GetAsync(
                request.TenantId,
                request.CampusId,
                cancellationToken);

            return Result<IReadOnlyCollection<Response>>.Success(employees);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/hr/employee/by-current-campus",
                async (
                    ICurrentUser currentUser,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    if (currentUser.TenantId is not Guid tenantId ||
                        currentUser.BranchId is not Guid campusId)
                    {
                        return Results.Forbid();
                    }

                    var query = new Query(tenantId, campusId);
                    var result = await mediator.SendAsync<
                        Query,
                        Result<IReadOnlyCollection<Response>>>(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetStaffByCurrentCampus")
            .WithTags("HR")
            .RequireAuthorization();

        return endpoints;
    }
}
