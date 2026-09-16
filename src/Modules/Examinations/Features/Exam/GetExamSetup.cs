using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.Exam;

public static class GetExamSetup
{
    public sealed record Query(Guid TenantId) : IRequest<Result<Response>>;
    public sealed record Item(Guid ClassSectionId, string ClassSection, Guid CourseOfferingId, string Course);
    public sealed record Response(IReadOnlyList<Item> Items);
    public interface IGetExamSetupQuery { Task<Response> GetAsync(Query request, CancellationToken cancellationToken); }
    internal sealed class GetExamSetupQuery(IDbConnectionFactory factory, ICurrentUser user) : IGetExamSetupQuery
    {
        public async Task<Response> GetAsync(Query request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT DISTINCT cs.class_section_id AS "ClassSectionId", cs.name AS "ClassSection",
                    co.course_offering_id AS "CourseOfferingId", coalesce(co.display_name, co.name) AS "Course"
                FROM academic.teacher_course_assignment ta
                JOIN academic.class_section cs ON cs.class_section_id = ta.class_section_id AND cs.tenant_id = ta.tenant_id AND cs.is_active
                JOIN academic.course_offering co ON co.course_offering_id = ta.course_offering_id AND co.tenant_id = ta.tenant_id AND co.is_active
                WHERE ta.tenant_id = @TenantId AND ta.is_active AND (@CampusId IS NULL OR cs.campus_id = @CampusId)
                ORDER BY cs.name, "Course";
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return new((await connection.QueryAsync<Item>(new CommandDefinition(sql,
                new { request.TenantId, CampusId = user.BranchId }, cancellationToken: cancellationToken))).AsList());
        }
    }
    public sealed class Handler(IGetExamSetupQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken) =>
            Result<Response>.Success(await query.GetAsync(request, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/examinations/setup", async (Guid? tenantId, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(tenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Query, Result<Response>>(new(tenant.Value), cancellationToken)).ToHttpResult();
        }).WithName("GetExamSetup").WithTags("Examinations").RequireAuthorization(SmartSchoolPolicies.ExaminationManagement);
        return endpoints;
    }
}
