using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Models;

/// <summary>
/// Targets an assignment to a student. The student identifier is an external business identifier;
/// Learning does not reference the Students module assembly or entity model.
/// </summary>
public sealed class AssignmentStudentEntity : Entity
{
    private AssignmentStudentEntity()
    {
    }

    public Guid AssignmentStudentId { get; private set; } = Guid.NewGuid();

    public Guid AcademicAssignmentId { get; private set; }

    public Guid StudentId { get; private set; }

    public static AssignmentStudentEntity Create(
        Guid tenantId,
        Guid academicAssignmentId,
        Guid studentId)
    {
        if (tenantId == Guid.Empty)
        {
            throw new ArgumentException("Tenant id is required.", nameof(tenantId));
        }

        if (academicAssignmentId == Guid.Empty)
        {
            throw new ArgumentException("Assignment id is required.", nameof(academicAssignmentId));
        }

        if (studentId == Guid.Empty)
        {
            throw new ArgumentException("Student id is required.", nameof(studentId));
        }

        return new AssignmentStudentEntity
        {
            TenantId = tenantId,
            AcademicAssignmentId = academicAssignmentId,
            StudentId = studentId
        };
    }
}
