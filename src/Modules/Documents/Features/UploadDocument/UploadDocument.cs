using System.Security.Cryptography;
using SmartSchool.Application.Identity;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.Modules.Documents.Persistence;

namespace SmartSchool.Modules.Documents.Features.UploadDocument;

public static class UploadDocument
{
    private const long MaxFileSize = 25 * 1024 * 1024;

    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapPost("/api/documents/files", HandleAsync).WithTags("Documents").RequireAuthorization().DisableAntiforgery();

    private static async Task<IResult> HandleAsync(HttpRequest request, Guid? tenantId, ITenantScope tenantScope, IDocumentsDbContext dbContext, TimeProvider timeProvider, CancellationToken cancellationToken)
    {
        var resolvedTenantId = tenantScope.Resolve(tenantId);
        if (!resolvedTenantId.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
        if (!request.HasFormContentType) return Results.BadRequest(new { message = "multipart/form-data is required." });

        var form = await request.ReadFormAsync(cancellationToken);
        var file = form.Files.GetFile("file");
        if (file is null || file.Length == 0) return Results.BadRequest(new { message = "A file is required." });
        if (file.Length > MaxFileSize) return Results.BadRequest(new { message = "File exceeds the 25 MB limit." });
        if (!Guid.TryParse(form["entityId"], out var entityId)) return Results.BadRequest(new { message = "entityId is required." });

        var entityType = Required(form["entityType"].ToString(), "entityType");
        var purpose = Required(form["purpose"].ToString(), "purpose");
        var category = Required(form["category"].ToString(), "category");
        var documentType = Required(form["documentType"].ToString(), "documentType");
        var schoolId = Guid.TryParse(form["schoolId"], out var school) ? school : (Guid?)null;
        var branchId = Guid.TryParse(form["branchId"], out var branch) ? branch : (Guid?)null;

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory, cancellationToken);
        var bytes = memory.ToArray();
        var documentId = Guid.NewGuid();
        var extension = Path.GetExtension(file.FileName);
        var documentNumber = $"DOC-{timeProvider.GetUtcNow():yyyyMMdd}-{documentId:N}"[..21].ToUpperInvariant();
        var originalFileName = Path.GetFileName(file.FileName);
        var mimeType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
        var checksum = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

        var document = DocumentFileEntity.Create(resolvedTenantId.Value, schoolId, branchId, documentNumber, originalFileName, $"{documentId:N}{extension}", extension, mimeType, file.Length, checksum, bytes, category, documentType, form["title"], bool.TryParse(form["isConfidential"], out var confidential) && confidential, tenantScope.UserId);
        var link = DocumentLinkEntity.Create(resolvedTenantId.Value, document.DocumentId, entityType, entityId, purpose, bool.TryParse(form["isPrimary"], out var primary) && primary);

        await dbContext.DocumentFiles.AddAsync(document, cancellationToken);
        await dbContext.DocumentLinks.AddAsync(link, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Created($"/api/documents/files/{document.DocumentId}", new { documentNumber, fileName = originalFileName, file.Length, category, documentType });
    }

    private static string Required(string value, string name) => string.IsNullOrWhiteSpace(value) ? throw new BadHttpRequestException($"{name} is required.") : value.Trim();
}
