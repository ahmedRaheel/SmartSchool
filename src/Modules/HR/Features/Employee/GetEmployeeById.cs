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
        Guid? SourceCandidateId);

    public sealed record Query(Guid TenantId, Guid Id) : IRequest<Result<Response>>;

    public sealed class Handler(IDbConnectionFactory connectionFactory) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT tenant_id AS "TenantId", employee_id AS "Id", user_id AS "UserId", employee_number AS "EmployeeNumber", first_name AS "FirstName", last_name AS "LastName", cnic_number AS "CnicNumber", photo AS "Photo", photo_content_type AS "PhotoContentType", photo_file_name AS "PhotoFileName", email AS "Email", phone AS "Phone", hire_date AS "HireDate", employment_type_code AS "EmploymentTypeCode", status AS "Status", source_candidate_id AS "SourceCandidateId"
                FROM hr.employee
                WHERE tenant_id = @TenantId
                  AND employee_id = @Id
                  AND is_active = TRUE;
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
