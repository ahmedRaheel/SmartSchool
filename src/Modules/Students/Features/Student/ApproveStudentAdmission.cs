using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
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

    public sealed class Handler(
        IStudentQuery query,
        IStudentCommand command,
        IStudentOnboardingQuery onboardingQuery,
        IStudentOnboardingCommand onboardingCommand,
        IIdentityAccountService accounts,
        IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var student = await query.GetByIdAsync(
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
                "Student",
                request.Email,
                student.FirstName,
                student.LastName ?? string.Empty,
                student.SchoolId,
                student.BranchId,
                new[] { "Student" },
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
                DateOnly.FromDateTime(DateTime.UtcNow),
                "ACTIVE");

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
