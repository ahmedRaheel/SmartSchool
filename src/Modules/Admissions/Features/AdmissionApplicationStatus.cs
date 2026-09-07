using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public enum AdmissionApplicationStatus
{
    SubmittedApplication,
    AdmissionAccepted,
    AdmissionRejected,
    WaitingList
}

public static class AdmissionApplicationStatusExtensions
{
    public static bool TryParseDatabaseValue(
        string? value,
        out AdmissionApplicationStatus status)
    {
        status = value?.Trim().ToUpperInvariant() switch
        {
            "SUBMITTED_APPLICATION" => AdmissionApplicationStatus.SubmittedApplication,
            "ADMISSION_ACCEPTED" => AdmissionApplicationStatus.AdmissionAccepted,
            "ADMISSION_REJECTED" => AdmissionApplicationStatus.AdmissionRejected,
            LifecycleStatuses.WaitingList => AdmissionApplicationStatus.WaitingList,
            _ => default
        };

        return value?.Trim().ToUpperInvariant() is
            "SUBMITTED_APPLICATION" or
            "ADMISSION_ACCEPTED" or
            "ADMISSION_REJECTED" or
            LifecycleStatuses.WaitingList;
    }

    public static string ToDatabaseValue(this AdmissionApplicationStatus status) => status switch
    {
        AdmissionApplicationStatus.SubmittedApplication => "SUBMITTED_APPLICATION",
        AdmissionApplicationStatus.AdmissionAccepted => "ADMISSION_ACCEPTED",
        AdmissionApplicationStatus.AdmissionRejected => "ADMISSION_REJECTED",
        AdmissionApplicationStatus.WaitingList => LifecycleStatuses.WaitingList,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };
}
