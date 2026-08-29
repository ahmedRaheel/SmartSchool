namespace SmartSchool.Modules.Organization.Features.DataAccess.BranchPolicy;

public sealed record LookupItem(Guid Id, string Code, string Name);
public sealed record BranchPolicy(Guid BranchGenderTypeId, string GenderCode, IReadOnlyCollection<LookupItem> EducationLevels);

public interface IBranchPolicyQuery
{
    Task<IReadOnlyCollection<LookupItem>> GetGenderTypesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<LookupItem>> GetEducationLevelsAsync(CancellationToken cancellationToken);
    Task<BranchPolicy?> GetBranchPolicyAsync(Guid tenantId, Guid branchId, CancellationToken cancellationToken);
}
