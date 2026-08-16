using SmartSchool.Modules.AIInquiry;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.InquiryMessage;

public static class DeleteInquiryMessage
{
    public sealed record Command(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IInquiryMessageQuery query,
        IInquiryMessageCommand command)
    {
        public async Task<Result<bool>> HandleAsync(
            Command command,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                command.TenantId,
                command.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<bool>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(InquiryMessage))));
            }

            await command.DeleteAsync(
                entity,
                cancellationToken);

            return Result<bool>.Success(true);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/aiinquiry/inquiry-message/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var command = new Command(
                        tenantId,
                        id);

                    var result = await handler.HandleAsync(
                        command,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("DeleteInquiryMessage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
