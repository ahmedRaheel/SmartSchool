using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Models;

/// <summary>
/// Represents a co-curricular activity or school event.
/// </summary>
public sealed class ActivityEntity : Entity
{
    private ActivityEntity()
    {
    }

    public Guid ActivityId { get; private set; } = Guid.NewGuid();
    public Guid? CampusId { get; private set; }
    public Guid? CoordinatorEmployeeId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = "OTHER";
    public DateOnly ActivityDate { get; private set; }
    public TimeOnly? StartTime { get; private set; }
    public TimeOnly? EndTime { get; private set; }
    public string? Venue { get; private set; }
    public string? Description { get; private set; }
    public int? MaxParticipants { get; private set; }
    public string Status { get; private set; } = "UPCOMING";

    public static ActivityEntity Create(
        Guid tenantId,
        string code,
        string name,
        string category,
        DateOnly activityDate,
        Guid? campusId,
        Guid? coordinatorEmployeeId,
        TimeOnly? startTime,
        TimeOnly? endTime,
        string? venue,
        string? description,
        int? maxParticipants,
        string status)
    {
        var entity = new ActivityEntity
        {
            TenantId = tenantId,
            Code = NormalizeRequired(code),
            Name = NormalizeRequired(name),
            Category = NormalizeRequired(category).ToUpperInvariant(),
            ActivityDate = activityDate,
            CampusId = campusId,
            CoordinatorEmployeeId = coordinatorEmployeeId,
            StartTime = startTime,
            EndTime = endTime,
            Venue = NormalizeOptional(venue),
            Description = NormalizeOptional(description),
            MaxParticipants = maxParticipants,
            Status = NormalizeRequired(status).ToUpperInvariant()
        };

        entity.ValidateTimeRange();
        entity.ValidateCapacity();
        return entity;
    }

    public void UpdateDetails(
        string name,
        string category,
        DateOnly activityDate,
        Guid? campusId,
        Guid? coordinatorEmployeeId,
        TimeOnly? startTime,
        TimeOnly? endTime,
        string? venue,
        string? description,
        int? maxParticipants,
        string status)
    {
        Name = NormalizeRequired(name);
        Category = NormalizeRequired(category).ToUpperInvariant();
        ActivityDate = activityDate;
        CampusId = campusId;
        CoordinatorEmployeeId = coordinatorEmployeeId;
        StartTime = startTime;
        EndTime = endTime;
        Venue = NormalizeOptional(venue);
        Description = NormalizeOptional(description);
        MaxParticipants = maxParticipants;
        Status = NormalizeRequired(status).ToUpperInvariant();

        ValidateTimeRange();
        ValidateCapacity();
        MarkAsUpdated();
    }

    private void ValidateTimeRange()
    {
        if (StartTime.HasValue && EndTime.HasValue && EndTime.Value <= StartTime.Value)
        {
            throw new ArgumentException("End time must be later than start time.");
        }
    }

    private void ValidateCapacity()
    {
        if (MaxParticipants.HasValue && MaxParticipants.Value <= 0)
        {
            throw new ArgumentException("Maximum participants must be greater than zero.");
        }
    }

    private static string NormalizeRequired(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        return value.Trim();
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
