using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.DepartmentSubjectTeacher;

public static class GetDepartmentSubjectTeachers
{
    public sealed record Request(Guid TenantId, Guid DepartmentId) : IRequest<Result<IReadOnlyCollection<Response>>>;
    public sealed record Response(Guid Id, Guid DepartmentId, Guid SubjectId, string SubjectName, Guid TeacherId, string TeacherName, bool IsPrimary, DateOnly? EffectiveFrom, DateOnly? EffectiveTo);

    public sealed class Handler(IDbConnectionFactory connectionFactory) : IRequestHandler<Request, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT dst.department_subject_teacher_id AS "Id", dst.department_id AS "DepartmentId",
                       dst.subject_id AS "SubjectId", s.name AS "SubjectName", dst.teacher_id AS "TeacherId",
                       trim(concat(e.first_name, ' ', coalesce(e.last_name, ''))) AS "TeacherName",
                       dst.is_primary AS "IsPrimary", dst.effective_from AS "EffectiveFrom", dst.effective_to AS "EffectiveTo"
                FROM academic.department_subject_teacher dst
                JOIN academic.subject s ON s.subject_id=dst.subject_id AND s.tenant_id=dst.tenant_id
                JOIN hr.employee e ON e.employee_id=dst.teacher_id AND e.tenant_id=dst.tenant_id
                WHERE dst.tenant_id=@TenantId AND dst.department_id=@DepartmentId AND dst.is_active=TRUE
                ORDER BY s.name, "TeacherName";
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = (await connection.QueryAsync<Response>(new CommandDefinition(sql, request, cancellationToken: cancellationToken))).AsList();
            return Result<IReadOnlyCollection<Response>>.Success(rows);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/academics/department/{departmentId:guid}/subject-teachers",
                async (Guid departmentId, Guid tenantId, IMediator mediator, CancellationToken ct) =>
                    (await mediator.SendAsync<Request, Result<IReadOnlyCollection<Response>>>(new Request(tenantId, departmentId), ct)).ToHttpResult())
            .WithName("GetDepartmentSubjectTeachers").WithTags(ModuleConstants.Name).RequireAuthorization();
        return endpoints;
    }
}
