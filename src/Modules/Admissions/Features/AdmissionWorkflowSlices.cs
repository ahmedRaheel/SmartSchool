using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public enum AdmissionApplicationStatus
{
    SubmittedApplication,
    AdmissionAccepted,
    AdmissionRejected,
    WaitingList
}

public static class AdmissionApplicationStatusExtensions
{
    public static bool TryParseDatabaseValue(
        string? value,
        out AdmissionApplicationStatus status)
    {
        status = value?.Trim().ToUpperInvariant() switch
        {
            "SUBMITTED_APPLICATION" => AdmissionApplicationStatus.SubmittedApplication,
            "ADMISSION_ACCEPTED" => AdmissionApplicationStatus.AdmissionAccepted,
            "ADMISSION_REJECTED" => AdmissionApplicationStatus.AdmissionRejected,
            LifecycleStatuses.WaitingList => AdmissionApplicationStatus.WaitingList,
            _ => default
        };

        return value?.Trim().ToUpperInvariant() is
            "SUBMITTED_APPLICATION" or
            "ADMISSION_ACCEPTED" or
            "ADMISSION_REJECTED" or
            LifecycleStatuses.WaitingList;
    }

    public static string ToDatabaseValue(this AdmissionApplicationStatus status) => status switch
    {
        AdmissionApplicationStatus.SubmittedApplication => "SUBMITTED_APPLICATION",
        AdmissionApplicationStatus.AdmissionAccepted => "ADMISSION_ACCEPTED",
        AdmissionApplicationStatus.AdmissionRejected => "ADMISSION_REJECTED",
        AdmissionApplicationStatus.WaitingList => LifecycleStatuses.WaitingList,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };
}

public sealed record AdmissionApplicationDto(
    Guid Id,
    Guid SchoolId,
    Guid BranchId,
    Guid? AcademicYearId,
    Guid? ClassId,
    Guid? SectionId,
    string FirstName,
    string? LastName,
    DateOnly? DateOfBirth,
    string? Gender,
    string? Email,
    string? Phone,
    string GuardianName,
    string? GuardianEmail,
    string? GuardianPhone,
    decimal? PreviousMarks,
    string Status,
    DateTimeOffset SubmittedAt,
    string? DecisionNotes,
    Guid? StudentId);

public sealed record AdmissionCriteriaDto(
    Guid Id,
    Guid SchoolId,
    Guid BranchId,
    Guid AcademicYearId,
    Guid ClassId,
    decimal MinimumMarks,
    decimal? EntranceTestMinimum,
    int? MinimumAge,
    int? MaximumAge,
    bool InterviewRequired,
    string? RequiredDocuments,
    string Status);

public sealed record AdmissionApplicationDetails(
    Guid Id,
    Guid SchoolId,
    Guid BranchId,
    Guid? AcademicYearId,
    Guid? ClassId,
    Guid? SectionId,
    string FirstName,
    string? LastName,
    DateOnly? DateOfBirth,
    string? Gender,
    string? Email,
    string GuardianName,
    string? GuardianCnic,
    string? GuardianEmail,
    string? GuardianPhone,
    string? Relationship,
    Guid? StudentId);

public static class GetAdmissionApplications
{
    public sealed record Request(Guid? TenantId)
        : IRequest<Result<IReadOnlyList<AdmissionApplicationDto>>>;

    public sealed class Handler(
        ITenantScope tenantScope,
        IAdmissionWorkflowQuery query)
        : IRequestHandler<Request, Result<IReadOnlyList<AdmissionApplicationDto>>>
    {
        public async Task<Result<IReadOnlyList<AdmissionApplicationDto>>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<IReadOnlyList<AdmissionApplicationDto>>.Failure(
                    Error.Validation("Tenant context is required."));
            }

            var applications = await query.GetApplicationsAsync(
                tenantId.Value,
                cancellationToken);

            return Result<IReadOnlyList<AdmissionApplicationDto>>.Success(applications);
        }
    }
}

public static class CreateAdmissionApplication
{
    public sealed record Request(
        Guid? TenantId,
        Guid SchoolId,
        Guid BranchId,
        Guid? AcademicYearId,
        Guid? ClassId,
        Guid? SectionId,
        string FirstName,
        string? LastName,
        DateOnly? DateOfBirth,
        string? Gender,
        string? Email,
        string? Phone,
        string? Address,
        string GuardianName,
        string? GuardianCnic,
        string? GuardianEmail,
        string? GuardianPhone,
        string? Relationship,
        string? PreviousSchool,
        decimal? PreviousMarks)
        : IRequest<Result<Response>>;

    public sealed record Response(Guid Id, string Status);

    public sealed class Handler(
        ITenantScope tenantScope,
        IAdmissionWorkflowQuery query,
        IAdmissionWorkflowCommand command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var branchIsValid = await query.BranchBelongsToSchoolAsync(
                tenantId.Value,
                request.SchoolId,
                request.BranchId,
                cancellationToken);

            if (!branchIsValid)
            {
                return Result<Response>.Failure(
                    Error.Validation("Selected branch does not belong to the selected school."));
            }

            if (string.IsNullOrWhiteSpace(request.Gender))
            {
                return Result<Response>.Failure(Error.Validation("Applicant gender is required."));
            }

            var branchGenderPolicy = await query.GetBranchGenderPolicyAsync(
                tenantId.Value,
                request.BranchId,
                cancellationToken);

            if (!GenderIsAllowed(branchGenderPolicy, request.Gender))
            {
                return Result<Response>.Failure(
                    Error.Validation("Applicant gender is not eligible for the selected branch."));
            }

            if (request.ClassId.HasValue)
            {
                var classIsEligible = await query.ClassIsEligibleForBranchAsync(
                    tenantId.Value,
                    request.BranchId,
                    request.ClassId.Value,
                    cancellationToken);

                if (!classIsEligible)
                {
                    return Result<Response>.Failure(
                        Error.Validation("The selected class is not available for this branch education level."));
                }
            }

            if (request.AcademicYearId.HasValue)
            {
                var academicYearIsValid = await query.AcademicYearBelongsToBranchAsync(
                    tenantId.Value,
                    request.BranchId,
                    request.AcademicYearId.Value,
                    cancellationToken);

                if (!academicYearIsValid)
                {
                    return Result<Response>.Failure(
                        Error.Validation("Academic year is not available for the selected branch."));
                }
            }

            var applicationId = await command.CreateApplicationAsync(
                tenantId.Value,
                request,
                cancellationToken);

            return Result<Response>.Success(
                new Response(
                    applicationId,
                    AdmissionApplicationStatus.SubmittedApplication.ToDatabaseValue()));
        }

        private static bool GenderIsAllowed(string? branchPolicy, string applicantGender)
        {
            if (string.Equals(branchPolicy, "CO_EDUCATION", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(branchPolicy, "BOYS_ONLY", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(applicantGender, "MALE", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(applicantGender, "BOY", StringComparison.OrdinalIgnoreCase);
            }

            if (string.Equals(branchPolicy, "GIRLS_ONLY", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(applicantGender, "FEMALE", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(applicantGender, "GIRL", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}

public static class ChangeAdmissionStatus
{
    public sealed record Body(
        Guid? TenantId,
        string Status,
        string? Notes);

    public sealed record Request(
        Guid Id,
        Guid? TenantId,
        AdmissionApplicationStatus Status,
        string? Notes)
        : IRequest<Result<Response>>;

    public sealed record Response(
        Guid Id,
        string Status,
        string? StudentNumber = null);

    public sealed class Handler(
        ITenantScope tenantScope,
        IAdmissionWorkflowQuery query,
        IAdmissionWorkflowCommand command,
        IIdentityAccountService accounts,
        IBusinessNumberGenerator numbers)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            if (request.Status != AdmissionApplicationStatus.AdmissionAccepted)
            {
                return await ChangeNonAdmissionStatusAsync(
                    tenantId.Value,
                    request,
                    cancellationToken);
            }

            return await AcceptAdmissionAsync(
                tenantId.Value,
                request,
                cancellationToken);
        }

        private async Task<Result<Response>> ChangeNonAdmissionStatusAsync(
            Guid tenantId,
            Request request,
            CancellationToken cancellationToken)
        {
            var changed = await command.ChangeStatusAsync(
                tenantId,
                request.Id,
                request.Status,
                request.Notes,
                cancellationToken);

            if (!changed)
            {
                return Result<Response>.Failure(
                    Error.NotFound("Admission application was not found."));
            }

            return Result<Response>.Success(
                new Response(request.Id, request.Status.ToDatabaseValue()));
        }

        private async Task<Result<Response>> AcceptAdmissionAsync(
            Guid tenantId,
            Request request,
            CancellationToken cancellationToken)
        {
            var application = await query.GetApplicationAsync(
                tenantId,
                request.Id,
                cancellationToken);

            if (application is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound("Admission application was not found."));
            }

            if (application.StudentId.HasValue)
            {
                return Result<Response>.Failure(
                    Error.Conflict("This application has already been admitted."));
            }

            if (string.IsNullOrWhiteSpace(application.Email) ||
                string.IsNullOrWhiteSpace(application.GuardianEmail))
            {
                return Result<Response>.Failure(
                    Error.Validation(
                        "Student and guardian email are required before admission is accepted."));
            }

            var branchCode = await query.GetBranchCodeAsync(
                tenantId,
                application.BranchId,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(branchCode))
            {
                return Result<Response>.Failure(
                    Error.Validation("Application branch is invalid."));
            }

            var studentId = Guid.NewGuid();
            var guardianId = Guid.NewGuid();
            var studentNumber = await numbers.NextAsync(
                $"STUDENT:{application.BranchId}",
                $"{branchCode}-",
                tenantId,
                7,
                cancellationToken);

            var studentAccount = await accounts.CreateAccountAsync(
                tenantId,
                studentId,
                SmartSchoolRoles.Student,
                application.Email,
                application.FirstName,
                application.LastName ?? string.Empty,
                application.SchoolId,
                application.BranchId,
                [SmartSchoolRoles.Student],
                cancellationToken);

            ProvisionedAccount? parentAccount = null;

            try
            {
                parentAccount = await accounts.CreateAccountAsync(
                    tenantId,
                    guardianId,
                    SmartSchoolRoles.Parent,
                    application.GuardianEmail,
                    application.GuardianName,
                    string.Empty,
                    application.SchoolId,
                    application.BranchId,
                    [SmartSchoolRoles.Parent],
                    cancellationToken);

                await command.CompleteAdmissionAsync(
                    tenantId,
                    application,
                    studentId,
                    studentAccount.UserId,
                    guardianId,
                    parentAccount.UserId,
                    studentNumber,
                    request.Notes,
                    cancellationToken);
            }
            catch
            {
                await accounts.DeactivateAccountAsync(
                    studentAccount.UserId,
                    cancellationToken);

                if (parentAccount is not null)
                {
                    await accounts.DeactivateAccountAsync(
                        parentAccount.UserId,
                        cancellationToken);
                }

                throw;
            }

            return Result<Response>.Success(
                new Response(
                    request.Id,
                    AdmissionApplicationStatus.AdmissionAccepted.ToDatabaseValue(),
                    studentNumber));
        }
    }
}

public static class GetAdmissionCriteria
{
    public sealed record Request(Guid? TenantId)
        : IRequest<Result<IReadOnlyList<AdmissionCriteriaDto>>>;

    public sealed class Handler(
        ITenantScope tenantScope,
        IAdmissionWorkflowQuery query)
        : IRequestHandler<Request, Result<IReadOnlyList<AdmissionCriteriaDto>>>
    {
        public async Task<Result<IReadOnlyList<AdmissionCriteriaDto>>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<IReadOnlyList<AdmissionCriteriaDto>>.Failure(
                    Error.Validation("Tenant context is required."));
            }

            var criteria = await query.GetCriteriaAsync(
                tenantId.Value,
                cancellationToken);

            return Result<IReadOnlyList<AdmissionCriteriaDto>>.Success(criteria);
        }
    }
}

public static class CreateAdmissionCriteria
{
    public sealed record Request(
        Guid? TenantId,
        Guid SchoolId,
        Guid BranchId,
        Guid AcademicYearId,
        Guid ClassId,
        decimal MinimumMarks,
        decimal? EntranceTestMinimum,
        int? MinimumAge,
        int? MaximumAge,
        bool InterviewRequired,
        string? RequiredDocuments)
        : IRequest<Result<Response>>;

    public sealed record Response(Guid Id);

    public sealed class Handler(
        ITenantScope tenantScope,
        IAdmissionWorkflowQuery query,
        IAdmissionWorkflowCommand command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var contextIsValid = await query.CriteriaContextIsValidAsync(
                tenantId.Value,
                request.SchoolId,
                request.BranchId,
                request.AcademicYearId,
                request.ClassId,
                cancellationToken);

            if (!contextIsValid)
            {
                return Result<Response>.Failure(
                    Error.Validation(
                        "School, branch, academic year and class must belong to the same tenant context."));
            }

            var criteriaId = await command.CreateCriteriaAsync(
                tenantId.Value,
                request,
                cancellationToken);

            return Result<Response>.Success(new Response(criteriaId));
        }
    }
}
