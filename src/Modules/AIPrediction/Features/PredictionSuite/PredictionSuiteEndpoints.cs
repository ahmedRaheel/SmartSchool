using System.Text.Json;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.ML;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionSuite;

public static class PredictionSuiteEndpoints
{
    public sealed record StudentRequest(PredictionKind Kind, StudentPredictionRequest Input) : IRequest<PredictionResult>;
    public sealed record EarlyWarningRequest(StudentPredictionRequest Input) : IRequest<EarlyWarningResult>;
    public sealed record AdmissionRequest(PredictionKind Kind, AdmissionPredictionRequest Input) : IRequest<PredictionResult>;
    public sealed record TeacherRequest(PredictionKind Kind, TeacherPredictionRequest Input) : IRequest<PredictionResult>;
    public sealed record PayrollRequest(PayrollPredictionRequest Input) : IRequest<PredictionResult>;
    public sealed record TransportRequest(TransportPredictionRequest Input) : IRequest<PredictionResult>;
    public sealed record LibraryRequest(LibraryPredictionRequest Input) : IRequest<PredictionResult>;
    public sealed record ForecastRequest(PredictionKind Kind, ForecastPredictionRequest Input) : IRequest<ForecastResult>;

    public interface IPredictionResultPersistence
    {
        Task AddAsync(Guid tenantId, PredictionResult result, CancellationToken cancellationToken,
            Guid? studentId = null, Guid? subjectId = null, Guid? relatedEntityId = null);
    }

    internal sealed class PredictionResultPersistence(IAIPredictionDbContext dbContext) : IPredictionResultPersistence
    {
        public async Task AddAsync(Guid tenantId, PredictionResult result, CancellationToken cancellationToken,
            Guid? studentId = null, Guid? subjectId = null, Guid? relatedEntityId = null)
        {
            var entity = MlPredictionResultEntity.Create(
                tenantId, result.Kind.ToString(), result.Score, result.Probability, result.RiskLevel,
                result.Outcome, result.Confidence, result.ModelVersion, result.UsedMachineLearning,
                JsonSerializer.Serialize(result.Factors), studentId, subjectId, relatedEntityId);

            await dbContext.MlPredictionResults.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class StudentHandler(IPredictionSuiteService service, IPredictionResultPersistence persistence)
        : IRequestHandler<StudentRequest, PredictionResult>
    {
        public async Task<PredictionResult> HandleAsync(StudentRequest request, CancellationToken cancellationToken)
        {
            var result = await service.PredictStudentAsync(request.Kind, request.Input, cancellationToken);
            await persistence.AddAsync(request.Input.TenantId, result, cancellationToken, request.Input.StudentId, request.Input.SubjectId);
            return result;
        }
    }

    public sealed class EarlyWarningHandler(IPredictionSuiteService service) : IRequestHandler<EarlyWarningRequest, EarlyWarningResult>
    {
        public Task<EarlyWarningResult> HandleAsync(EarlyWarningRequest request, CancellationToken cancellationToken) =>
            service.GetEarlyWarningAsync(request.Input, cancellationToken);
    }

    public sealed class AdmissionHandler(IPredictionSuiteService service, IPredictionResultPersistence persistence)
        : IRequestHandler<AdmissionRequest, PredictionResult>
    {
        public async Task<PredictionResult> HandleAsync(AdmissionRequest request, CancellationToken cancellationToken)
        {
            var result = await service.PredictAdmissionAsync(request.Kind, request.Input, cancellationToken);
            await persistence.AddAsync(request.Input.TenantId, result, cancellationToken, relatedEntityId: request.Input.ApplicantId);
            return result;
        }
    }

    public sealed class TeacherHandler(IPredictionSuiteService service, IPredictionResultPersistence persistence)
        : IRequestHandler<TeacherRequest, PredictionResult>
    {
        public async Task<PredictionResult> HandleAsync(TeacherRequest request, CancellationToken cancellationToken)
        {
            var result = await service.PredictTeacherAsync(request.Kind, request.Input, cancellationToken);
            await persistence.AddAsync(request.Input.TenantId, result, cancellationToken, relatedEntityId: request.Input.TeacherEmployeeId);
            return result;
        }
    }

    public sealed class PayrollHandler(IPredictionSuiteService service, IPredictionResultPersistence persistence)
        : IRequestHandler<PayrollRequest, PredictionResult>
    {
        public async Task<PredictionResult> HandleAsync(PayrollRequest request, CancellationToken cancellationToken)
        {
            var result = await service.PredictPayrollAsync(request.Input, cancellationToken);
            await persistence.AddAsync(request.Input.TenantId, result, cancellationToken, relatedEntityId: request.Input.EmployeeId);
            return result;
        }
    }

    public sealed class TransportHandler(IPredictionSuiteService service, IPredictionResultPersistence persistence)
        : IRequestHandler<TransportRequest, PredictionResult>
    {
        public async Task<PredictionResult> HandleAsync(TransportRequest request, CancellationToken cancellationToken)
        {
            var result = await service.PredictTransportAsync(request.Input, cancellationToken);
            await persistence.AddAsync(request.Input.TenantId, result, cancellationToken, relatedEntityId: request.Input.RouteId);
            return result;
        }
    }

    public sealed class LibraryHandler(IPredictionSuiteService service, IPredictionResultPersistence persistence)
        : IRequestHandler<LibraryRequest, PredictionResult>
    {
        public async Task<PredictionResult> HandleAsync(LibraryRequest request, CancellationToken cancellationToken)
        {
            var result = await service.PredictLibraryAsync(request.Input, cancellationToken);
            await persistence.AddAsync(request.Input.TenantId, result, cancellationToken, studentId: request.Input.StudentId);
            return result;
        }
    }

    public sealed class ForecastHandler(IPredictionSuiteService service) : IRequestHandler<ForecastRequest, ForecastResult>
    {
        public Task<ForecastResult> HandleAsync(ForecastRequest request, CancellationToken cancellationToken) =>
            service.ForecastAsync(request.Kind, request.Input, cancellationToken);
    }

    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/aiprediction").WithTags("AI Prediction").RequireAuthorization();

        group.MapPost("/student/{predictionKind}", async (PredictionKind predictionKind, StudentPredictionRequest request, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<StudentRequest, PredictionResult>(new StudentRequest(predictionKind, request), ct)));
        group.MapPost("/early-warning", async (StudentPredictionRequest request, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<EarlyWarningRequest, EarlyWarningResult>(new EarlyWarningRequest(request), ct)));
        group.MapPost("/admission/{predictionKind}", async (PredictionKind predictionKind, AdmissionPredictionRequest request, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<AdmissionRequest, PredictionResult>(new AdmissionRequest(predictionKind, request), ct)));
        group.MapPost("/teacher/{predictionKind}", async (PredictionKind predictionKind, TeacherPredictionRequest request, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<TeacherRequest, PredictionResult>(new TeacherRequest(predictionKind, request), ct)));
        group.MapPost("/payroll/anomaly", async (PayrollPredictionRequest request, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<PayrollRequest, PredictionResult>(new PayrollRequest(request), ct)));
        group.MapPost("/transport/delay", async (TransportPredictionRequest request, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<TransportRequest, PredictionResult>(new TransportRequest(request), ct)));
        group.MapPost("/library/overdue", async (LibraryPredictionRequest request, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<LibraryRequest, PredictionResult>(new LibraryRequest(request), ct)));
        group.MapPost("/forecast/{predictionKind}", async (PredictionKind predictionKind, ForecastPredictionRequest request, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<ForecastRequest, ForecastResult>(new ForecastRequest(predictionKind, request), ct)));
    }
}
