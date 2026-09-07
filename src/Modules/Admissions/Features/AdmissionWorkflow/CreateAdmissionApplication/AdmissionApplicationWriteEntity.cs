using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Features;

public sealed class AdmissionApplicationWriteEntity : Entity
{
    private AdmissionApplicationWriteEntity() { }
    public Guid ApplicationId { get; private set; } = Guid.NewGuid();
    public Guid SchoolId { get; private set; }
    public Guid BranchId { get; private set; }
    public Guid? AcademicYearId { get; private set; }
    public Guid? ClassId { get; private set; }
    public Guid? SectionId { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string? LastName { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public string? Gender { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }
    public string GuardianName { get; private set; } = string.Empty;
    public string? GuardianCnic { get; private set; }
    public string? GuardianEmail { get; private set; }
    public string? GuardianPhone { get; private set; }
    public string? Relationship { get; private set; }
    public string? PreviousSchool { get; private set; }
    public decimal? PreviousMarks { get; private set; }
    public string Status { get; private set; } = string.Empty;

    public static AdmissionApplicationWriteEntity Create(Guid tenantId, CreateAdmissionApplication.Request request) => new()
    {
        TenantId = tenantId, SchoolId = request.SchoolId, BranchId = request.BranchId, AcademicYearId = request.AcademicYearId,
        ClassId = request.ClassId, SectionId = request.SectionId, FirstName = request.FirstName.Trim(), LastName = request.LastName?.Trim(),
        DateOfBirth = request.DateOfBirth, Gender = request.Gender?.Trim(), Email = request.Email?.Trim(), Phone = request.Phone?.Trim(),
        Address = request.Address?.Trim(), GuardianName = request.GuardianName.Trim(), GuardianCnic = request.GuardianCnic?.Trim(),
        GuardianEmail = request.GuardianEmail?.Trim(), GuardianPhone = request.GuardianPhone?.Trim(), Relationship = request.Relationship?.Trim(),
        PreviousSchool = request.PreviousSchool?.Trim(), PreviousMarks = request.PreviousMarks,
        Status = AdmissionApplicationStatus.SubmittedApplication.ToDatabaseValue()
    };
}
