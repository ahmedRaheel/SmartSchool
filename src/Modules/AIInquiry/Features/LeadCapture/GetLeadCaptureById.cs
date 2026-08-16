using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Features.LeadCapture;

public static class GetLeadCaptureById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<LeadCapture> repository)
    {
        public async Task<Result<LeadCapture>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<LeadCapture>.Failure(
                    Error.NotFound("LeadCapture was not found."));
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
            .WithTags("AIInquiry")
            .RequireAuthorization();

        return endpoints;
    }
}
