namespace SmartSchool.Modules.AIPrediction.ML;

public enum PredictionKind
{
    FailureRisk,
    GradeTrend,
    AttendanceRisk,
    DropoutRisk,
    FeeDefaultRisk,
    AssignmentCompletionRisk,
    SubjectDifficulty,
    PromotionRisk,
    AdmissionConversion,
    AdmissionSuccess,
    TeacherWorkloadRisk,
    TeacherStudentPerformance,
    PayrollAnomaly,
    StudentBehaviorRisk,
    TransportDelay,
    LibraryOverdueRisk,
    SchoolCapacityForecast,
    FeeCollectionForecast,
    EnrollmentForecast,
    EarlyWarning
}

public sealed record StudentPredictionRequest(Guid TenantId, Guid StudentId, Guid? SubjectId = null);
public sealed record AdmissionPredictionRequest(Guid TenantId, Guid ApplicantId);
public sealed record TeacherPredictionRequest(Guid TenantId, Guid TeacherEmployeeId);
public sealed record PayrollPredictionRequest(Guid TenantId, Guid? EmployeeId = null);
public sealed record TransportPredictionRequest(Guid TenantId, Guid RouteId);
public sealed record LibraryPredictionRequest(Guid TenantId, Guid StudentId);
public sealed record ForecastPredictionRequest(Guid TenantId, int HorizonMonths = 6);

public sealed record PredictionResult(
    PredictionKind Kind,
    decimal Score,
    decimal Probability,
    string RiskLevel,
    string Outcome,
    decimal Confidence,
    string ModelVersion,
    bool UsedMachineLearning,
    IReadOnlyList<string> Factors);

public sealed record ForecastPoint(DateOnly Period, decimal PredictedValue, decimal LowerBound, decimal UpperBound);

public sealed record ForecastResult(
    PredictionKind Kind,
    IReadOnlyList<ForecastPoint> Points,
    decimal Confidence,
    string ModelVersion,
    bool UsedMachineLearning);

public sealed record EarlyWarningResult(
    decimal OverallRiskScore,
    string RiskLevel,
    PredictionResult Academic,
    PredictionResult Attendance,
    PredictionResult Assignment,
    PredictionResult Fee,
    PredictionResult Dropout,
    PredictionResult Promotion,
    IReadOnlyList<string> TopFactors);
