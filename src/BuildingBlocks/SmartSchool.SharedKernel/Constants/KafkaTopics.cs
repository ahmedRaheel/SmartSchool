namespace SmartSchool.SharedKernel.Constants;

public static class KafkaTopics
{
    public const string StudentEnrolled = "smartschool.student.enrolled";
    public const string ExamResultPublished = "smartschool.exam.result-published";
    public const string AttendanceRecorded = "smartschool.attendance.recorded";
    public const string FeePaymentReceived = "smartschool.finance.payment-received";
    public const string NotificationRequested = "smartschool.notification.requested";
    public const string PredictionRefreshRequested = "smartschool.ai.prediction-refresh-requested";
}
