namespace SmartSchool.Modules.Academics.Persistence;

public sealed record AcademicSetupItem(
    Guid Id,
    string Name,
    string? Code,
    Guid BranchId,
    Guid? ParentId = null,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null,
    bool? IsCurrent = null,
    Guid? EducationLevelId = null,
    string? EducationLevelName = null);

public interface IAcademicSetupQuery
{
    Task<IReadOnlyCollection<AcademicSetupItem>> GetAcademicYearsAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<AcademicSetupItem>> GetClassesAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken);

    Task<bool> BranchAllowsEducationLevelAsync(
        Guid tenantId,
        Guid branchId,
        Guid educationLevelId,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<AcademicSetupItem>> GetSectionsAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken);
}
