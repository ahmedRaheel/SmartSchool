using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Models;

public sealed class MlPredictionResultEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid MlPredictionResultId { get; private set; } = Guid.NewGuid();
    private MlPredictionResultEntity() { }

    public string PredictionType { get; private set; } = string.Empty;
    public Guid? StudentId { get; private set; }
    public Guid? SubjectId { get; private set; }
    public Guid? RelatedEntityId { get; private set; }
    public decimal Score { get; private set; }
    public decimal Probability { get; private set; }
    public string RiskLevel { get; private set; } = string.Empty;
    public string Outcome { get; private set; } = string.Empty;
    public decimal ConfidenceScore { get; private set; }
    public string ModelVersion { get; private set; } = string.Empty;
    public bool UsedMachineLearning { get; private set; }
    public string? FactorsJson { get; private set; }
    public DateTimeOffset GeneratedAt { get; private set; }

    public static MlPredictionResultEntity Create(
        Guid tenantId, string predictionType, decimal score, decimal probability,
        string riskLevel, string outcome, decimal confidenceScore, string modelVersion,
        bool usedMachineLearning, string? factorsJson, Guid? studentId=null,
        Guid? subjectId=null, Guid? relatedEntityId=null)
    {
        return new MlPredictionResultEntity {
            TenantId=tenantId, PredictionType=predictionType, StudentId=studentId,
            SubjectId=subjectId, RelatedEntityId=relatedEntityId, Score=score,
            Probability=probability, RiskLevel=riskLevel, Outcome=outcome,
            ConfidenceScore=confidenceScore, ModelVersion=modelVersion,
            UsedMachineLearning=usedMachineLearning, FactorsJson=factorsJson,
            GeneratedAt=DateTimeOffset.UtcNow
        };
    }
}
