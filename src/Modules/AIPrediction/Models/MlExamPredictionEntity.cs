using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Models;

public sealed class MlExamPredictionEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid PredictionId { get; private set; } = Guid.NewGuid();

	private MlExamPredictionEntity() { }

	public Guid StudentId { get; private set; }
	public Guid SubjectId { get; private set; }
	public Guid? TargetExamId { get; private set; }
	public Guid? TargetExamSubjectId { get; private set; }
	public string TargetExamTypeCode { get; private set; } = string.Empty;
	public decimal PredictedMarks { get; private set; }
	public decimal PredictedPercentage { get; private set; }
	public string PredictedGrade { get; private set; } = string.Empty;
	public decimal LowerBoundPercentage { get; private set; }
	public decimal UpperBoundPercentage { get; private set; }
	public decimal ConfidenceScore { get; private set; }
	public decimal PassProbability { get; private set; }
	public string Trend { get; private set; } = string.Empty;
	public string RiskLevel { get; private set; } = string.Empty;
	public string ModelVersion { get; private set; } = string.Empty;
	public int HistoricalResultCount { get; private set; }
	public bool UsedMachineLearning { get; private set; }
	public DateTimeOffset GeneratedAt { get; private set; }

	public static MlExamPredictionEntity Create(
		Guid tenantId,
		Guid studentId,
		Guid subjectId,
		Guid? targetExamId,
		Guid? targetExamSubjectId,
		string targetExamTypeCode,
		decimal predictedMarks,
		decimal predictedPercentage,
		string predictedGrade,
		decimal lowerBoundPercentage,
		decimal upperBoundPercentage,
		decimal confidenceScore,
		decimal passProbability,
		string trend,
		string riskLevel,
		string modelVersion,
		int historicalResultCount,
		bool usedMachineLearning)
	{
		return new MlExamPredictionEntity
		{
			TenantId = tenantId,
			StudentId = studentId,
			SubjectId = subjectId,
			TargetExamId = targetExamId,
			TargetExamSubjectId = targetExamSubjectId,
			TargetExamTypeCode = targetExamTypeCode,
			PredictedMarks = predictedMarks,
			PredictedPercentage = predictedPercentage,
			PredictedGrade = predictedGrade,
			LowerBoundPercentage = lowerBoundPercentage,
			UpperBoundPercentage = upperBoundPercentage,
			ConfidenceScore = confidenceScore,
			PassProbability = passProbability,
			Trend = trend,
			RiskLevel = riskLevel,
			ModelVersion = modelVersion,
			HistoricalResultCount = historicalResultCount,
			UsedMachineLearning = usedMachineLearning,
			GeneratedAt = DateTimeOffset.UtcNow
		};
	}

	/// <summary>Gets the persisted prediction model id value.</summary>
	public Guid PredictionModelId { get; private set; }

	/// <summary>Gets the persisted prediction type value.</summary>
	public string PredictionType { get; private set; } = string.Empty;

	/// <summary>Gets the persisted score value.</summary>
	public decimal? Score { get; private set; }

	/// <summary>Gets the persisted explanation value.</summary>
	public string? Explanation { get; private set; }

	/// <summary>Gets the persisted predicted at value.</summary>
	public DateTimeOffset PredictedAt { get; private set; }
}
