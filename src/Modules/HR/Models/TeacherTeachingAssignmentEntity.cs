using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Models;

public sealed class TeacherTeachingAssignmentEntity : Entity
{
    public Guid TeacherTeachingAssignmentId { get; private set; } = Guid.NewGuid();
    public Guid SchoolId { get; private set; }
    public Guid CampusId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Guid ClassSectionId { get; private set; }
    public Guid SubjectId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public int? PeriodsPerWeek { get; private set; }
    public bool IsClassTeacher { get; private set; }
    public DateOnly? EffectiveFrom { get; private set; }
    public DateOnly? EffectiveTo { get; private set; }

    private TeacherTeachingAssignmentEntity()
    {
    }

    public static TeacherTeachingAssignmentEntity Create(
        Guid tenantId,
        Guid schoolId,
        Guid campusId,
        Guid employeeId,
        Guid classSectionId,
        Guid subjectId,
        string code,
        string name,
        int? periodsPerWeek,
        bool isClassTeacher,
        DateOnly? effectiveFrom,
        DateOnly? effectiveTo)
    {
        return new TeacherTeachingAssignmentEntity
        {
            TenantId = tenantId,
            SchoolId = schoolId,
            CampusId = campusId,
            EmployeeId = employeeId,
            ClassSectionId = classSectionId,
            SubjectId = subjectId,
            Code = code,
            Name = name,
            PeriodsPerWeek = periodsPerWeek,
            IsClassTeacher = isClassTeacher,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo
        };
    }
}
