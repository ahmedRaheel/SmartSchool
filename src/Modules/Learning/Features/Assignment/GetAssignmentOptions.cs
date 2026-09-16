using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Authorization;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.Assignment;

public static class GetAssignmentOptions
{
    public sealed record Query(Guid TenantId) : IRequest<Result<Response>>;
    public sealed record Item(Guid Id, Guid ClassSectionId, Guid CourseOfferingId,
        Guid TeacherEmployeeId, Guid BranchId, string ClassSection, string Course, string Teacher);
    public sealed record Response(IReadOnlyList<Item> Items);
    public interface IGetAssignmentOptionsQuery { Task<Response> GetAsync(Guid tenantId, CancellationToken cancellationToken); }
    internal sealed class GetAssignmentOptionsQuery(IDbConnectionFactory factory, ICurrentUser user) : IGetAssignmentOptionsQuery
    {
        public async Task<Response> GetAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT ta.teacher_course_assignment_id AS "Id", cs.class_section_id AS "ClassSectionId",
                    co.course_offering_id AS "CourseOfferingId", ta.employee_id AS "TeacherEmployeeId",
                    cs.campus_id AS "BranchId", cs.name AS "ClassSection",
                    COALESCE(co.display_name, co.name) AS "Course",
                    trim(e.first_name || ' ' || coalesce(e.last_name, '')) AS "Teacher"
                FROM academic.teacher_course_assignment ta
                JOIN academic.course_offering co ON co.course_offering_id = ta.course_offering_id
                    AND co.tenant_id = ta.tenant_id AND co.is_active
                JOIN academic.class_section cs ON cs.class_section_id = ta.class_section_id
                    AND cs.tenant_id = ta.tenant_id AND cs.is_active
                JOIN hr.employee e ON e.employee_id = ta.employee_id AND e.tenant_id = ta.tenant_id AND e.is_active
                WHERE ta.tenant_id = @TenantId AND ta.is_active
                    AND (@CampusId IS NULL OR cs.campus_id = @CampusId)
                    AND (@ManageAll OR ta.employee_id = @EmployeeId)
                    AND (ta.effective_from IS NULL OR ta.effective_from <= CURRENT_DATE)
                    AND (ta.effective_to IS NULL OR ta.effective_to >= CURRENT_DATE)
                ORDER BY cs.name, co.name, e.first_name;
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync<Item>(new CommandDefinition(sql, new {
                TenantId = tenantId, CampusId = user.BranchId, ManageAll = LearningPermissions.CanManageAll(user),
                EmployeeId = user.EmployeeId ?? user.TeacherId }, cancellationToken: cancellationToken));
            return new(rows.AsList());
        }
    }
    public sealed class Handler(IGetAssignmentOptionsQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken) =>
            Result<Response>.Success(await query.GetAsync(request.TenantId, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/learning/assignment-options", async (Guid? tenantId, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(tenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Query, Result<Response>>(new(tenant.Value), cancellationToken)).ToHttpResult();
        }).WithName("GetAssignmentOptions").WithTags("Learning").RequireAuthorization(SmartSchoolPolicies.AcademicManagement);
        return endpoints;
    }
}
