namespace SmartSchool.Modules.Academics.Persistence;

public interface IAcademicSetupCommand
{
    Task<bool> BranchBelongsToSchoolAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        CancellationToken cancellationToken);

    Task<AcademicSetupItem> CreateAcademicYearAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        string name,
        string code,
        DateOnly startDate,
        DateOnly endDate,
        bool isCurrent,
        CancellationToken cancellationToken);

    Task<AcademicSetupItem> CreateClassAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        string name,
        string code,
        Guid educationLevelId,
        CancellationToken cancellationToken);

    Task<AcademicSetupItem> CreateSectionAsync(
        Guid tenantId,
        Guid branchId,
        Guid classId,
        string name,
        string code,
        CancellationToken cancellationToken);
}
