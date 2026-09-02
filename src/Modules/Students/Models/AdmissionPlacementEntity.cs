using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Models;

public sealed class AdmissionPlacementEntity : Entity
{
    private AdmissionPlacementEntity() { }
    public Guid AdmissionPlacementId { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public Guid AcademicYearId { get; private set; }
    public Guid ClassSectionId { get; private set; }
    public DateTimeOffset RequestedAt { get; private set; } = DateTimeOffset.UtcNow;
    public string Status { get; private set; } = LifecycleStatuses.Pending;
    public DateTimeOffset? ApprovedAt { get; private set; }

    public static AdmissionPlacementEntity Create(Guid tenantId, Guid studentId, Guid academicYearId, Guid classSectionId) => new()
    {
        TenantId = tenantId,
        StudentId = studentId,
        AcademicYearId = academicYearId,
        ClassSectionId = classSectionId
    };

    public void Approve()
    {
        Status = LifecycleStatuses.Approved;
        ApprovedAt = DateTimeOffset.UtcNow;
        MarkAsUpdated();
    }
}
