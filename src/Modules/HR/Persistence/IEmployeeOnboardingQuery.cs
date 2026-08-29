namespace SmartSchool.Modules.HR.Persistence;

public interface IEmployeeOnboardingQuery
{
    Task<bool> CampusBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid campusId, CancellationToken cancellationToken);
    Task<bool> DepartmentBelongsToCampusAsync(Guid tenantId, Guid campusId, Guid departmentId, CancellationToken cancellationToken);
    Task<IReadOnlyList<string>> GetMissingRequiredDocumentsAsync(Guid tenantId, Guid employeeId, string staffType, CancellationToken cancellationToken);
    Task<bool> HasEducationAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken);
}
