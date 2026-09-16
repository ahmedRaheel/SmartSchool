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
    public Guid? ClassSectionId { get; private set; }
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
    public Guid? StudentId { get; private set; }
    public decimal? EntranceTestMarks { get; private set; }
    public bool? InterviewPassed { get; private set; }

    public void RecordReview(decimal? entranceTestMarks, bool? interviewPassed)
    {
        EntranceTestMarks = entranceTestMarks; InterviewPassed = interviewPassed; MarkAsUpdated();
    }

    public string? DecisionNotes { get; private set; }
    public DateTime? DecidedAt { get; private set; }
    public DateTimeOffset SubmittedAt { get; private set; } = DateTimeOffset.UtcNow;

    public void UpdateContact(string firstName, string? lastName, string guardianName, string? guardianPhone)
    {
        FirstName = firstName.Trim();
        LastName = lastName?.Trim();
        GuardianName = guardianName.Trim();
        GuardianPhone = guardianPhone?.Trim();
        MarkAsUpdated();
    }

    public void Accept(Guid studentId, string? notes, DateTime decidedAt)
    {
        if (StudentId.HasValue)
        {
            throw new InvalidOperationException("This application has already been admitted.");
        }

        StudentId = studentId;
        ChangeStatus(AdmissionApplicationStatus.AdmissionAccepted, notes, decidedAt);
    }

    public void ChangeStatus(AdmissionApplicationStatus status, string? notes, DateTime decidedAt)
    {
        if (StudentId.HasValue && status != AdmissionApplicationStatus.AdmissionAccepted)
        {
            throw new InvalidOperationException("Use the student lifecycle to change an admitted student's status.");
        }

        Status = status.ToDatabaseValue();
        DecisionNotes = notes?.Trim();
        DecidedAt = status == AdmissionApplicationStatus.SubmittedApplication ? null : decidedAt;
        MarkAsUpdated();
    }

    public static AdmissionApplicationWriteEntity Create(Guid tenantId, CreateAdmissionApplication.Request request) => new()
    {
        TenantId = tenantId, SchoolId = request.SchoolId, BranchId = request.BranchId, AcademicYearId = request.AcademicYearId,
        ClassId = request.ClassId, ClassSectionId = request.ClassSectionId, FirstName = request.FirstName.Trim(), LastName = request.LastName?.Trim(),
        DateOfBirth = request.DateOfBirth, Gender = request.Gender?.Trim(), Email = request.Email?.Trim(), Phone = request.Phone?.Trim(),
        Address = request.Address?.Trim(), GuardianName = request.GuardianName.Trim(), GuardianCnic = request.GuardianCnic?.Trim(),
        GuardianEmail = request.GuardianEmail?.Trim(), GuardianPhone = request.GuardianPhone?.Trim(), Relationship = request.Relationship?.Trim(),
        PreviousSchool = request.PreviousSchool?.Trim(), PreviousMarks = request.PreviousMarks,
        Status = AdmissionApplicationStatus.SubmittedApplication.ToDatabaseValue()
    };
}
