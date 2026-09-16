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

public static class GetExamPage
{
    public sealed record Query(Guid TenantId, int Page = 1, int PageSize = 25) : IRequest<Result<PagedResult<Item>>>;
    public sealed record Item(Guid Id, string Name, string Code, string ExamTypeCode, string ClassSection,
        DateOnly? StartDate, DateOnly? EndDate, string Status, long SubjectCount);
    public interface IGetExamPageQuery { Task<PagedResult<Item>> GetAsync(Query request, CancellationToken cancellationToken); }
    internal sealed class GetExamPageQuery(IDbConnectionFactory factory, ICurrentUser user) : IGetExamPageQuery
    {
        public async Task<PagedResult<Item>> GetAsync(Query request, CancellationToken cancellationToken)
        {
            const string filter = """
                FROM exam.exam e LEFT JOIN academic.class_section cs ON cs.class_section_id = e.class_section_id AND cs.tenant_id = e.tenant_id
                WHERE e.tenant_id = @TenantId AND e.is_active AND (@CampusId IS NULL OR e.campus_id = @CampusId)
                    AND (@Manage OR (e.status = 'PUBLISHED' AND EXISTS (SELECT 1 FROM student.student_enrollment en
                        WHERE en.tenant_id = e.tenant_id AND en.class_section_id = e.class_section_id AND en.is_active
                        AND (en.student_id = @StudentId OR EXISTS (SELECT 1 FROM student.student_guardian sg
                            JOIN student.guardian g ON g.guardian_id = sg.guardian_id AND g.tenant_id = sg.tenant_id
                            WHERE sg.tenant_id = en.tenant_id AND sg.student_id = en.student_id AND sg.is_active
                                AND sg.can_view_academics AND g.user_id = @UserId AND g.is_active)))))
                """;
            const string projection = """
                SELECT e.exam_id AS "Id", e.name AS "Name", e.code AS "Code", e.exam_type_code AS "ExamTypeCode",
                    coalesce(cs.name, '') AS "ClassSection", e.start_date AS "StartDate", e.end_date AS "EndDate", e.status AS "Status",
                    (SELECT count(*) FROM exam.exam_subject es WHERE es.exam_id = e.exam_id AND es.tenant_id = e.tenant_id AND es.is_active) AS "SubjectCount"
                """;
            var page = Math.Max(1, request.Page);
            var size = Math.Clamp(request.PageSize, 1, 100);
            var parameters = new { request.TenantId, CampusId = user.BranchId,
                Manage = Authorization.ExamPermissions.CanManage(user), user.StudentId, user.UserId, Size = size, Offset = (page - 1) * size };
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            var total = await connection.ExecuteScalarAsync<long>(new CommandDefinition("SELECT count(*) " + filter, parameters, cancellationToken: cancellationToken));
            var rows = await connection.QueryAsync<Item>(new CommandDefinition(projection + filter +
                " ORDER BY e.start_date DESC, e.exam_id LIMIT @Size OFFSET @Offset", parameters, cancellationToken: cancellationToken));
            return new(rows.AsList(), page, size, total);
        }
    }
    public sealed class Handler(IGetExamPageQuery query) : IRequestHandler<Query, Result<PagedResult<Item>>>
    {
        public async Task<Result<PagedResult<Item>>> HandleAsync(Query request, CancellationToken cancellationToken) =>
            Result<PagedResult<Item>>.Success(await query.GetAsync(request, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/examinations/exam", async (Guid? tenantId, int? page, int? pageSize, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(tenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Query, Result<PagedResult<Item>>>(new(tenant.Value, page ?? 1, pageSize ?? 25), cancellationToken)).ToHttpResult();
        }).WithName("GetExamPage").WithTags("Examinations").RequireAuthorization();
        return endpoints;
    }
}
