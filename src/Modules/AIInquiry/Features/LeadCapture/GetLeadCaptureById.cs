using SmartSchool.Modules.AIInquiry;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.LeadCapture;

public static class GetLeadCaptureById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        ILeadCaptureQuery query)
    {
        public async Task<Result<LeadCapture>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<LeadCapture>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(LeadCapture))));
            }

            return Result<LeadCapture>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aiinquiry/lead-capture/{id:guid}",
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
            .WithName("GetLeadCaptureById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
