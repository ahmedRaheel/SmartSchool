using SmartSchool.Modules.Admissions.Features;

namespace SmartSchool.Modules.Admissions.Features.DataAccess.AdmissionWorkflow;

public interface IAdmissionWorkflowQuery
{
    Task<IReadOnlyList<AdmissionApplicationDto>> GetApplicationsAsync(
        Guid tenantId,
        CancellationToken cancellationToken);

    Task<AdmissionApplicationDetails?> GetApplicationAsync(
        Guid tenantId,
        Guid applicationId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AdmissionCriteriaDto>> GetCriteriaAsync(
        Guid tenantId,
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

    Task<bool> CriteriaContextIsValidAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        Guid academicYearId,
        Guid classId,
        CancellationToken cancellationToken);

    Task<string?> GetBranchCodeAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken);
}
