using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Models;

/// <summary>
/// A time-bound task assigned by an examiner to the teacher responsible for an exam subject.
/// </summary>
public sealed class ExamTaskEntity : Entity
{
    public Guid ExamTaskId { get; private set; } = Guid.NewGuid();
    public Guid ExamId { get; private set; }
    public Guid ExamSubjectId { get; private set; }
    public Guid CourseOfferingId { get; private set; }
    public Guid TeacherCourseAssignmentId { get; private set; }
    public Guid TeacherEmployeeId { get; private set; }
    public Guid TeacherUserId { get; private set; }
    public string TaskType { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string? Instructions { get; private set; }
    public Guid AssignedByUserId { get; private set; }
    public DateTimeOffset AssignedAt { get; private set; }
    public DateTimeOffset DueAt { get; private set; }
    public string Status { get; private set; } = "ASSIGNED";
    public DateTimeOffset? SubmittedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public Guid? CompletedByUserId { get; private set; }
    public string? SubmissionNotes { get; private set; }
    public string? SubmissionFileName { get; private set; }
    public string? SubmissionContentType { get; private set; }
    public byte[]? SubmissionFileData { get; private set; }
    public DateTimeOffset? AssignmentNotifiedAt { get; private set; }
    public DateTimeOffset? Reminder24HoursSentAt { get; private set; }
    public DateTimeOffset? Reminder2HoursSentAt { get; private set; }
    public DateTimeOffset? OverdueReminderSentAt { get; private set; }

    private ExamTaskEntity()
    {
    }

    public static ExamTaskEntity Assign(
        Guid tenantId,
        Guid examId,
        Guid examSubjectId,
        Guid courseOfferingId,
        Guid teacherCourseAssignmentId,
        Guid teacherEmployeeId,
        Guid teacherUserId,
        string taskType,
        string title,
        string? instructions,
        Guid assignedByUserId,
        DateTimeOffset dueAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskType);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        return new ExamTaskEntity
        {
            TenantId = tenantId,
            ExamId = examId,
            ExamSubjectId = examSubjectId,
            CourseOfferingId = courseOfferingId,
            TeacherCourseAssignmentId = teacherCourseAssignmentId,
            TeacherEmployeeId = teacherEmployeeId,
            TeacherUserId = teacherUserId,
            TaskType = taskType.Trim().ToUpperInvariant(),
            Title = title.Trim(),
            Instructions = string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim(),
            AssignedByUserId = assignedByUserId,
            AssignedAt = DateTimeOffset.UtcNow,
            DueAt = dueAt.ToUniversalTime(),
            Status = "ASSIGNED"
        };
    }

    public void UpdateAssignment(DateTimeOffset dueAt, string? instructions)
    {
        DueAt = dueAt.ToUniversalTime();
        Instructions = string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim();
        if (Status == "OVERDUE" && DueAt > DateTimeOffset.UtcNow)
        {
            Status = "ASSIGNED";
            OverdueReminderSentAt = null;
        }
        MarkAsUpdated();
    }

    public void SaveSubmissionNotes(string? notes)
    {
        SubmissionNotes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        if (Status == "ASSIGNED")
        {
            Status = "IN_PROGRESS";
        }
        MarkAsUpdated();
    }

    public void SubmitPaper(string fileName, string contentType, byte[] data, string? notes)
    {
        SubmissionFileName = fileName;
        SubmissionContentType = contentType;
        SubmissionFileData = data;
        SubmissionNotes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        SubmittedAt = DateTimeOffset.UtcNow;
        Status = "SUBMITTED";
        MarkAsUpdated();
    }

    public void MarkResultsSubmitted(string? notes)
    {
        SubmissionNotes = string.IsNullOrWhiteSpace(notes) ? SubmissionNotes : notes.Trim();
        SubmittedAt = DateTimeOffset.UtcNow;
        Status = "SUBMITTED";
        MarkAsUpdated();
    }

    public void Complete(Guid completedByUserId)
    {
        Status = "COMPLETED";
        CompletedAt = DateTimeOffset.UtcNow;
        CompletedByUserId = completedByUserId;
        MarkAsUpdated();
    }

    public void Reopen(DateTimeOffset dueAt)
    {
        Status = "ASSIGNED";
        DueAt = dueAt.ToUniversalTime();
        CompletedAt = null;
        CompletedByUserId = null;
        SubmittedAt = null;
        Reminder24HoursSentAt = null;
        Reminder2HoursSentAt = null;
        OverdueReminderSentAt = null;
        MarkAsUpdated();
    }
}
