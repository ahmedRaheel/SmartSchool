using System.Net.Http.Json;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction;

public sealed record GradePredictionRequest(
    Guid StudentId,
    Guid SubjectId,
    Guid? TargetExamId);

public sealed record GradePredictionResponse(
    decimal PredictedPercentage,
    string PredictedGrade,
    decimal LowerBound,
    decimal UpperBound,
    decimal Confidence,
    decimal PassProbability,
    string Trend,
    string RiskLevel);

public sealed class PredictionClient(
    IHttpClientFactory httpClientFactory)
{
    public async Task<GradePredictionResponse?> PredictAsync(
        GradePredictionRequest request,
        CancellationToken cancellationToken)
    {
        var client =
            httpClientFactory.CreateClient(ApplicationConstants.MachineLearningHttpClient);

        using var response = await client.PostAsJsonAsync(
            ExternalServiceRoutes.GradePrediction,
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<GradePredictionResponse>(
            cancellationToken: cancellationToken);
    }
}

public sealed class RefreshPredictionsJob(
    ILogger<RefreshPredictionsJob> logger)
{
    public Task ExecuteAsync(
        Guid tenantId
       )
    {
        logger.LogInformation(
            "Refreshing predictions for tenant {TenantId}",
            tenantId);

        return Task.CompletedTask;
    }
}
