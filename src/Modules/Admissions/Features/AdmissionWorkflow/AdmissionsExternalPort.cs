namespace SmartSchool.Modules.Admissions.Features;

/// <summary>
/// Admissions-owned boundary for data and actions that belong to other modules.
/// The Admissions module never references another module's DbContext, entity or table directly.
/// The host supplies an adapter at the composition boundary.
/// </summary>
public interface IAdmissionsExternalPort
{
    Task<bool> CriteriaContextIsValidAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        Guid academicYearId,
        Guid classId,
        CancellationToken cancellationToken);

    Task<bool> BranchBelongsToSchoolAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        CancellationToken cancellationToken);

    Task<string?> GetBranchGenderPolicyAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken);

    Task<bool> ClassIsEligibleForBranchAsync(
        Guid tenantId,
        Guid branchId,
        Guid classId,
        CancellationToken cancellationToken);

    Task<bool> AcademicYearBelongsToBranchAsync(
        Guid tenantId,
        Guid branchId,
        Guid academicYearId,
        CancellationToken cancellationToken);

    Task<bool> PlacementIsValidAsync(
        Guid tenantId,
        Guid branchId,
        Guid academicYearId,
        Guid classId,
        Guid classSectionId,
        CancellationToken cancellationToken);

    Task<AdmissionPlacementValidation?> ValidatePlacementAsync(
        Guid tenantId,
        AdmissionApplicationDetails application,
        CancellationToken cancellationToken);

    Task<string?> GetBranchCodeAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<string>> GetMissingApplicationDocumentsAsync(
        Guid tenantId,
        Guid applicationId,
        Guid branchId,
        IReadOnlyCollection<string> requiredDocumentCodes,
        CancellationToken cancellationToken);

    Task<ClassSectionAvailability?> GetClassSectionAvailabilityAsync(
        Guid tenantId,
        Guid classSectionId,
        CancellationToken cancellationToken);

    Task<int> GetActiveEnrollmentCountAsync(
        Guid tenantId,
        Guid classSectionId,
        CancellationToken cancellationToken);

    Task ProvisionAdmissionAsync(
        AdmissionProvisioningRequest request,
        CancellationToken cancellationToken);
}

public sealed record AdmissionPlacementValidation(
    bool Allowed,
    DateOnly StartDate);

public sealed record ClassSectionAvailability(
    int? Capacity,
    bool IsActive);

public sealed record AdmissionProvisioningRequest(
    Guid TenantId,
    Guid ApplicationId,
    Guid StudentId,
    Guid StudentUserId,
    Guid GuardianId,
    Guid GuardianUserId,
    Guid SchoolId,
    Guid BranchId,
    Guid? AcademicYearId,
    Guid? ClassSectionId,
    string StudentNumber,
    string FirstName,
    string? LastName,
    DateOnly? DateOfBirth,
    string? Gender,
    string GuardianName,
    string? GuardianCnic,
    string? GuardianEmail,
    string? GuardianPhone,
    string Relationship,
    DateOnly AdmissionDate);
