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

namespace SmartSchool.Modules.Learning.Features.AssignmentSubmission;

public static class CreateAssignmentSubmission
{
    public sealed record Request(Guid TenantId, Guid AssignmentId, Guid StudentId, string? Comment,
        string? FileName, string? ContentType, byte[]? Content) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, string Status, int AttemptNo, DateTimeOffset? SubmittedAt);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.AssignmentId).NotEmpty();
            RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.Comment).MaximumLength(20000);
            RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Comment) || x.Content is { Length: > 0 })
                .WithMessage("Attach a file or enter submission text.");
            RuleFor(x => x.Content).Must(x => x is null || x.Length <= 25 * 1024 * 1024)
                .WithMessage("Files must be 25 MB or smaller.");
        }
    }
    public interface ICreateAssignmentSubmissionQuery { Task<bool> IsEnrolledAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class CreateAssignmentSubmissionQuery(IDbConnectionFactory factory) : ICreateAssignmentSubmissionQuery
    {
        public async Task<bool> IsEnrolledAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (SELECT 1 FROM lms.academic_assignment a
                    JOIN student.student_enrollment e ON e.tenant_id = a.tenant_id
                        AND e.class_section_id = a.class_section_id AND e.is_active AND e.status = 'ACTIVE'
                    WHERE a.academic_assignment_id = @AssignmentId AND a.tenant_id = @TenantId
                        AND a.is_active AND e.student_id = @StudentId);
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, request, cancellationToken: cancellationToken));
        }
    }
    public interface ICreateAssignmentSubmissionCommand { Task<Result<Response>> SubmitAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class CreateAssignmentSubmissionCommand(ILearningDbContext db) : ICreateAssignmentSubmissionCommand
    {
        public async Task<Result<Response>> SubmitAsync(Request request, CancellationToken cancellationToken)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
            // Serialize attempts against the assignment to enforce the limit under concurrent uploads.
            var assignment = await db.Assignments.FromSqlInterpolated($"SELECT * FROM lms.academic_assignment WHERE tenant_id = {request.TenantId} AND academic_assignment_id = {request.AssignmentId} FOR UPDATE")
                .SingleOrDefaultAsync(cancellationToken);
            if (assignment is null || !assignment.IsActive || assignment.Status != "PUBLISHED")
                return Result<Response>.Failure(Error.Validation("This assignment is not accepting submissions."));
            var isLate = assignment.DueAt.HasValue && DateTimeOffset.UtcNow > assignment.DueAt.Value;
            if (isLate && !assignment.AllowLateSubmission)
                return Result<Response>.Failure(Error.Validation("The submission deadline has passed."));
            var attempts = await db.AssignmentSubmissions.Where(x => x.TenantId == request.TenantId &&
                x.AcademicAssignmentId == request.AssignmentId && x.StudentId == request.StudentId)
                .CountAsync(cancellationToken);
            if (attempts >= assignment.MaxAttempts)
                return Result<Response>.Failure(Error.Conflict("You have used all allowed submission attempts."));
            var entity = AssignmentSubmissionEntity.Submit(request.TenantId, request.AssignmentId,
                request.StudentId, attempts + 1, request.Comment, request.FileName, request.ContentType, request.Content, isLate);
            db.AssignmentSubmissions.Add(entity);
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result<Response>.Success(new(entity.SubmissionId, entity.Status, entity.AttemptNo, entity.SubmittedAt));
        }
    }
    public sealed class Handler(ICreateAssignmentSubmissionQuery query, ICreateAssignmentSubmissionCommand command,
        ICurrentUser user) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (user.StudentId != request.StudentId || !user.IsInRole(SmartSchoolRoles.Student))
                return Result<Response>.Failure(Error.Forbidden("Only the assigned student may submit this work."));
            if (!await query.IsEnrolledAsync(request, cancellationToken))
                return Result<Response>.Failure(Error.Forbidden("You are not enrolled in this assignment's class."));
            return await command.SubmitAsync(request, cancellationToken);
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/learning/assignment-submission", async (HttpRequest http, ITenantScope scope,
            ICurrentUser user, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!http.HasFormContentType) return Results.BadRequest(new { message = "Send a multipart form." });
            var form = await http.ReadFormAsync(cancellationToken);
            if (!Guid.TryParse(form["assignmentId"], out var assignmentId))
                return Results.BadRequest(new { message = "Assignment is required." });
            var tenant = scope.Resolve(Guid.TryParse(form["tenantId"], out var requestedTenant) ? requestedTenant : null);
            if (!tenant.HasValue || !user.StudentId.HasValue) return Results.Forbid();
            var file = form.Files.GetFile("file");
            if (file is { Length: > 26214400 or 0 })
                return Results.BadRequest(new { message = "Choose a nonempty file up to 25 MB." });
            byte[]? content = null;
            if (file is not null)
            {
                using var buffer = new MemoryStream();
                await file.CopyToAsync(buffer, cancellationToken);
                content = buffer.ToArray();
            }
            var request = new Request(tenant.Value, assignmentId, user.StudentId.Value,
                form["comment"], file is null ? null : Path.GetFileName(file.FileName),
                file?.ContentType, content);
            return (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult();
        }).WithName("CreateAssignmentSubmission").WithTags("Learning")
            .RequireAuthorization(SmartSchoolPolicies.StudentSelfService).DisableAntiforgery();
        return endpoints;
    }
}
