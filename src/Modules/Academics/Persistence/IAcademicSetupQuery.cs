namespace SmartSchool.Modules.Academics.Persistence;

public sealed class AcademicSetupItem
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Code { get; init; }

    public Guid BranchId { get; init; }

    public Guid? ParentId { get; init; }

    public DateOnly? StartDate { get; init; }

    public DateOnly? EndDate { get; init; }

    public bool? IsCurrent { get; init; }

    public Guid? EducationLevelId { get; init; }

    public string? EducationLevelName { get; init; }
}


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
