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

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

public static class SaveExamResults
{
    public sealed record Entry(Guid StudentId, Guid ExamSubjectId, decimal? MarksObtained, bool IsAbsent, string? Remarks);
    public sealed record Request(Guid TenantId, Guid ExamId, IReadOnlyList<Entry> Rows) : IRequest<Result<Response>>;
    public sealed record Response(int SavedCount);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.ExamId).NotEmpty();
            RuleFor(x => x.Rows).NotEmpty().Must(x => x is not null && x.Count <= 3000 && x.Select(y => (y.StudentId, y.ExamSubjectId)).Distinct().Count() == x.Count);
            RuleForEach(x => x.Rows).ChildRules(v =>
            {
                v.RuleFor(x => x.StudentId).NotEmpty();
                v.RuleFor(x => x.ExamSubjectId).NotEmpty();
                v.RuleFor(x => x.MarksObtained).GreaterThanOrEqualTo(0);
                v.RuleFor(x => x).Must(x => x.IsAbsent || x.MarksObtained.HasValue).WithMessage("Enter marks or mark the student absent.");
                v.RuleFor(x => x.Remarks).MaximumLength(2000);
            });
        }
    }
    public sealed record Boundary(string Name, decimal MinimumPercentage, decimal MaximumPercentage);
    public interface ISaveExamResultsQuery
    {
        Task<HashSet<Guid>> GetStudentsAsync(Request request, CancellationToken cancellationToken);
        Task<IReadOnlyList<Boundary>> GetScaleAsync(Request request, CancellationToken cancellationToken);
    }
    internal sealed class SaveExamResultsQuery(IDbConnectionFactory factory) : ISaveExamResultsQuery
    {
        public async Task<HashSet<Guid>> GetStudentsAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT DISTINCT en.student_id FROM student.student_enrollment en
                JOIN exam.exam e ON e.class_section_id = en.class_section_id AND e.tenant_id = en.tenant_id
                WHERE en.tenant_id = @TenantId AND e.exam_id = @ExamId AND en.is_active AND en.status = 'ACTIVE';
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Guid>(new CommandDefinition(sql, request, cancellationToken: cancellationToken))).ToHashSet();
        }
        public async Task<IReadOnlyList<Boundary>> GetScaleAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT g.name AS "Name", g.minimum_percentage AS "MinimumPercentage", g.maximum_percentage AS "MaximumPercentage"
                FROM exam.grade_scale g JOIN exam.exam e ON e.tenant_id = g.tenant_id AND e.campus_id = g.campus_id
                WHERE e.tenant_id = @TenantId AND e.exam_id = @ExamId AND g.is_active ORDER BY g.minimum_percentage DESC;
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Boundary>(new CommandDefinition(sql, request, cancellationToken: cancellationToken))).AsList();
        }
    }
    public interface ISaveExamResultsCommand
    {
        Task<Result<Response>> SaveAsync(Request request, IReadOnlyList<Boundary> scale, CancellationToken cancellationToken);
    }
    internal sealed class SaveExamResultsCommand(IExaminationsDbContext db, ICurrentUser user) : ISaveExamResultsCommand
    {
        public async Task<Result<Response>> SaveAsync(Request request, IReadOnlyList<Boundary> scale, CancellationToken cancellationToken)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
            var exam = await db.Exams.FromSqlInterpolated($"SELECT * FROM exam.exam WHERE tenant_id = {request.TenantId} AND exam_id = {request.ExamId} FOR UPDATE")
                .SingleOrDefaultAsync(cancellationToken);
            if (exam is null || !exam.IsActive || (user.BranchId.HasValue && user.BranchId != exam.CampusId))
                return Result<Response>.Failure(Error.NotFound("Exam not found."));
            if (exam.Status == "PUBLISHED") return Result<Response>.Failure(Error.Conflict("Published results are locked."));
            var subjects = await db.ExamSubjects.Where(x => x.TenantId == request.TenantId && x.ExamId == request.ExamId && x.IsActive).ToDictionaryAsync(x => x.ExamSubjectId, cancellationToken);
            if (request.Rows.Any(x => !subjects.ContainsKey(x.ExamSubjectId) || x.MarksObtained > subjects[x.ExamSubjectId].TotalMarks))
                return Result<Response>.Failure(Error.Validation("Every result must belong to this exam and marks must be within the subject total."));
            var subjectIds = subjects.Keys.ToArray();
            var existing = await db.StudentExamResults.Where(x => x.TenantId == request.TenantId && subjectIds.Contains(x.ExamSubjectId) && x.IsActive).ToListAsync(cancellationToken);
            foreach (var row in request.Rows)
            {
                var total = subjects[row.ExamSubjectId].TotalMarks;
                var percent = row.IsAbsent || !row.MarksObtained.HasValue ? (decimal?)null : Math.Round(row.MarksObtained.Value / total * 100, 2);
                var grade = scale.FirstOrDefault(x => percent >= x.MinimumPercentage && percent <= x.MaximumPercentage)?.Name;
                var result = existing.SingleOrDefault(x => x.StudentId == row.StudentId && x.ExamSubjectId == row.ExamSubjectId);
                if (result is null)
                    db.StudentExamResults.Add(StudentExamResultEntity.Record(request.TenantId, row.ExamSubjectId, row.StudentId,
                        row.MarksObtained, total, row.IsAbsent, grade, row.Remarks, user.UserId));
                else result.SetMarks(row.MarksObtained, total, row.IsAbsent, grade, row.Remarks, user.UserId);
            }
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result<Response>.Success(new(request.Rows.Count));
        }
    }
    public sealed class Handler(ISaveExamResultsQuery query, ISaveExamResultsCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var students = await query.GetStudentsAsync(request, cancellationToken);
            if (request.Rows.Any(x => !students.Contains(x.StudentId)))
                return Result<Response>.Failure(Error.Validation("Results can only be entered for students enrolled in this exam's class."));
            return await command.SaveAsync(request, await query.GetScaleAsync(request, cancellationToken), cancellationToken);
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/examinations/exam/{examId:guid}/results", async (Guid examId, Request request,
            ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { ExamId = examId, TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("SaveExamResults").WithTags("Examinations").RequireAuthorization(SmartSchoolPolicies.ExaminationManagement);
        return endpoints;
    }
}
