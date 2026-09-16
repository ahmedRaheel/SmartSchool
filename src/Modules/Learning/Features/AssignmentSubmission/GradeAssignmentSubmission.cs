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

public static class GradeAssignmentSubmission
{
    public sealed record Request(Guid TenantId, Guid Id, decimal Marks, string? Feedback) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, decimal? Marks, string? Feedback, string Status);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Marks).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Feedback).MaximumLength(10000);
        }
    }
    public interface IGradeAssignmentSubmissionCommand { Task<Result<Response>> GradeAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class GradeAssignmentSubmissionCommand(ILearningDbContext db, ICurrentUser user) : IGradeAssignmentSubmissionCommand
    {
        public async Task<Result<Response>> GradeAsync(Request request, CancellationToken cancellationToken)
        {
            var submission = await db.AssignmentSubmissions.SingleOrDefaultAsync(x => x.TenantId == request.TenantId &&
                x.SubmissionId == request.Id && x.IsActive, cancellationToken);
            if (submission is null) return Result<Response>.Failure(Error.NotFound("Submission not found."));
            var assignment = await db.Assignments.SingleOrDefaultAsync(x => x.TenantId == request.TenantId &&
                x.AcademicAssignmentId == submission.AcademicAssignmentId && x.IsActive, cancellationToken);
            if (assignment is null || !LearningPermissions.CanManage(user, assignment.TeacherEmployeeId) ||
                (user.BranchId.HasValue && user.BranchId != assignment.BranchId))
                return Result<Response>.Failure(Error.Forbidden("You cannot grade this assignment."));
            if (request.Marks > (assignment.TotalMarks ?? 0))
                return Result<Response>.Failure(Error.Validation("Marks cannot exceed the assignment's total marks."));
            submission.Grade(request.Marks, request.Feedback);
            await db.SaveChangesAsync(cancellationToken);
            return Result<Response>.Success(new(submission.SubmissionId, submission.MarksObtained,
                submission.TeacherFeedback, submission.Status));
        }
    }
    public sealed class Handler(IGradeAssignmentSubmissionCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken) => command.GradeAsync(request, cancellationToken);
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/learning/assignment-submission/{id:guid}/grade", async (Guid id, Request request,
            ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { Id = id, TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("GradeAssignmentSubmission").WithTags("Learning").RequireAuthorization(SmartSchoolPolicies.AcademicManagement);
        return endpoints;
    }
}
