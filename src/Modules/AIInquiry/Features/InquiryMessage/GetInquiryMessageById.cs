using SmartSchool.Modules.AIInquiry;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.InquiryMessage;

public static class GetInquiryMessageById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IInquiryMessageQuery query)
    {
        public async Task<Result<InquiryMessage>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<InquiryMessage>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(InquiryMessage))));
            }

            return Result<InquiryMessage>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aiinquiry/inquiry-message/{id:guid}",
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
            .WithName("GetInquiryMessageById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
