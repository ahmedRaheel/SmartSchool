using System.Security.Cryptography;
using Dapper;
using Microsoft.AspNetCore.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Documents.Features;

/// <summary>Central binary document store. Database storage is the initial provider; metadata remains stable when moved to cloud storage.</summary>
public static class DocumentManagementEndpoints
{
	private const long MaxFileSize = 25 * 1024 * 1024;

	public static IEndpointRouteBuilder MapDocumentManagement(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/documents/files")
			.WithTags("Documents").RequireAuthorization();
        group.MapPost("", UploadAsync).DisableAntiforgery();
        group.MapGet("/{documentId:guid}", DownloadAsync);
        group.MapGet("/entity/{entityType}/{entityId:guid}", ListAsync);
        group.MapDelete("/{documentId:guid}", ArchiveAsync);
        return endpoints;
	}

	private static async Task<IResult> UploadAsync(
        HttpRequest request, Guid? tenantId, ITenantScope tenantScope, IDbConnectionFactory factory, CancellationToken ct)
    {
        var resolvedTenant = tenantScope.Resolve(tenantId);
        if (!resolvedTenant.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
        if (!request.HasFormContentType) return Results.BadRequest(new { message = "multipart/form-data is required." });

        var form = await request.ReadFormAsync(ct);
		if (form is null || !form.Files.Any())
		{
			return Results.BadRequest(new
			{
				message = "A file is required."
			});
		}
		var file = form.Files.GetFile("file");
        if (file is null || file.Length == 0) 
			return Results.BadRequest(new { message = "A file is required." });

        if (file.Length > MaxFileSize)
			return Results.BadRequest(new { message = "File exceeds the 25 MB limit." });

        if (!Guid.TryParse(form["entityId"], out var entityId)) 
			return Results.BadRequest(new { message = "entityId is required." });

		var entityType = Clean(form["entityType"].ToString(), "entityType");
		var purpose = Clean(form["purpose"].ToString(), "purpose");
        var category = Clean(form["category"].ToString(), "category");
        var documentType = Clean(form["documentType"].ToString(), "documentType");
        Guid? schoolId = Guid.TryParse(form["schoolId"], out var school) ? school : null;
        Guid? branchId = Guid.TryParse(form["branchId"], out var branch) ? branch : null;

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory, ct);
        var bytes = memory.ToArray();
        var checksum = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        var documentId = Guid.NewGuid();
        var linkId = Guid.NewGuid();
        var documentNumber = $"DOC-{DateTime.UtcNow:yyyyMMdd}-{documentId.ToString("N")[..8].ToUpperInvariant()}";
        var extension = Path.GetExtension(file.FileName);

        const string sql = """
        INSERT INTO document.document(document_id,tenant_id,school_id,branch_id,document_number,original_file_name,stored_file_name,extension,mime_type,size_bytes,sha256,storage_provider,blob_data,category,document_type,title,status,is_confidential,uploaded_by)
        VALUES(@DocumentId,@TenantId,@SchoolId,@BranchId,@DocumentNumber,@OriginalFileName,@StoredFileName,@Extension,@MimeType,@SizeBytes,@Sha256,'DATABASE',@BlobData,@Category,@DocumentType,@Title,'ACTIVE',@IsConfidential,@UploadedBy);
        INSERT INTO document.document_link(document_link_id,tenant_id,document_id,entity_type,entity_id,purpose,is_primary)
        VALUES(@LinkId,@TenantId,@DocumentId,@EntityType,@EntityId,@Purpose,@IsPrimary);
        """;
        await using var connection = await factory.OpenConnectionAsync(ct);
        await connection.ExecuteAsync(new CommandDefinition(sql, new {
            DocumentId=documentId, LinkId=linkId, TenantId=resolvedTenant.Value, SchoolId=schoolId, BranchId=branchId,
            DocumentNumber=documentNumber, OriginalFileName=Path.GetFileName(file.FileName), StoredFileName=$"{documentId:N}{extension}", Extension=extension,
            MimeType=string.IsNullOrWhiteSpace(file.ContentType)?"application/octet-stream":file.ContentType, SizeBytes=file.Length, Sha256=checksum,
            BlobData=bytes, Category=category, DocumentType=documentType, Title=(string?)form["title"], IsConfidential=bool.TryParse(form["isConfidential"],out var c)&&c,
            UploadedBy=tenantScope.UserId, EntityType=entityType.ToUpperInvariant(), EntityId=entityId, Purpose=purpose.ToUpperInvariant(), IsPrimary=bool.TryParse(form["isPrimary"],out var p)&&p
        }, cancellationToken:ct));
        return Results.Created($"/api/documents/files/{documentId}", new { documentNumber, fileName=file.FileName, file.Length, category, documentType });
    }

    private static async Task<IResult> DownloadAsync(Guid documentId, Guid? tenantId, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct)
    {
        var resolved = scope.Resolve(tenantId); if(!resolved.HasValue) return Results.BadRequest();
        const string sql="SELECT blob_data AS \"Data\", mime_type AS \"MimeType\", original_file_name AS \"FileName\" FROM document.document WHERE tenant_id=@TenantId AND document_id=@DocumentId AND status='ACTIVE'";
        await using var c=await factory.OpenConnectionAsync(ct); var row=await c.QuerySingleOrDefaultAsync<FileRow>(new CommandDefinition(sql,new{TenantId=resolved.Value,DocumentId=documentId},cancellationToken:ct));
        return row?.Data is null ? Results.NotFound() : Results.File(row.Data,row.MimeType,row.FileName);
    }

    private static async Task<IResult> ListAsync(string entityType, Guid entityId, Guid? tenantId, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct)
    {
        var resolved=scope.Resolve(tenantId); if(!resolved.HasValue)return Results.BadRequest();
        const string sql="""SELECT d.document_number AS "DocumentNumber",d.original_file_name AS "FileName",d.mime_type AS "MimeType",d.size_bytes AS "SizeBytes",d.category AS "Category",d.document_type AS "DocumentType",d.title AS "Title",d.version_no AS "Version",d.status AS "Status",l.purpose AS "Purpose",l.is_primary AS "IsPrimary",d.created_at AS "UploadedAt" FROM document.document d JOIN document.document_link l ON l.document_id=d.document_id AND l.tenant_id=d.tenant_id WHERE d.tenant_id=@TenantId AND l.entity_type=@EntityType AND l.entity_id=@EntityId AND d.status='ACTIVE' ORDER BY l.is_primary DESC,d.created_at DESC""";
        await using var c=await factory.OpenConnectionAsync(ct); return Results.Ok(await c.QueryAsync(new CommandDefinition(sql,new{TenantId=resolved.Value,EntityType=entityType.ToUpperInvariant(),EntityId=entityId},cancellationToken:ct)));
    }

    private static async Task<IResult> ArchiveAsync(Guid documentId, Guid? tenantId, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct)
    {
        var resolved=scope.Resolve(tenantId); if(!resolved.HasValue)return Results.BadRequest(); await using var c=await factory.OpenConnectionAsync(ct);
        var n=await c.ExecuteAsync(new CommandDefinition("UPDATE document.document SET status='ARCHIVED',updated_at=now(),row_version=row_version+1 WHERE tenant_id=@TenantId AND document_id=@DocumentId",new{TenantId=resolved.Value,DocumentId=documentId},cancellationToken:ct)); return n==0?Results.NotFound():Results.NoContent();
    }
	private static string Clean(string value, string name) => string.IsNullOrWhiteSpace(value) ? throw new BadHttpRequestException($"{name} is required.") : value.Trim();
	private sealed record FileRow(byte[]? Data, string MimeType, string FileName);
}
