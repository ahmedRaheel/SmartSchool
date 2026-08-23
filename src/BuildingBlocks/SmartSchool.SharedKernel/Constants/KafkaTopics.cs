namespace SmartSchool.SharedKernel.Constants;

public static class KafkaTopics
{
	public const string StudentEnrolled = "smartschool.student.enrolled";
	public const string ExamResultPublished = "smartschool.exam.result-published";
	public const string AttendanceRecorded = "smartschool.attendance.recorded";
	public const string FeePaymentReceived = "smartschool.finance.payment-received";
	public const string ChatMessageSent = "smartschool.communication.chat-message-sent";
	public const string NotificationCreated = "smartschool.communication.notification-created";
	public const string RagDocumentIngestionRequested = "smartschool.ai.rag-document-ingestion-requested";
	public const string ChatbotQuestionAsked = "smartschool.ai.chatbot-question-asked";
	public const string NotificationRequested = "smartschool.notification.requested";
	public const string PredictionRefreshRequested = "smartschool.ai.prediction-refresh-requested";
	public const string CagContextInvalidationRequested =
	   "smartschool.ai.cag-context-invalidation-requested";
}
