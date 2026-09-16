using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Learning.Authorization;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.Assignment;

public static class UpdateAssignment
{
    public sealed record Request(Guid TenantId, Guid Id, string Name, string? Description,
        DateTimeOffset? DueAt, decimal TotalMarks, bool AllowLateSubmission, int MaxAttempts) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, string Name);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Description).MaximumLength(10000);
            RuleFor(x => x.TotalMarks).GreaterThan(0).LessThanOrEqualTo(10000);
            RuleFor(x => x.MaxAttempts).InclusiveBetween(1, 10);
        }
    }
    public interface IUpdateAssignmentCommand { Task<Result<Response>> UpdateAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class UpdateAssignmentCommand(ILearningDbContext db, ICurrentUser user) : IUpdateAssignmentCommand
    {
        public async Task<Result<Response>> UpdateAsync(Request request, CancellationToken cancellationToken)
        {
            var entity = await db.Assignments.SingleOrDefaultAsync(x => x.TenantId == request.TenantId && x.AcademicAssignmentId == request.Id && x.IsActive, cancellationToken);
            if (entity is null) return Result<Response>.Failure(Error.NotFound("Assignment not found."));
            if (!LearningPermissions.CanManage(user, entity.TeacherEmployeeId) || (user.BranchId.HasValue && user.BranchId != entity.BranchId))
                return Result<Response>.Failure(Error.Forbidden("You cannot edit this assignment."));
            if (await db.AssignmentSubmissions.AnyAsync(x => x.TenantId == request.TenantId && x.AcademicAssignmentId == request.Id && x.IsActive, cancellationToken)
                && (request.TotalMarks != entity.TotalMarks || request.MaxAttempts < entity.MaxAttempts))
                return Result<Response>.Failure(Error.Conflict("After work is submitted, total marks cannot change and attempt limits cannot decrease."));
            entity.Amend(request.Name, request.Description, request.DueAt, request.TotalMarks, request.AllowLateSubmission, request.MaxAttempts);
            await db.SaveChangesAsync(cancellationToken);
            return Result<Response>.Success(new(entity.AcademicAssignmentId, entity.Name));
        }
    }
    public sealed class Handler(IUpdateAssignmentCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken) => command.UpdateAsync(request, cancellationToken);
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/learning/assignment/{id:guid}", async (Guid id, Request request, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { Id = id, TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("UpdateAssignment").WithTags("Learning").RequireAuthorization(SmartSchoolPolicies.AcademicManagement);
        return endpoints;
    }
}
