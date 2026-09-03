using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Models;

/// <summary>Represents a student's placement in a class section for an academic year.</summary>
public sealed class EnrollmentEntity : Entity
{
    public Guid StudentEnrollmentId { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public string EnrollmentNumber { get; private set; } = string.Empty;
    public Guid AcademicYearId { get; private set; }
    public Guid ClassSectionId { get; private set; }
    public DateOnly EnrollmentDate { get; private set; }
    public string Status { get; private set; } = string.Empty;

    private EnrollmentEntity() { }

    public static EnrollmentEntity Create(
        Guid tenantId,
        Guid studentId,
        string enrollmentNumber,
        Guid academicYearId,
        Guid classSectionId,
        DateOnly enrollmentDate,
        string status)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(status);
        return new EnrollmentEntity
        {
            TenantId = tenantId,
            StudentId = studentId,
            EnrollmentNumber = enrollmentNumber,
            AcademicYearId = academicYearId,
            ClassSectionId = classSectionId,
            EnrollmentDate = enrollmentDate,
            Status = status.Trim().ToUpperInvariant()
        };
    }

    public void ChangePlacement(Guid classSectionId, string status)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(status);
        ClassSectionId = classSectionId;
        Status = status.Trim().ToUpperInvariant();
        MarkAsUpdated();
    }
}
