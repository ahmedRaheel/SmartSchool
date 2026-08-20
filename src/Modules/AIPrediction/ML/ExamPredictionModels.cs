namespace SmartSchool.Modules.AIPrediction.ML;

public sealed record PredictExamPerformanceRequest(
	Guid TenantId,
	Guid StudentId,
	Guid SubjectId,
	string TargetExamTypeCode,
	Guid? TargetExamId = null,
	Guid? TargetExamSubjectId = null);

public sealed record ExamPerformancePrediction(
	decimal PredictedMarks,
	decimal PredictedPercentage,
	string PredictedGrade,
	decimal LowerBoundPercentage,
	decimal UpperBoundPercentage,
	decimal Confidence,
	decimal PassProbability,
	string Trend,
	string RiskLevel,
	string TargetExamTypeCode,
	string ModelVersion,
	int HistoricalResultCount,
	bool UsedMachineLearning);

internal sealed class ExamTrainingRow
{
	public float PreviousPercentage { get; set; }
	public float AveragePercentage { get; set; }
	public float RecentThreeAverage { get; set; }
	public float Trend { get; set; }
	public float ResultCount { get; set; }
	public string ExamTypeCode { get; set; } = string.Empty;
	public float Label { get; set; }
}

internal sealed class ExamScorePrediction
{
	public float Score { get; set; }
}

internal sealed record HistoricalExamResult(
	Guid StudentId,
	DateTime ExamDate,
	string ExamTypeCode,
	decimal Percentage,
	decimal TotalMarks);
