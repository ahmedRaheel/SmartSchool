using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class GetEmployeeById
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid? UserId,
        string? EmployeeNumber,
        string FirstName,
        string? LastName,
        string? CnicNumber,
        byte[]? Photo,
        string? PhotoContentType,
        string? PhotoFileName,
        string? Email,
        string? Phone,
        DateOnly HireDate,
        string EmploymentTypeCode,
        string Status,
        Guid? SourceCandidateId,
    Guid? SchoolId,
    string? SchoolCode,
    string? SchoolName,
    Guid? DepartmentId,
    string? DepartmentCode,
    string? DepartmentName);

    public sealed record Query(Guid TenantId, Guid Id) : IRequest<Result<Response>>;    public interface IGetEmployeeByIdQuery
    {
        Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken);
    }



    internal sealed class GetEmployeeByIdQuery(IDbConnectionFactory connectionFactory) : IGetEmployeeByIdQuery
    {
        public async Task<Result<Response>> ExecuteAsync(Query request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT entity.tenant_id AS "TenantId", entity.employee_id AS "Id", entity.user_id AS "UserId", entity.employee_number AS "EmployeeNumber", entity.first_name AS "FirstName", entity.last_name AS "LastName", entity.cnic_number AS "CnicNumber", entity.photo AS "Photo", entity.photo_content_type AS "PhotoContentType", entity.photo_file_name AS "PhotoFileName", entity.email AS "Email", entity.phone AS "Phone", entity.hire_date AS "HireDate", entity.employment_type_code AS "EmploymentTypeCode", entity.status AS "Status", entity.source_candidate_id AS "SourceCandidateId",
                        p1.school_id AS "SchoolId",
                        p1.code AS "SchoolCode",
                        p1.name AS "SchoolName",
                        p2.department_id AS "DepartmentId",
                        p2.code AS "DepartmentCode",
                        p2.name AS "DepartmentName"
                FROM hr.employee AS entity
                    LEFT JOIN org.school AS p1
                        ON p1.school_id = entity.school_id
                    LEFT JOIN org.department AS p2
                        ON p2.department_id = entity.department_id
                WHERE entity.tenant_id = @TenantId
                  AND entity.employee_id = @Id
                  AND entity.is_active = TRUE;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var response = await connection.QuerySingleOrDefaultAsync<Response>(
                new CommandDefinition(sql, new { request.TenantId, request.Id }, cancellationToken: cancellationToken));

            if (response is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Response))));
            }

            return Result<Response>.Success(response);
        }
    }

    public sealed class Handler(IGetEmployeeByIdQuery query)
        : IRequestHandler<Query, Result<Response>>
    {
        public Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            return query.ExecuteAsync(request, cancellationToken);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "employee"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<Response>>(new Query(tenantId, id), cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetEmployeeById").WithTags(ModuleConstants.Name).RequireAuthorization();
        return endpoints;
    }
}
