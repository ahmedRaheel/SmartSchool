using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Contracts;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Features.GeneratedDocument;

public static class GetGeneratedDocumentById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<GeneratedDocumentResponse>>;

    public sealed class Handler(IGeneratedDocumentQuery entityQuery)
        : IRequestHandler<Query, Result<GeneratedDocumentResponse>>
    {
        public async Task<Result<GeneratedDocumentResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<GeneratedDocumentResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(GeneratedDocument))));
            }
            return Result<GeneratedDocumentResponse>.Success(GeneratedDocumentResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "generated-document"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<GeneratedDocumentResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetGeneratedDocumentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
