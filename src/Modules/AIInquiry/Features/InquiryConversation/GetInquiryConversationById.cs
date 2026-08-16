using SmartSchool.Modules.AIInquiry;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.InquiryConversation;

public static class GetInquiryConversationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IInquiryConversationQuery query)
    {
        public async Task<Result<InquiryConversation>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<InquiryConversation>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(InquiryConversation))));
            }

            return Result<InquiryConversation>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aiinquiry/inquiry-conversation/{id:guid}",
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
            .WithName("GetInquiryConversationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
