using Dapper;
using SmartSchool.Application.Persistence;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Enrollment;

public static class GetEnrollmentById
{
    /// <summary>
    /// Represents the response returned by this EnrollmentEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    Guid StudentId,
    Guid AcademicYearId,
    Guid ClassSectionId,
    DateOnly EnrollmentDate,
    string Status,
    string? AcademicYearCode,
    string? AcademicYearName,
    string? ClassSectionCode,
    string? ClassSectionName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;    public interface IGetEnrollmentByIdQuery
    {
        Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken);
    }



    internal sealed class GetEnrollmentByIdQuery(IDbConnectionFactory connectionFactory) : IGetEnrollmentByIdQuery
    {
        public async Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT entity.tenant_id AS "TenantId", entity.student_enrollment_id AS "Id", entity.student_id AS "StudentId", entity.academic_year_id AS "AcademicYearId", entity.class_section_id AS "ClassSectionId", entity.enrollment_date AS "EnrollmentDate", entity.status AS "Status",
                        p1.code AS "AcademicYearCode",
                        p1.name AS "AcademicYearName",
                        p2.code AS "ClassSectionCode",
                        p2.name AS "ClassSectionName"
                FROM student.student_enrollment AS entity
                    LEFT JOIN academic.academic_year AS p1
                        ON p1.academic_year_id = entity.academic_year_id
                    LEFT JOIN academic.class_section AS p2
                        ON p2.class_section_id = entity.class_section_id
                WHERE entity.tenant_id = @TenantId
                  AND entity.student_enrollment_id = @Id
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

    public sealed class Handler(IGetEnrollmentByIdQuery query)
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
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "enrollment"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetEnrollmentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
        return endpoints;
    }
}
