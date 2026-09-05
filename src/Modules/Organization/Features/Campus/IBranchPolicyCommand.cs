namespace SmartSchool.Modules.Organization.Features.Campus;

public interface IBranchPolicyCommand
{
    Task<bool> GenderTypeExistsAsync(Guid genderTypeId, CancellationToken cancellationToken);
    Task<bool> EducationLevelsExistAsync(IReadOnlyCollection<Guid> educationLevelIds, CancellationToken cancellationToken);
    Task SetEducationLevelsAsync(Guid tenantId, Guid branchId, IReadOnlyCollection<Guid> educationLevelIds, CancellationToken cancellationToken);
}
