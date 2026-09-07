using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Student;

public static class GetStudentPage
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        string? StudentNumber,
        string FirstName,
        string? LastName,
        DateOnly? DateOfBirth,
        string? Gender,
        DateOnly? AdmissionDate,
        string Status,
    Guid? SchoolId,
    string? SchoolCode,
    string? SchoolName);

    public sealed record Query(Guid TenantId, int Page = 1, int PageSize = 25)
        : IRequest<Result<PagedResult<Response>>>;

    public interface IGetStudentPageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetStudentPageQuery(
        IDbConnectionFactory connectionFactory) : IGetStudentPageQuery
    {
        public async Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken)
            {
                const string countSql = """
                    SELECT COUNT(*)
                    FROM student.student AS entity
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE;
                    """;

                const string pageSql = """
                    SELECT
                    entity.tenant_id AS "TenantId",
                    entity.student_id AS "Id",
                    entity.student_number AS "StudentNumber",
                    entity.first_name AS "FirstName",
                    entity.last_name AS "LastName",
                    entity.date_of_birth AS "DateOfBirth",
                    entity.gender AS "Gender",
                    entity.admission_date AS "AdmissionDate",
                    entity.status AS "Status",
                        p1.school_id AS "SchoolId",
                        p1.code AS "SchoolCode",
                        p1.name AS "SchoolName"
                    FROM student.student AS entity
                    LEFT JOIN org.school AS p1
                        ON p1.school_id = entity.school_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE
                    ORDER BY entity.student_id
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

    public sealed class Handler(IGetStudentPageQuery query)
        : IRequestHandler<Query, Result<PagedResult<Response>>>
    {
        public async Task<Result<PagedResult<Response>>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(request.Page, request.PageSize);
            var page = await query.GetPageAsync(
                request.TenantId, pageRequest.NormalizedPage, pageRequest.NormalizedPageSize, cancellationToken);
            var response = new PagedResult<Response>(
                page.Items, page.Page, page.PageSize, page.TotalCount);
            return Result<PagedResult<Response>>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        new Query(tenantId, page, pageSize), cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentPage").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
        return endpoints;
    }
}
