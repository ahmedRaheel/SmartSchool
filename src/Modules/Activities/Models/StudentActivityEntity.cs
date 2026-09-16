using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Models;

/// <summary>
/// Represents one student's participation in an activity.
/// </summary>
public sealed class StudentActivityEntity : Entity
{
    private StudentActivityEntity()
    {
    }

    public Guid StudentActivityId { get; private set; } = Guid.NewGuid();
    public Guid ActivityId { get; private set; }
    public Guid StudentId { get; private set; }
    public string? RoleName { get; private set; }
    public DateOnly JoinedAt { get; private set; }
    public DateOnly? LeftAt { get; private set; }

    public static StudentActivityEntity Create(
        Guid tenantId,
        Guid activityId,
        Guid studentId,
        string? roleName,
        DateOnly joinedAt)
    {
        return new StudentActivityEntity
        {
            TenantId = tenantId,
            ActivityId = activityId,
            StudentId = studentId,
            RoleName = NormalizeOptional(roleName),
            JoinedAt = joinedAt
        };
    }

    public void Update(string? roleName, DateOnly joinedAt, DateOnly? leftAt)
    {
        if (leftAt.HasValue && leftAt.Value < joinedAt)
        {
            throw new ArgumentException("Left date cannot be earlier than joined date.");
        }

        RoleName = NormalizeOptional(roleName);
        JoinedAt = joinedAt;
        LeftAt = leftAt;
        MarkAsUpdated();
    }

    public void Leave(DateOnly leftAt)
    {
        if (leftAt < JoinedAt)
        {
            throw new ArgumentException("Left date cannot be earlier than joined date.");
        }

        LeftAt = leftAt;
        MarkAsUpdated();
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
