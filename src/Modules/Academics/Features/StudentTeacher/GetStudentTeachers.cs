using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.StudentTeacher;

public static class GetStudentTeachers
{
    public sealed record Request(Guid TenantId, Guid StudentId, Guid? StudentEnrollmentId) : IRequest<Result<IReadOnlyCollection<Response>>>;
    public sealed record Response(Guid Id, Guid TeacherId, string TeacherName, Guid SubjectId, string SubjectName, Guid StudentEnrollmentId, Guid ClassSectionId, Guid AcademicYearId);
    public sealed class Handler(IDbConnectionFactory connectionFactory) : IRequestHandler<Request, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT st.student_teacher_id AS "Id", st.teacher_id AS "TeacherId",
                       trim(concat(e.first_name, ' ', coalesce(e.last_name, ''))) AS "TeacherName",
                       st.subject_id AS "SubjectId", s.name AS "SubjectName", st.student_enrollment_id AS "StudentEnrollmentId",
                       st.class_section_id AS "ClassSectionId", st.academic_year_id AS "AcademicYearId"
                FROM academic.student_teacher st
                JOIN hr.employee e ON e.employee_id=st.teacher_id AND e.tenant_id=st.tenant_id
                JOIN academic.subject s ON s.subject_id=st.subject_id AND s.tenant_id=st.tenant_id
                WHERE st.tenant_id=@TenantId AND st.student_id=@StudentId AND st.is_active=TRUE
                  AND (@StudentEnrollmentId IS NULL OR st.student_enrollment_id=@StudentEnrollmentId)
                ORDER BY s.name, "TeacherName";
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows=(await connection.QueryAsync<Response>(new CommandDefinition(sql, request, cancellationToken:cancellationToken))).AsList();
            return Result<IReadOnlyCollection<Response>>.Success(rows);
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/academics/student/{studentId:guid}/teachers",
                async (Guid studentId, Guid tenantId, Guid? studentEnrollmentId, IMediator mediator, CancellationToken ct) =>
                    (await mediator.SendAsync<Request, Result<IReadOnlyCollection<Response>>>(new Request(tenantId, studentId, studentEnrollmentId), ct)).ToHttpResult())
            .WithName("GetStudentTeachers").WithTags(ModuleConstants.Name).RequireAuthorization();
        return endpoints;
    }
}
