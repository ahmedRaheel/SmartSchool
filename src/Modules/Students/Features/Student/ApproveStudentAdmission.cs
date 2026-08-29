using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Persistence;
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

    public sealed class Handler(IStudentQuery query, IStudentCommand command, IIdentityAccountService accounts, IBusinessNumberGenerator numberGenerator, IDbConnectionFactory connectionFactory)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var student = await query.GetByIdAsync(request.TenantId, request.StudentId, cancellationToken);
            if (student is null) return Result<Response>.Failure(Error.NotFound("Student was not found."));
            if (student.UserId.HasValue) return Result<Response>.Failure(Error.Conflict("Student already has a login account."));

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);

            var guardianExists = await connection.ExecuteScalarAsync<bool>(new CommandDefinition(
                "SELECT EXISTS(SELECT 1 FROM student.student_guardian WHERE student_id=@StudentId)",
                new { request.StudentId }, cancellationToken:cancellationToken));
            if (!guardianExists)
                return Result<Response>.Failure(Error.Validation("A parent or guardian is required before student admission can be approved."));

            var missingDocuments = (await connection.QueryAsync<string>(new CommandDefinition(
                """
                SELECT r.display_name
                FROM document.required_document r
                WHERE r.is_active=true AND r.is_required=true AND r.actor_type='STUDENT'
                  AND (r.tenant_id IS NULL OR r.tenant_id=@TenantId)
                  AND NOT EXISTS (
                    SELECT 1 FROM document.document d
                    JOIN document.document_link l ON l.document_id=d.document_id AND l.tenant_id=d.tenant_id
                    WHERE d.tenant_id=@TenantId AND l.entity_type='STUDENT' AND l.entity_id=@StudentId
                      AND d.document_type=r.document_type AND d.status='ACTIVE')
                """, new { request.TenantId, request.StudentId }, cancellationToken:cancellationToken))).ToArray();
            if (missingDocuments.Length > 0)
                return Result<Response>.Failure(Error.Validation($"Required student documents are missing: {string.Join(", ", missingDocuments)}."));

            var placement = await connection.QuerySingleOrDefaultAsync<PlacementRow>(new CommandDefinition(
                """SELECT academic_year_id AS AcademicYearId,class_section_id AS ClassSectionId FROM student.admission_placement
                    WHERE tenant_id=@TenantId AND student_id=@StudentId AND status='PENDING' ORDER BY requested_at DESC LIMIT 1""",
                new { request.TenantId, request.StudentId }, cancellationToken:cancellationToken));
            if (placement is null)
                return Result<Response>.Failure(Error.Validation("Academic year and class section placement are required before approval."));

            var branchCode = await connection.ExecuteScalarAsync<string>(new CommandDefinition(
                "SELECT code FROM org.campus WHERE tenant_id=@TenantId AND campus_id=@BranchId",
                new { request.TenantId, student.BranchId }, cancellationToken: cancellationToken));
            if (string.IsNullOrWhiteSpace(branchCode)) return Result<Response>.Failure(Error.Validation("The student's branch is invalid."));
            var studentNumber = await numberGenerator.NextAsync(
                $"STUDENT:{student.BranchId}", $"{branchCode}-", request.TenantId, 7, cancellationToken);

            var account = await accounts.CreateAccountAsync(
                request.TenantId, student.StudentId, "Student", request.Email, student.FirstName, student.LastName ?? string.Empty,
                student.SchoolId, student.BranchId, new[] { "Student" }, cancellationToken);

            student.ApproveAdmission(account.UserId, studentNumber);
            await command.UpdateAsync(student, cancellationToken);

            var enrollmentNumber = await numberGenerator.NextAsync(
                $"ENROLLMENT:{student.BranchId}", string.Empty, request.TenantId, 3, cancellationToken);
            await connection.ExecuteAsync(new CommandDefinition(
                """
                INSERT INTO student.student_enrollment(student_enrollment_id,tenant_id,student_id,enrollment_number,academic_year_id,class_section_id,class_id,enrollment_date,status,is_active,created_at,row_version)
                SELECT gen_random_uuid(),@TenantId,@StudentId,@EnrollmentNumber,@AcademicYearId,@ClassSectionId,cs.class_id,CURRENT_DATE,'ACTIVE',true,now(),0
                FROM academic.class_section cs WHERE cs.class_section_id=@ClassSectionId AND cs.tenant_id=@TenantId;
                UPDATE student.admission_placement SET status='APPROVED',approved_at=now()
                WHERE tenant_id=@TenantId AND student_id=@StudentId AND academic_year_id=@AcademicYearId AND status='PENDING';
                """, new { request.TenantId, request.StudentId, EnrollmentNumber=enrollmentNumber, placement.AcademicYearId, placement.ClassSectionId }, cancellationToken:cancellationToken));

            return Result<Response>.Success(new Response(student.StudentId, account.UserId, student.StudentNumber!, student.Status));
        }
    }

    private sealed record PlacementRow(Guid AcademicYearId, Guid ClassSectionId);

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
