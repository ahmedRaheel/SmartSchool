using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Identity;
using SmartSchool.Modules.Documents.Persistence;

namespace SmartSchool.Modules.Documents.Features.ArchiveDocument;
public static class ArchiveDocument
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapDelete("/api/documents/files/{documentId:guid}", HandleAsync).WithTags("Documents").RequireAuthorization();
    private static async Task<IResult> HandleAsync(Guid documentId, Guid? tenantId, ITenantScope tenantScope, IDocumentsDbContext dbContext, CancellationToken cancellationToken)
    {
        var resolvedTenantId=tenantScope.Resolve(tenantId); if(!resolvedTenantId.HasValue) return Results.BadRequest();
        var entity=await dbContext.DocumentFiles.SingleOrDefaultAsync(x=>x.TenantId==resolvedTenantId.Value && x.DocumentId==documentId,cancellationToken);
        if(entity is null) return Results.NotFound();
        entity.Archive();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }
}
