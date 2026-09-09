using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.Document;

public static class DeleteDocument
{
    public sealed record Request(Guid DocumentId) : IRequest<Result>;

    public interface IDeleteDocumentCommand
    {
        Task<bool> ExecuteAsync(Guid documentId, Guid tenantId, Guid? campusId, CancellationToken cancellationToken);
    }

    internal sealed class DeleteDocumentCommand(IDocumentsDbContext dbContext) : IDeleteDocumentCommand
    {
        public async Task<bool> ExecuteAsync(Guid documentId, Guid tenantId, Guid? campusId, CancellationToken cancellationToken)
        {
            var document = await dbContext.DocumentFiles.SingleOrDefaultAsync(
                entity => entity.DocumentId == documentId &&
                          entity.TenantId == tenantId &&
                          (campusId == null || entity.CampusId == campusId),
                cancellationToken);
            if (document is null)
            {
                return false;
            }

            document.Deactivate();
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public sealed class Handler(IDeleteDocumentCommand command, ICurrentUser currentUser) : IRequestHandler<Request, Result>
    {
        public async Task<Result> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (currentUser.TenantId is not Guid tenantId)
            {
                return Result.Failure(Error.Validation("Tenant context is required."));
            }

            return await command.ExecuteAsync(request.DocumentId, tenantId, currentUser.BranchId, cancellationToken)
                ? Result.Success()
                : Result.Failure(Error.NotFound("Document was not found."));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/documents/{documentId:guid}",
                async (Guid documentId, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result>(new Request(documentId), cancellationToken)).ToHttpResult())
            .WithName("DeleteDocument")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
