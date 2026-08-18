namespace SmartSchool.SharedKernel.Constants;

public static class JobConstants
{
	public static class Queues
	{
		public const string Default = "default";
		public const string Notifications = "notifications";
		public const string Payroll = "payroll";
		public const string Reports = "reports";
		public const string ArtificialIntelligence = "ai";
	}

	public static class RecurringJobs
	{
		public const string PredictionRefresh = "prediction-refresh";
		public const string FeeReminder = "fee-reminder";
		public const string OutboxRecovery = "outbox-recovery";
	}
}
