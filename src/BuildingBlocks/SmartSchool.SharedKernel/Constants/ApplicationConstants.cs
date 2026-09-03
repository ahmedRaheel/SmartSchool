namespace SmartSchool.SharedKernel.Constants;

public static class ApplicationConstants
{
    public const string ApplicationName = "SmartSchool.Api";
    public const string ProductName = "SmartSchool";
    public const string HealthStatusOk = "ok";
    public const string MachineLearningHttpClient = "ml";
}

/// <summary>
/// Canonical lifecycle/status values persisted by SmartSchool modules.
/// Keep persisted status strings centralized to avoid magic values and drift.
/// </summary>
public static class LifecycleStatuses
{
    public const string Active = "ACTIVE";
    public const string Inactive = "INACTIVE";
    public const string Pending = "PENDING";
    public const string PendingApproval = "PENDING_APPROVAL";
    public const string Approved = "APPROVED";
    public const string Rejected = "REJECTED";
    public const string Hired = "HIRED";
    public const string Submitted = "SUBMITTED";
    public const string WaitingList = "WAITING_LIST";
}
