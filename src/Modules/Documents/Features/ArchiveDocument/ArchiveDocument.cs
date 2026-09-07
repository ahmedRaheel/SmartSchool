using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Identity;
using SmartSchool.Modules.Documents.Persistence;

namespace SmartSchool.Modules.Documents.Features.ArchiveDocument;

public static class ArchiveDocument
{
    public sealed record Request(Guid DocumentId, Guid? TenantId);
    public sealed record Response(Guid DocumentId);

    public interface IArchiveDocumentCommand
    {
        Task<bool> ExecuteAsync(Guid tenantId, Guid documentId, CancellationToken cancellationToken);
    }

    internal sealed class ArchiveDocumentCommand(IDocumentsDbContext dbContext) : IArchiveDocumentCommand
    {
        public async Task<bool> ExecuteAsync(Guid tenantId, Guid documentId, CancellationToken cancellationToken)
        {
            var entity = await dbContext.DocumentFiles.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.DocumentId == documentId, cancellationToken);
            if (entity is null) return false;
            entity.Archive();
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public sealed class Handler(IArchiveDocumentCommand command)
    {
        public Task<bool> HandleAsync(Guid tenantId, Guid documentId, CancellationToken cancellationToken) => command.ExecuteAsync(tenantId, documentId, cancellationToken);
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapDelete("/api/documents/files/{documentId:guid}", HandleAsync).WithTags("Documents").RequireAuthorization();

    private static async Task<IResult> HandleAsync(Guid documentId, Guid? tenantId, ITenantScope tenantScope, Handler handler, CancellationToken cancellationToken)
    {
        var resolvedTenantId = tenantScope.Resolve(tenantId);
        if (!resolvedTenantId.HasValue) return Results.BadRequest();
        return await handler.HandleAsync(resolvedTenantId.Value, documentId, cancellationToken) ? Results.NoContent() : Results.NotFound();
    }
}
