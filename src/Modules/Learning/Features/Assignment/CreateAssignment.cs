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

namespace SmartSchool.Modules.Learning.Features.Assignment;

public static class CreateAssignment
{
    public sealed record Request(Guid TenantId, string Name, Guid CourseOfferingId,
        Guid ClassSectionId, Guid TeacherEmployeeId, string AssignmentTypeCode,
        string? Description, DateTimeOffset? DueAt, decimal TotalMarks,
        bool AllowLateSubmission = false, int MaxAttempts = 1) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, string Code, string Name, string Status);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
            RuleFor(x => x.CourseOfferingId).NotEmpty();
            RuleFor(x => x.ClassSectionId).NotEmpty();
            RuleFor(x => x.TeacherEmployeeId).NotEmpty();
            RuleFor(x => x.AssignmentTypeCode).Must(x => new[] { "HOMEWORK", "PROJECT", "ESSAY",
                "LAB_REPORT", "PRESENTATION", "RESEARCH", "CLASSWORK" }.Contains(x));
            RuleFor(x => x.Description).MaximumLength(10000);
            RuleFor(x => x.TotalMarks).GreaterThan(0).LessThanOrEqualTo(10000);
            RuleFor(x => x.MaxAttempts).InclusiveBetween(1, 10);
        }
    }

    public sealed record Placement(Guid CampusId, string CampusCode);
    public interface ICreateAssignmentQuery
    {
        Task<Placement?> GetPlacementAsync(Request request, CancellationToken cancellationToken);
    }
    internal sealed class CreateAssignmentQuery(IDbConnectionFactory factory) : ICreateAssignmentQuery
    {
        public async Task<Placement?> GetPlacementAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT DISTINCT c.campus_id AS "CampusId", c.code AS "CampusCode"
                FROM academic.teacher_course_assignment ta
                JOIN academic.course_offering co ON co.course_offering_id = ta.course_offering_id
                    AND co.tenant_id = ta.tenant_id AND co.is_active
                JOIN academic.class_section cs ON cs.class_section_id = ta.class_section_id
                    AND cs.tenant_id = ta.tenant_id AND cs.is_active
                    AND cs.campus_id = co.campus_id AND cs.academic_year_id = co.academic_year_id
                JOIN org.campus c ON c.campus_id = cs.campus_id AND c.tenant_id = cs.tenant_id AND c.is_active
                WHERE ta.tenant_id = @TenantId AND ta.course_offering_id = @CourseOfferingId
                    AND ta.class_section_id = @ClassSectionId AND ta.employee_id = @TeacherEmployeeId
                    AND ta.is_active AND (ta.effective_from IS NULL OR ta.effective_from <= CURRENT_DATE)
                    AND (ta.effective_to IS NULL OR ta.effective_to >= CURRENT_DATE);
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Placement>(new CommandDefinition(sql, request,
                cancellationToken: cancellationToken));
        }
    }
    public interface ICreateAssignmentCommand
    {
        Task AddAsync(AssignmentEntity entity, CancellationToken cancellationToken);
    }
    internal sealed class CreateAssignmentCommand(ILearningDbContext db) : ICreateAssignmentCommand
    {
        public async Task AddAsync(AssignmentEntity entity, CancellationToken cancellationToken)
        {
            db.Assignments.Add(entity);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
    public sealed class Handler(ICreateAssignmentQuery query, ICreateAssignmentCommand command,
        IBusinessNumberGenerator numbers, ICurrentUser user) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (!LearningPermissions.CanManage(user, request.TeacherEmployeeId))
                return Result<Response>.Failure(Error.Forbidden("Assignments can only be created for your teaching allocation."));
            var placement = await query.GetPlacementAsync(request, cancellationToken);
            if (placement is null || (user.BranchId.HasValue && user.BranchId != placement.CampusId))
                return Result<Response>.Failure(Error.Validation("Select an active teacher, course and class allocation in your branch."));
            var code = await numbers.NextAsync($"ASSIGNMENT:{placement.CampusId}",
                $"{placement.CampusCode}-ASG-", request.TenantId, 7, cancellationToken);
            var entity = AssignmentEntity.Assign(request.TenantId, placement.CampusId, code, request.Name,
                request.CourseOfferingId, request.ClassSectionId, request.TeacherEmployeeId,
                request.AssignmentTypeCode, request.Description, request.DueAt, request.TotalMarks,
                request.AllowLateSubmission, request.MaxAttempts);
            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(new(entity.AcademicAssignmentId, entity.Code, entity.Name, entity.Status));
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/learning/assignment", async (Request request, ITenantScope scope,
            IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("CreateAssignment").WithTags("Learning").RequireAuthorization(SmartSchoolPolicies.AcademicManagement);
        return endpoints;
    }
}
