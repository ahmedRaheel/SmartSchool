using SmartSchool.Modules.Admissions;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features.AdmissionDecision;

public static class GetAdmissionDecisionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IAdmissionDecisionQuery query)
    {
        public async Task<Result<AdmissionDecision>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<AdmissionDecision>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AdmissionDecision))));
            }

            return Result<AdmissionDecision>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admissions/admission-decision/{id:guid}",
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
            .WithName("GetAdmissionDecisionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
