namespace SmartSchool.Modules.AIPrediction.ML;

public interface IExamPredictionService
{
    Task<ExamPerformancePrediction> PredictAsync(
        PredictExamPerformanceRequest request,
        CancellationToken cancellationToken);
}
