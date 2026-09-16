using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.TeachingAllocation;

public static class GetTeachingAllocationSetup
{
    public sealed record Query(Guid TenantId, Guid CampusId) : IRequest<Result<Response>>;
    public sealed record Lookup(Guid Id, string Name);
    public sealed record Allocation(Guid Id, string ClassSection, string Subject, string Teacher, int? PeriodsPerWeek);
    public sealed record Response(IReadOnlyList<Lookup> Sections, IReadOnlyList<Lookup> Subjects,
        IReadOnlyList<Lookup> Teachers, IReadOnlyList<Allocation> Allocations);
    public interface IGetTeachingAllocationSetupQuery { Task<Response> GetAsync(Query request, CancellationToken cancellationToken); }
    internal sealed class GetTeachingAllocationSetupQuery(IDbConnectionFactory factory) : IGetTeachingAllocationSetupQuery
    {
        public async Task<Response> GetAsync(Query request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT cs.class_section_id AS "Id", gl.name || ' — ' || cs.name || ' (' || y.name || ')' AS "Name"
                FROM academic.class_section cs JOIN academic.grade_level gl ON gl.grade_level_id = cs.grade_level_id AND gl.tenant_id = cs.tenant_id
                JOIN academic.academic_year y ON y.academic_year_id = cs.academic_year_id AND y.tenant_id = cs.tenant_id
                WHERE cs.tenant_id = @TenantId AND cs.campus_id = @CampusId AND cs.is_active ORDER BY y.start_date DESC, gl.sort_order, cs.name;
                SELECT s.subject_id AS "Id", s.name AS "Name" FROM academic.subject s
                JOIN org.department d ON d.department_id = s.department_id AND d.tenant_id = s.tenant_id AND d.is_active
                WHERE s.tenant_id = @TenantId AND d.campus_id = @CampusId AND s.is_active ORDER BY s.name;
                SELECT employee_id AS "Id", trim(first_name || ' ' || coalesce(last_name, '')) AS "Name"
                FROM hr.employee WHERE tenant_id = @TenantId AND branch_id = @CampusId AND is_active
                    AND upper(staff_type) = 'TEACHER' AND status IN ('ACTIVE', 'APPROVED') ORDER BY first_name;
                SELECT ta.teacher_course_assignment_id AS "Id", gl.name || ' — ' || cs.name AS "ClassSection",
                    coalesce(co.display_name, co.name) AS "Subject", trim(e.first_name || ' ' || coalesce(e.last_name, '')) AS "Teacher",
                    ta.periods_per_week AS "PeriodsPerWeek"
                FROM academic.teacher_course_assignment ta
                JOIN academic.class_section cs ON cs.class_section_id = ta.class_section_id AND cs.tenant_id = ta.tenant_id
                JOIN academic.grade_level gl ON gl.grade_level_id = cs.grade_level_id AND gl.tenant_id = cs.tenant_id
                JOIN academic.course_offering co ON co.course_offering_id = ta.course_offering_id AND co.tenant_id = ta.tenant_id
                JOIN hr.employee e ON e.employee_id = ta.employee_id AND e.tenant_id = ta.tenant_id
                WHERE ta.tenant_id = @TenantId AND cs.campus_id = @CampusId AND ta.is_active ORDER BY gl.sort_order, cs.name, co.name;
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            using var grid = await connection.QueryMultipleAsync(new CommandDefinition(sql, request, cancellationToken: cancellationToken));
            return new((await grid.ReadAsync<Lookup>()).AsList(), (await grid.ReadAsync<Lookup>()).AsList(),
                (await grid.ReadAsync<Lookup>()).AsList(), (await grid.ReadAsync<Allocation>()).AsList());
        }
    }
    public sealed class Handler(IGetTeachingAllocationSetupQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken) => Result<Response>.Success(await query.GetAsync(request, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/academics/teaching-allocations", async (Guid? tenantId, Guid campusId, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(tenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Query, Result<Response>>(new(tenant.Value, campusId), cancellationToken)).ToHttpResult();
        }).WithName("GetTeachingAllocationSetup").WithTags("Organization").RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
        return endpoints;
    }
}
