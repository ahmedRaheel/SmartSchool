using System.Threading.Tasks;
using Dapper;
using SmartSchool.Modules.Students.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Student;

public static class ApproveStudentAdmission
{
    public sealed record Request(Guid TenantId, Guid StudentId, string Email) : IRequest<Result<Response>>;
    public sealed record Response(Guid StudentId, Guid UserId, string StudentNumber, string Status);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public sealed class Handler(
        ApproveStudentAdmissionStudentQuery query,
        ApproveStudentAdmissionStudentCommand command,
        ApproveStudentAdmissionStudentOnboardingQuery onboardingQuery,
        ApproveStudentAdmissionStudentOnboardingCommand onboardingCommand,
        IIdentityAccountService accounts,
        IBusinessNumberGenerator numberGenerator,
        TimeProvider timeProvider)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var student = await command.GetByIdAsync(
                request.TenantId,
                request.StudentId,
                cancellationToken);

            if (student is null)
            {
                return Result<Response>.Failure(Error.NotFound("Student was not found."));
            }

            if (student.UserId.HasValue)
            {
                return Result<Response>.Failure(Error.Conflict("Student already has a login account."));
            }

            if (!await onboardingQuery.HasGuardianAsync(request.TenantId, request.StudentId, cancellationToken))
            {
                return Result<Response>.Failure(
                    Error.Validation("A parent or guardian is required before student admission can be approved."));
            }

            var missingDocuments = await onboardingQuery.GetMissingRequiredDocumentsAsync(
                request.TenantId,
                request.StudentId,
                cancellationToken);

            if (missingDocuments.Count > 0)
            {
                return Result<Response>.Failure(
                    Error.Validation($"Required student documents are missing: {string.Join(", ", missingDocuments)}."));
            }

            var placement = await onboardingQuery.GetPendingPlacementAsync(
                request.TenantId,
                request.StudentId,
                cancellationToken);

            if (placement is null)
            {
                return Result<Response>.Failure(
                    Error.Validation("Academic year and class section placement are required before approval."));
            }

            var branchCode = await onboardingQuery.GetCampusCodeAsync(
                request.TenantId,
                student.BranchId,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(branchCode))
            {
                return Result<Response>.Failure(Error.Validation("The student's branch is invalid."));
            }

            var studentNumber = await numberGenerator.NextAsync(
                $"STUDENT:{student.BranchId}",
                $"{branchCode}-",
                request.TenantId,
                7,
                cancellationToken);

            var account = await accounts.CreateAccountAsync(
                request.TenantId,
                student.StudentId,
                SmartSchoolRoles.Student,
                request.Email,
                student.FirstName,
                student.LastName ?? string.Empty,
                student.SchoolId,
                student.BranchId,
                new[] { SmartSchoolRoles.Student },
                cancellationToken);

            student.ApproveAdmission(account.UserId, studentNumber);
            await command.UpdateAsync(student, cancellationToken);

            var enrollmentNumber = await numberGenerator.NextAsync(
                $"ENROLLMENT:{student.BranchId}",
                string.Empty,
                request.TenantId,
                3,
                cancellationToken);

            var enrollment = EnrollmentEntity.Create(
                request.TenantId,
                student.StudentId,
                enrollmentNumber,
                placement.AcademicYearId,
                placement.ClassSectionId,
                DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime),
                LifecycleStatuses.Active);

            await onboardingCommand.AddEnrollmentAndApprovePlacementAsync(
                enrollment,
                request.TenantId,
                student.StudentId,
                placement.AcademicYearId,
                cancellationToken);

            return Result<Response>.Success(
                new Response(student.StudentId, account.UserId, student.StudentNumber!, student.Status));
        }
    }


    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/students/student/{studentId:guid}/approve", async (Guid studentId, Request request, ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
            var command = request with { TenantId = tenantId.Value, StudentId = studentId };
            return (await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken)).ToHttpResult();
        }).WithName("ApproveStudentAdmission").WithTags("Students").RequireAuthorization();
        return endpoints;
    }
}

/// <summary>
/// Feature-owned data access for ApproveStudentAdmission. Do not share across slices.
/// </summary>
public sealed class ApproveStudentAdmissionStudentOnboardingCommand(IStudentsDbContext dbContext)
{

    public async Task AddEnrollmentAndApprovePlacementAsync(EnrollmentEntity enrollment, Guid tenantId, Guid studentId, Guid academicYearId, CancellationToken cancellationToken)
    {
        var placement = await dbContext.AdmissionPlacements
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.StudentId == studentId && x.AcademicYearId == academicYearId && x.Status == LifecycleStatuses.Pending, cancellationToken);
        await dbContext.Enrollments.AddAsync(enrollment, cancellationToken);
        placement?.Approve();
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Feature-owned data access for ApproveStudentAdmission. Do not share across slices.
/// </summary>
public sealed class ApproveStudentAdmissionStudentOnboardingQuery(IDbConnectionFactory connectionFactory)
{

    public async Task<bool> HasGuardianAsync(Guid tenantId, Guid studentId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM student.student_guardian WHERE tenant_id=@TenantId AND student_id=@StudentId AND is_active=true);";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, StudentId = studentId }, cancellationToken: cancellationToken));
    }


    public async Task<IReadOnlyList<string>> GetMissingRequiredDocumentsAsync(Guid tenantId, Guid studentId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT r.display_name
            FROM document.required_document r
            WHERE r.is_active = true
              AND r.is_required = true
              AND r.actor_type = 'STUDENT'
              AND (r.tenant_id IS NULL OR r.tenant_id = @TenantId)
              AND NOT EXISTS (
                  SELECT 1
                  FROM document.document d
                  JOIN document.student_document sd ON sd.document_id = d.document_id
                  WHERE sd.tenant_id = @TenantId
                    AND sd.student_id = @StudentId
                    AND d.document_type = r.document_type
                    AND d.status = 'ACTIVE'
              );
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<string>(new CommandDefinition(sql, new { TenantId = tenantId, StudentId = studentId }, cancellationToken: cancellationToken));
        return rows.AsList();
    }


    public async Task<AdmissionPlacementReadModel?> GetPendingPlacementAsync(Guid tenantId, Guid studentId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT ap.academic_year_id AS AcademicYearId,
                   ap.class_section_id AS ClassSectionId,
                   cs.class_id AS ClassId
            FROM student.admission_placement ap
            JOIN academic.class_section cs ON cs.class_section_id = ap.class_section_id AND cs.tenant_id = ap.tenant_id
            WHERE ap.tenant_id = @TenantId
              AND ap.student_id = @StudentId
              AND ap.status = 'PENDING'
            ORDER BY ap.requested_at DESC
            LIMIT 1;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<AdmissionPlacementReadModel>(new CommandDefinition(sql, new { TenantId = tenantId, StudentId = studentId }, cancellationToken: cancellationToken));
    }


    public async Task<string?> GetCampusCodeAsync(Guid tenantId, Guid campusId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT code FROM org.campus WHERE tenant_id=@TenantId AND campus_id=@CampusId;";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<string?>(new CommandDefinition(sql, new { TenantId = tenantId, CampusId = campusId }, cancellationToken: cancellationToken));
    }
}

/// <summary>
/// Feature-owned data access for ApproveStudentAdmission. Do not share across slices.
/// </summary>
public sealed class ApproveStudentAdmissionStudentCommand(IStudentsDbContext dbContext)
{
    public Task<StudentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Students.SingleOrDefaultAsync(
            entity => entity.TenantId == tenantId && entity.StudentId == id, cancellationToken);
    }


    public async Task UpdateAsync(
        StudentEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Students
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Feature-owned data access for ApproveStudentAdmission. Do not share across slices.
/// </summary>
public sealed class ApproveStudentAdmissionStudentQuery(IDbConnectionFactory connectionFactory)
{
    
