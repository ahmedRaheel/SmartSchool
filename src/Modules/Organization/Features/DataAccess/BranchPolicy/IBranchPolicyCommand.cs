namespace SmartSchool.Modules.Organization.Features.DataAccess.BranchPolicy;

public interface IBranchPolicyCommand
{
    Task<bool> GenderTypeExistsAsync(Guid genderTypeId, CancellationToken cancellationToken);
    Task<bool> EducationLevelsExistAsync(IReadOnlyCollection<Guid> educationLevelIds, CancellationToken cancellationToken);
    Task SetEducationLevelsAsync(Guid branchId, IReadOnlyCollection<Guid> educationLevelIds, CancellationToken cancellationToken);
}
