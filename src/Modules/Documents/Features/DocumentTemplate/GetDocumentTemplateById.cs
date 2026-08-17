using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Contracts;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Features.DocumentTemplate;

public static class GetDocumentTemplateById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<DocumentTemplateResponse>>;

    public sealed class Handler(IDocumentTemplateQuery entityQuery)
        : IRequestHandler<Query, Result<DocumentTemplateResponse>>
    {
        public async Task<Result<DocumentTemplateResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<DocumentTemplateResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(DocumentTemplate))));
            }
            return Result<DocumentTemplateResponse>.Success(DocumentTemplateResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "document-template"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<DocumentTemplateResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetDocumentTemplateById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
