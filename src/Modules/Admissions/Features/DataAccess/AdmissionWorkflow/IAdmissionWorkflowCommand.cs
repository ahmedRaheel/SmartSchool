using SmartSchool.Modules.Admissions.Features;

namespace SmartSchool.Modules.Admissions.Features.DataAccess.AdmissionWorkflow;

public interface IAdmissionWorkflowCommand
{
    Task<Guid> CreateApplicationAsync(
        Guid tenantId,
        CreateAdmissionApplication.Request request,
        CancellationToken cancellationToken);

    Task<bool> ChangeStatusAsync(
        Guid tenantId,
        Guid applicationId,
        AdmissionApplicationStatus status,
        string? notes,
        CancellationToken cancellationToken);

    Task CompleteAdmissionAsync(
        Guid tenantId,
        AdmissionApplicationDetails application,
        Guid studentId,
        Guid studentUserId,
        Guid guardianId,
        Guid guardianUserId,
        string studentNumber,
        string? notes,
        CancellationToken cancellationToken);

    Task<Guid> CreateCriteriaAsync(
        Guid tenantId,
        CreateAdmissionCriteria.Request request,
        CancellationToken cancellationToken);
}
