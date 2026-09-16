using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
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

public static class PublishExamResults
{
    public sealed record Request(Guid TenantId, Guid ExamId) : IRequest<Result<Response>>;
    public sealed record Response(Guid ExamId, string Status);
    public interface IPublishExamResultsQuery { Task<long> GetMissingCountAsync(Request request, DbConnection connection, DbTransaction transaction, CancellationToken cancellationToken); }
    internal sealed class PublishExamResultsQuery : IPublishExamResultsQuery
    {
        public async Task<long> GetMissingCountAsync(Request request, DbConnection connection, DbTransaction transaction, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT count(*) FROM exam.exam e
                JOIN exam.exam_subject es ON es.exam_id = e.exam_id AND es.tenant_id = e.tenant_id AND es.is_active
                JOIN student.student_enrollment en ON en.class_section_id = e.class_section_id AND en.tenant_id = e.tenant_id AND en.is_active AND en.status = 'ACTIVE'
                WHERE e.tenant_id = @TenantId AND e.exam_id = @ExamId AND NOT EXISTS (
                    SELECT 1 FROM exam.student_exam_result r WHERE r.tenant_id = e.tenant_id
                        AND r.exam_subject_id = es.exam_subject_id AND r.student_id = en.student_id AND r.is_active
                        AND (r.is_absent OR r.marks_obtained IS NOT NULL));
                """;
            return await connection.ExecuteScalarAsync<long>(new CommandDefinition(sql, request, transaction: transaction, cancellationToken: cancellationToken));
        }
    }
    public interface IPublishExamResultsCommand { Task<Result<Response>> PublishAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class PublishExamResultsCommand(IExaminationsDbContext db, IPublishExamResultsQuery query, ICurrentUser user) : IPublishExamResultsCommand
    {
        public async Task<Result<Response>> PublishAsync(Request request, CancellationToken cancellationToken)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
            var exam = await db.Exams.FromSqlInterpolated($"SELECT * FROM exam.exam WHERE tenant_id = {request.TenantId} AND exam_id = {request.ExamId} FOR UPDATE")
                .SingleOrDefaultAsync(cancellationToken);
            if (exam is null || !exam.IsActive || (user.BranchId.HasValue && user.BranchId != exam.CampusId))
                return Result<Response>.Failure(Error.NotFound("Exam not found."));
            if (exam.Status == "PUBLISHED") return Result<Response>.Success(new(exam.ExamId, exam.Status));
            if (!await db.StudentExamResults.AnyAsync(x => x.TenantId == request.TenantId && x.IsActive &&
                db.ExamSubjects.Any(es => es.ExamId == request.ExamId && es.ExamSubjectId == x.ExamSubjectId && es.IsActive), cancellationToken))
                return Result<Response>.Failure(Error.Validation("Enter exam results before publishing."));
            var missing = await query.GetMissingCountAsync(request, db.Database.GetDbConnection(), transaction.GetDbTransaction(), cancellationToken);
            if (missing > 0) return Result<Response>.Failure(Error.Validation($"Enter marks or mark absence for all students and subjects. {missing} results are missing."));
            exam.Publish();
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result<Response>.Success(new(exam.ExamId, exam.Status));
        }
    }
    public sealed class Handler(IPublishExamResultsCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken) => command.PublishAsync(request, cancellationToken);
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/examinations/exam/{examId:guid}/publish", async (Guid examId, Request request,
            ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { ExamId = examId, TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("PublishExamResults").WithTags("Examinations").RequireAuthorization(SmartSchoolPolicies.ExaminationManagement);
        return endpoints;
    }
}
