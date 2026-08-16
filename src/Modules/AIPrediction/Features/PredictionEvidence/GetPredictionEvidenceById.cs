using SmartSchool.Modules.AIPrediction;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvidence;

public static class GetPredictionEvidenceById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IPredictionEvidenceQuery query)
    {
        public async Task<Result<PredictionEvidence>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<PredictionEvidence>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(PredictionEvidence))));
            }

            return Result<PredictionEvidence>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aiprediction/prediction-evidence/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(tenantId, id);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetPredictionEvidenceById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
