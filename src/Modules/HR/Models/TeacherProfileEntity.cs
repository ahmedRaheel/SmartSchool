using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Models;

/// <summary>
/// Stores detailed professional information for a teacher.
/// </summary>
public sealed class TeacherProfileEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid TeacherProfileId { get; private set; } = Guid.NewGuid();
    public Guid EmployeeId { get; private set; }
    public string EmployeeNumber { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Cnic { get; private set; } = string.Empty;
    public DateOnly DateOfBirth { get; private set; }
    public string GenderCode { get; private set; } = string.Empty;
    public string MobileNumber { get; private set; } = string.Empty;
    public string? EmailAddress { get; private set; }
    public string? Qualification { get; private set; }
    public string? Specialization { get; private set; }
    public int? TeachingExperienceYears { get; private set; }
    public DateOnly JoiningDate { get; private set; }
    public Guid? DepartmentId { get; private set; }
    public Guid? JobId { get; private set; }
    public Guid? JobGradeId { get; private set; }
    public string EmploymentStatusCode { get; private set; } = string.Empty;

    private TeacherProfileEntity() { }
}
