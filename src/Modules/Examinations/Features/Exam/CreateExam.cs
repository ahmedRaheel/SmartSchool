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

public static class CreateExam
{
    public sealed record Subject(Guid CourseOfferingId, decimal TotalMarks, decimal PassingMarks, DateOnly ExamDate);
    public sealed record Request(Guid TenantId, string Name, Guid ClassSectionId, string ExamTypeCode,
        DateOnly StartDate, DateOnly EndDate, IReadOnlyList<Subject> Subjects) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, string Name, string Status);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
            RuleFor(x => x.ClassSectionId).NotEmpty();
            RuleFor(x => x.ExamTypeCode).NotEmpty().MaximumLength(40);
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate);
            RuleFor(x => x.Subjects).NotEmpty().Must(x => x is not null && x.Count <= 30 && x.Select(y => y.CourseOfferingId).Distinct().Count() == x.Count)
                .WithMessage("Select up to 30 distinct courses.");
            RuleForEach(x => x.Subjects).ChildRules(v =>
            {
                v.RuleFor(x => x.CourseOfferingId).NotEmpty();
                v.RuleFor(x => x.TotalMarks).GreaterThan(0).LessThanOrEqualTo(10000);
                v.RuleFor(x => x.PassingMarks).GreaterThanOrEqualTo(0).LessThanOrEqualTo(x => x.TotalMarks);
            });
            RuleFor(x => x).Must(x => x.Subjects is not null && x.Subjects.All(y => y.ExamDate >= x.StartDate && y.ExamDate <= x.EndDate))
                .WithMessage("Subject dates must be within the exam dates.");
        }
    }
    public sealed record Placement(Guid CampusId, Guid AcademicYearId, Guid AcademicSystemId);
    public sealed record Course(Guid Id, string Name);
    public interface ICreateExamQuery
    {
        Task<Placement?> GetPlacementAsync(Request request, CancellationToken cancellationToken);
        Task<IReadOnlyList<Course>> GetCoursesAsync(Request request, CancellationToken cancellationToken);
    }
    internal sealed class CreateExamQuery(IDbConnectionFactory factory, ICurrentUser user) : ICreateExamQuery
    {
        public async Task<Placement?> GetPlacementAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT cs.campus_id AS "CampusId", cs.academic_year_id AS "AcademicYearId", c.academic_system_id AS "AcademicSystemId"
                FROM academic.class_section cs JOIN org.campus c ON c.campus_id = cs.campus_id AND c.tenant_id = cs.tenant_id
                WHERE cs.tenant_id = @TenantId AND cs.class_section_id = @ClassSectionId AND cs.is_active AND c.is_active
                    AND c.academic_system_id IS NOT NULL AND (@CampusId IS NULL OR cs.campus_id = @CampusId);
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Placement>(new CommandDefinition(sql,
                new { request.TenantId, request.ClassSectionId, CampusId = user.BranchId }, cancellationToken: cancellationToken));
        }
        public async Task<IReadOnlyList<Course>> GetCoursesAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT DISTINCT co.course_offering_id AS "Id", coalesce(co.display_name, co.name) AS "Name"
                FROM academic.teacher_course_assignment ta
                JOIN academic.course_offering co ON co.course_offering_id = ta.course_offering_id AND co.tenant_id = ta.tenant_id AND co.is_active
                WHERE ta.tenant_id = @TenantId AND ta.class_section_id = @ClassSectionId AND ta.is_active
                    AND co.course_offering_id = ANY(@Ids);
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Course>(new CommandDefinition(sql,
                new { request.TenantId, request.ClassSectionId, Ids = request.Subjects.Select(x => x.CourseOfferingId).ToArray() }, cancellationToken: cancellationToken))).AsList();
        }
    }
    public interface ICreateExamCommand { Task SaveAsync(ExamEntity exam, IReadOnlyList<ExamSubjectEntity> subjects, CancellationToken cancellationToken); }
    internal sealed class CreateExamCommand(IExaminationsDbContext db) : ICreateExamCommand
    {
        public async Task SaveAsync(ExamEntity exam, IReadOnlyList<ExamSubjectEntity> subjects, CancellationToken cancellationToken)
        {
            db.Exams.Add(exam);
            db.ExamSubjects.AddRange(subjects);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
    public sealed class Handler(ICreateExamQuery query, ICreateExamCommand command, IBusinessNumberGenerator numbers) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var placement = await query.GetPlacementAsync(request, cancellationToken);
            var courses = await query.GetCoursesAsync(request, cancellationToken);
            if (placement is null || courses.Count != request.Subjects.Count)
                return Result<Response>.Failure(Error.Validation("Select a class with a configured academic system and courses allocated to it."));
            var code = await numbers.NextAsync("EXAM", "EXM-", request.TenantId, 7, cancellationToken);
            var exam = ExamEntity.Schedule(request.TenantId, code, request.Name, placement.CampusId,
                placement.AcademicYearId, placement.AcademicSystemId, request.ClassSectionId,
                request.ExamTypeCode, request.StartDate, request.EndDate);
            var subjects = request.Subjects.Select(x => ExamSubjectEntity.Schedule(request.TenantId, exam.ExamId,
                x.CourseOfferingId, courses.Single(c => c.Id == x.CourseOfferingId).Name,
                x.TotalMarks, x.PassingMarks, x.ExamDate)).ToList();
            await command.SaveAsync(exam, subjects, cancellationToken);
            return Result<Response>.Success(new(exam.ExamId, exam.Name, exam.Status));
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/examinations/exam", async (Request request, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("CreateExam").WithTags("Examinations").RequireAuthorization(SmartSchoolPolicies.ExaminationManagement);
        return endpoints;
    }
}
