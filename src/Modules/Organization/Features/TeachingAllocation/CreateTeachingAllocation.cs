using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.TeachingAllocation;

public static class CreateTeachingAllocation
{
    public sealed record Request(Guid TenantId, Guid CampusId, Guid ClassSectionId, Guid SubjectId,
        Guid EmployeeId, int PeriodsPerWeek, bool IsClassTeacher = false, DateOnly? EffectiveFrom = null, DateOnly? EffectiveTo = null, string? Name = null) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, Guid CourseOfferingId, string Code, string Name) { public Guid TeacherTeachingAssignmentId => Id; }
    public sealed record Placement(Guid AcademicYearId, string SubjectName);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty(); RuleFor(x => x.CampusId).NotEmpty();
            RuleFor(x => x.ClassSectionId).NotEmpty(); RuleFor(x => x.SubjectId).NotEmpty(); RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.EffectiveTo).GreaterThanOrEqualTo(x => x.EffectiveFrom).When(x => x.EffectiveTo.HasValue && x.EffectiveFrom.HasValue);
            RuleFor(x => x.Name).MaximumLength(250);
            RuleFor(x => x.PeriodsPerWeek).InclusiveBetween(1, 50);
        }
    }
    public interface ICreateTeachingAllocationQuery { Task<Placement?> GetPlacementAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class CreateTeachingAllocationQuery(IDbConnectionFactory factory) : ICreateTeachingAllocationQuery
    {
        public async Task<Placement?> GetPlacementAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT cs.academic_year_id AS "AcademicYearId", s.name AS "SubjectName"
                FROM academic.class_section cs JOIN hr.employee e ON e.tenant_id = cs.tenant_id AND e.branch_id = cs.campus_id
                JOIN org.department d ON d.tenant_id = cs.tenant_id AND d.campus_id = cs.campus_id AND d.is_active
                JOIN academic.subject s ON s.tenant_id = d.tenant_id AND s.department_id = d.department_id AND s.is_active
                WHERE cs.tenant_id = @TenantId AND cs.campus_id = @CampusId AND cs.class_section_id = @ClassSectionId AND cs.is_active
                    AND e.employee_id = @EmployeeId AND e.is_active AND upper(e.staff_type) = 'TEACHER' AND e.status IN ('ACTIVE','APPROVED')
                    AND s.subject_id = @SubjectId;
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Placement>(new CommandDefinition(sql, request, cancellationToken: cancellationToken));
        }
    }
    public interface ICreateTeachingAllocationCommand { Task<Result<Response>> SaveAsync(Request request, Placement placement, CancellationToken cancellationToken); }
    internal sealed class CreateTeachingAllocationCommand(IOrganizationDbContext db, IBusinessNumberGenerator numbers) : ICreateTeachingAllocationCommand
    {
        public async Task<Result<Response>> SaveAsync(Request request, Placement placement, CancellationToken cancellationToken)
        {
            var code = await numbers.NextAsync("COURSE", "CRS-", request.TenantId, 6, cancellationToken);
            var number = await numbers.NextAsync("TEACHER_ALLOCATION", "TAS-", request.TenantId, 6, cancellationToken);
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
            // Lock the class while creating its course allocation to prevent duplicate primary teachers.
            var section = await db.ClassSections.FromSqlInterpolated($"SELECT * FROM academic.class_section WHERE tenant_id = {request.TenantId} AND class_section_id = {request.ClassSectionId} FOR UPDATE").SingleAsync(cancellationToken);
            var course = await db.CourseOfferings.FirstOrDefaultAsync(x => x.TenantId == request.TenantId && x.CampusId == request.CampusId &&
                x.AcademicYearId == placement.AcademicYearId && x.SubjectId == request.SubjectId && x.IsActive, cancellationToken);
            if (course is null)
            {
                
                course = CourseOfferingEntity.Offer(request.TenantId, request.CampusId, placement.AcademicYearId, request.SubjectId, code, placement.SubjectName);
                db.CourseOfferings.Add(course);
            }
            else if (await db.TeacherAssignments.AnyAsync(x => x.TenantId == request.TenantId && x.ClassSectionId == request.ClassSectionId && x.CourseOfferingId == course.CourseOfferingId && x.IsActive, cancellationToken))
                return Result<Response>.Failure(Error.Conflict("This class already has a teacher allocated to that course."));
            
            var allocation = TeacherAssignmentEntity.Allocate(request.TenantId, course.CourseOfferingId, request.ClassSectionId,
                request.EmployeeId, number, string.IsNullOrWhiteSpace(request.Name) ? placement.SubjectName : request.Name, request.PeriodsPerWeek, request.EffectiveFrom, request.EffectiveTo);
            if (request.IsClassTeacher) section.AssignClassTeacher(request.EmployeeId);
            db.TeacherAssignments.Add(allocation); await db.SaveChangesAsync(cancellationToken); await transaction.CommitAsync(cancellationToken);
            return Result<Response>.Success(new(allocation.TeacherCourseAssignmentId, course.CourseOfferingId, allocation.Code, allocation.Name));
        }
    }
    public sealed class Handler(ICreateTeachingAllocationQuery query, ICreateTeachingAllocationCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var placement = await query.GetPlacementAsync(request, cancellationToken);
            return placement is null ? Result<Response>.Failure(Error.Validation("Choose an active class, subject and approved teacher in the same campus."))
                : await command.SaveAsync(request, placement, cancellationToken);
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        static async Task<IResult> Handle(Request request, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken)
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }
        endpoints.MapPost("/api/academics/teaching-allocations", Handle).WithName("CreateTeachingAllocation").WithTags("Organization").RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
        endpoints.MapPost("/api/hr/teaching-assignment", Handle).WithName("CreateTeachingAssignment").WithTags("HR").RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
        return endpoints;
    }
}
