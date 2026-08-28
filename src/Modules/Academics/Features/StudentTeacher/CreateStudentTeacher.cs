using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.StudentTeacher;

public static class CreateStudentTeacher
{
    public sealed record Request(Guid TenantId, Guid StudentId, Guid TeacherId, Guid SubjectId, Guid StudentEnrollmentId, DateOnly? EffectiveFrom, DateOnly? EffectiveTo) : IRequest<Result<Response>>;
    public sealed record Response(Guid StudentTeacherId, Guid StudentId, Guid TeacherId, Guid SubjectId, Guid StudentEnrollmentId, Guid ClassSectionId, Guid AcademicYearId);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty(); RuleFor(x => x.StudentId).NotEmpty(); RuleFor(x => x.TeacherId).NotEmpty();
            RuleFor(x => x.SubjectId).NotEmpty(); RuleFor(x => x.StudentEnrollmentId).NotEmpty();
            RuleFor(x => x).Must(x => !x.EffectiveFrom.HasValue || !x.EffectiveTo.HasValue || x.EffectiveTo >= x.EffectiveFrom).WithMessage("EffectiveTo must be on or after EffectiveFrom.");
        }
    }

    public sealed class Handler(IDbConnectionFactory connectionFactory) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            const string enrollmentSql = """
                SELECT se.class_section_id AS "ClassSectionId", se.academic_year_id AS "AcademicYearId"
                FROM student.student_enrollment se
                WHERE se.tenant_id=@TenantId AND se.student_enrollment_id=@StudentEnrollmentId
                  AND se.student_id=@StudentId AND se.status='ACTIVE';
                """;
            const string teacherSubjectSql = """
                SELECT EXISTS(
                    SELECT 1 FROM academic.department_subject_teacher dst
                    WHERE dst.tenant_id=@TenantId AND dst.teacher_id=@TeacherId AND dst.subject_id=@SubjectId AND dst.is_active=TRUE
                );
                """;
            const string insertSql = """
                INSERT INTO academic.student_teacher
                    (tenant_id, student_id, teacher_id, subject_id, student_enrollment_id, class_section_id, academic_year_id, effective_from, effective_to)
                VALUES
                    (@TenantId, @StudentId, @TeacherId, @SubjectId, @StudentEnrollmentId, @ClassSectionId, @AcademicYearId, @EffectiveFrom, @EffectiveTo)
                RETURNING student_teacher_id;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var enrollment = await connection.QuerySingleOrDefaultAsync<EnrollmentContext>(new CommandDefinition(enrollmentSql, request, cancellationToken: cancellationToken));
            if (enrollment is null) return Result<Response>.Failure(Error.Validation("Active student enrollment was not found for the student."));
            var teacherCanTeachSubject = await connection.ExecuteScalarAsync<bool>(new CommandDefinition(teacherSubjectSql, request, cancellationToken: cancellationToken));
            if (!teacherCanTeachSubject) return Result<Response>.Failure(Error.Validation("Teacher is not assigned to the selected subject through a department."));
            var args = new { request.TenantId, request.StudentId, request.TeacherId, request.SubjectId, request.StudentEnrollmentId, enrollment.ClassSectionId, enrollment.AcademicYearId, request.EffectiveFrom, request.EffectiveTo };
            try
            {
                var id = await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(insertSql, args, cancellationToken: cancellationToken));
                return Result<Response>.Success(new Response(id, request.StudentId, request.TeacherId, request.SubjectId, request.StudentEnrollmentId, enrollment.ClassSectionId, enrollment.AcademicYearId));
            }
            catch (Exception ex) when (ex.Message.Contains("unique", StringComparison.OrdinalIgnoreCase))
            {
                return Result<Response>.Failure(Error.Conflict("The teacher is already assigned to this student and subject for the enrollment."));
            }
        }
        private sealed record EnrollmentContext(Guid ClassSectionId, Guid AcademicYearId);
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student-teacher"),
                async (Request request, IMediator mediator, CancellationToken ct) => (await mediator.SendAsync<Request, Result<Response>>(request, ct)).ToHttpResult())
            .WithName("CreateStudentTeacher").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        return endpoints;
    }
}
