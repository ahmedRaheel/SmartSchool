using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Features.Compliance;

/// <summary>Counts mandatory documents against active people in the selected tenant and campus.</summary>
public static class GetDocumentCompliance
{
    public sealed record Query(Guid TenantId, Guid? CampusId) : IRequest<Response>;
    public sealed record Item(string Type, long Total, long Compliant, long Pending);
    public sealed record Response(IReadOnlyList<Item> Items);

    public interface IGetDocumentCompliance
    {
        Task<Response> ExecuteAsync(Query query, CancellationToken cancellationToken);
    }

    internal sealed class Read(IDbConnectionFactory connectionFactory) : IGetDocumentCompliance
    {
        public async Task<Response> ExecuteAsync(Query query, CancellationToken cancellationToken)
        {
            const string sql = """
                WITH actors AS (
                    SELECT student_id AS id, branch_id AS campus_id, 'STUDENT' AS actor_type, 'StudentDocument' AS owner_type
                    FROM student.student WHERE tenant_id = @TenantId AND is_active = TRUE
                    UNION ALL
                    SELECT employee_id, branch_id,
                        CASE WHEN staff_type = 'TEACHER' THEN 'TEACHER'
                             WHEN staff_type = 'ADMIN_OFFICER' THEN 'ADMIN_OFFICER' ELSE 'EMPLOYEE' END,
                        CASE WHEN staff_type = 'TEACHER' THEN 'TeacherDocument' ELSE 'EmployeeDocument' END
                    FROM hr.employee WHERE tenant_id = @TenantId AND is_active = TRUE AND staff_type <> 'DRIVER'
                    UNION ALL
                    SELECT d.driver_id, e.branch_id, 'DRIVER', 'DriverDocument'
                    FROM transport.driver d
                    LEFT JOIN hr.employee e ON e.employee_id = d.employee_id AND e.tenant_id = d.tenant_id
                    WHERE d.tenant_id = @TenantId AND d.is_active = TRUE
                ), missing AS (
                    SELECT a.id, a.actor_type, COUNT(r.required_document_id) AS pending
                    FROM actors a
                    LEFT JOIN document.required_document r
                        ON r.tenant_id = @TenantId AND r.is_active = TRUE AND r.is_mandatory = TRUE
                        AND UPPER(r.user_role) = a.actor_type
                        AND (r.campus_id IS NULL OR r.campus_id = a.campus_id)
                        AND NOT EXISTS (
                            SELECT 1 FROM document.document d
                            WHERE d.tenant_id = @TenantId AND d.owner_id = a.id
                                AND d.owner_type = a.owner_type AND d.is_active = TRUE AND d.status = 'ACTIVE'
                                AND d.required_document_type_id = r.required_document_type_id
                        )
                    WHERE @CampusId IS NULL OR a.campus_id = @CampusId
                    GROUP BY a.id, a.actor_type
                )
                SELECT actor_type AS "Type", COUNT(*) AS "Total",
                    COUNT(*) FILTER (WHERE pending = 0) AS "Compliant",
                    COALESCE(SUM(pending), 0)::bigint AS "Pending"
                FROM missing GROUP BY actor_type ORDER BY actor_type;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var items = (await connection.QueryAsync<Item>(new CommandDefinition(sql, query,
                cancellationToken: cancellationToken))).AsList();
            return new Response(items);
        }
    }

    public sealed class Handler(IGetDocumentCompliance query) : IRequestHandler<Query, Response>
    {
        public Task<Response> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            return query.ExecuteAsync(request, cancellationToken);
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/documents/compliance",
            async (Guid? tenantId, ICurrentUser currentUser, ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
            {
                if (tenantScope.Resolve(tenantId) is not Guid resolvedTenantId)
                {
                    return Results.BadRequest(new { message = "Tenant context is required." });
                }
                var response = await mediator.SendAsync<Query, Response>(new Query(resolvedTenantId, currentUser.BranchId), cancellationToken);
                return Results.Ok(response);
            })
            .WithName("GetDocumentCompliance")
            .WithTags("Documents")
            .RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
    }
}
