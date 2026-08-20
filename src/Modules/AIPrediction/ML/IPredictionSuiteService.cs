namespace SmartSchool.Modules.AIPrediction.ML;

public interface IPredictionSuiteService
{
	Task<PredictionResult> PredictStudentAsync(PredictionKind kind, StudentPredictionRequest request, CancellationToken cancellationToken);
	Task<PredictionResult> PredictAdmissionAsync(PredictionKind kind, AdmissionPredictionRequest request, CancellationToken cancellationToken);
	Task<PredictionResult> PredictTeacherAsync(PredictionKind kind, TeacherPredictionRequest request, CancellationToken cancellationToken);
	Task<PredictionResult> PredictPayrollAsync(PayrollPredictionRequest request, CancellationToken cancellationToken);
	Task<PredictionResult> PredictTransportAsync(TransportPredictionRequest request, CancellationToken cancellationToken);
	Task<PredictionResult> PredictLibraryAsync(LibraryPredictionRequest request, CancellationToken cancellationToken);
	Task<ForecastResult> ForecastAsync(PredictionKind kind, ForecastPredictionRequest request, CancellationToken cancellationToken);
	Task<EarlyWarningResult> GetEarlyWarningAsync(StudentPredictionRequest request, CancellationToken cancellationToken);
}
