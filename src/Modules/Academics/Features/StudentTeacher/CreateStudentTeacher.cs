using SmartSchool.Modules.Academics.Persistence;
using Microsoft.EntityFrameworkCore;
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

    public sealed class Handler(IAcademicsDbContext dbContext) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var enrollment = await dbContext.Database.SqlQueryRaw<EnrollmentContext>(
                "SELECT class_section_id AS \"ClassSectionId\", academic_year_id AS \"AcademicYearId\" FROM student.student_enrollment WHERE tenant_id = {0} AND student_enrollment_id = {1} AND student_id = {2} AND status = 'ACTIVE'",
                request.TenantId, request.StudentEnrollmentId, request.StudentId)
                .SingleOrDefaultAsync(cancellationToken);

            if (enrollment is null)
                return Result<Response>.Failure(Error.Validation("Active student enrollment was not found for the student."));

            var teacherCanTeachSubject = await dbContext.Database.SqlQueryRaw<bool>(
                "SELECT EXISTS (SELECT 1 FROM academic.department_subject_teacher WHERE tenant_id = {0} AND teacher_id = {1} AND subject_id = {2} AND is_active = TRUE) AS \"Value\"",
                request.TenantId, request.TeacherId, request.SubjectId).SingleAsync(cancellationToken);
            if (!teacherCanTeachSubject)
                return Result<Response>.Failure(Error.Validation("Teacher is not assigned to the selected subject through a department."));

            var id = Guid.NewGuid();
            await dbContext.Database.ExecuteSqlRawAsync(
                "INSERT INTO academic.student_teacher (student_teacher_id, tenant_id, student_id, teacher_id, subject_id, student_enrollment_id, class_section_id, academic_year_id, effective_from, effective_to) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})",
                new { id, request.TenantId, request.StudentId, request.TeacherId, request.SubjectId, request.StudentEnrollmentId, enrollment.ClassSectionId, enrollment.AcademicYearId, request.EffectiveFrom, request.EffectiveTo },
                cancellationToken);

            return Result<Response>.Success(new Response(id, request.StudentId, request.TeacherId, request.SubjectId, request.StudentEnrollmentId, enrollment.ClassSectionId, enrollment.AcademicYearId));
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
