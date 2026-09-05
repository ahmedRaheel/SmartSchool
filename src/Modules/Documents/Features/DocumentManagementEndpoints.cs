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
        group.MapGet("/requirements/{actorType}", RequiredDocumentsAsync);
        group.MapGet("/compliance/{actorType}/{entityId:guid}", ComplianceAsync);
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
        await CreateTypedDocumentLinkAsync(connection, resolvedTenant.Value, entityType, entityId, documentId, documentType, ct);
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
    private static async Task<IResult> RequiredDocumentsAsync(string actorType, string? staffType, Guid? tenantId, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct)
    {
        var resolved = scope.Resolve(tenantId);
        if (!resolved.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
        const string sql = """
            SELECT required_document_id AS "Id", actor_type AS "ActorType", staff_type AS "StaffType", document_type AS "DocumentType",
                   display_name AS "DisplayName", is_required AS "IsRequired", condition_code AS "ConditionCode", min_count AS "MinCount", sort_order AS "SortOrder"
            FROM document.required_document
            WHERE is_active=true AND actor_type=@ActorType
              AND (tenant_id IS NULL OR tenant_id=@TenantId)
              AND (staff_type IS NULL OR staff_type=@StaffType)
            ORDER BY CASE WHEN tenant_id=@TenantId THEN 0 ELSE 1 END, sort_order, display_name
            """;
        await using var connection = await factory.OpenConnectionAsync(ct);
        var rows = await connection.QueryAsync(new CommandDefinition(sql, new { TenantId=resolved.Value, ActorType=actorType.ToUpperInvariant(), StaffType=staffType?.ToUpperInvariant() }, cancellationToken:ct));
        return Results.Ok(rows);
    }

    private static async Task<IResult> ComplianceAsync(string actorType, Guid entityId, string? staffType, Guid? tenantId, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct)
    {
        var resolved = scope.Resolve(tenantId);
        if (!resolved.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
        const string sql = """
            WITH requirements AS (
                SELECT DISTINCT ON (document_type) document_type, display_name, min_count, condition_code
                FROM document.required_document
                WHERE is_active=true AND is_required=true AND actor_type=@ActorType
                  AND (tenant_id IS NULL OR tenant_id=@TenantId) AND (staff_type IS NULL OR staff_type=@StaffType)
                ORDER BY document_type, CASE WHEN tenant_id=@TenantId THEN 0 ELSE 1 END
            ), uploaded AS (
                SELECT d.document_type, count(*)::int AS uploaded_count
                FROM document.document d JOIN document.document_link l ON l.document_id=d.document_id AND l.tenant_id=d.tenant_id
                WHERE d.tenant_id=@TenantId AND l.entity_type=@ActorType AND l.entity_id=@EntityId AND d.status='ACTIVE'
                GROUP BY d.document_type
            )
            SELECT r.document_type AS "DocumentType", r.display_name AS "DisplayName", r.min_count AS "RequiredCount",
                   COALESCE(u.uploaded_count,0) AS "UploadedCount", (COALESCE(u.uploaded_count,0) >= r.min_count) AS "Satisfied", r.condition_code AS "ConditionCode"
            FROM requirements r LEFT JOIN uploaded u ON u.document_type=r.document_type ORDER BY r.display_name
            """;
        await using var connection = await factory.OpenConnectionAsync(ct);
        var rows = (await connection.QueryAsync(new CommandDefinition(sql, new { TenantId=resolved.Value, ActorType=actorType.ToUpperInvariant(), EntityId=entityId, StaffType=staffType?.ToUpperInvariant() }, cancellationToken:ct))).ToList();
        return Results.Ok(new { compliant = rows.All(x => (bool)x.Satisfied), requirements = rows });
    }

    private static async Task CreateTypedDocumentLinkAsync(System.Data.Common.DbConnection connection, Guid tenantId, string entityType, Guid entityId, Guid documentId, string documentType, CancellationToken ct)
    {
        var type = entityType.Trim().ToUpperInvariant();
        string? sql = type switch
        {
            "TENANT" => "INSERT INTO document.tenant_document(tenant_id,document_id) VALUES(@TenantId,@DocumentId) ON CONFLICT DO NOTHING",
            "STUDENT" => "INSERT INTO document.student_document(tenant_id,student_id,document_id) VALUES(@TenantId,@EntityId,@DocumentId) ON CONFLICT DO NOTHING",
            "TEACHER" => "INSERT INTO document.teacher_document(tenant_id,teacher_id,document_id) VALUES(@TenantId,@EntityId,@DocumentId) ON CONFLICT DO NOTHING",
            "ADMIN_OFFICER" => "INSERT INTO document.admin_officer_document(tenant_id,employee_id,document_id) VALUES(@TenantId,@EntityId,@DocumentId) ON CONFLICT DO NOTHING",
            "EMPLOYEE" or "STAFF" => "INSERT INTO document.staff_document(tenant_id,employee_id,document_id) VALUES(@TenantId,@EntityId,@DocumentId) ON CONFLICT DO NOTHING",
            "DRIVER" => "INSERT INTO document.driver_document(tenant_id,driver_id,document_id) VALUES(@TenantId,@EntityId,@DocumentId) ON CONFLICT DO NOTHING",
            "GUARDIAN" or "PARENT" => "INSERT INTO document.guardian_document(tenant_id,guardian_id,document_id) VALUES(@TenantId,@EntityId,@DocumentId) ON CONFLICT DO NOTHING",
            "CAMPUS" or "BRANCH" => "INSERT INTO document.campus_document(tenant_id,campus_id,document_id) VALUES(@TenantId,@EntityId,@DocumentId) ON CONFLICT DO NOTHING",
            _ => null
        };
        if (sql is not null)
            await connection.ExecuteAsync(new CommandDefinition(sql, new { TenantId=tenantId, EntityId=entityId, DocumentId=documentId, DocumentType=documentType }, cancellationToken:ct));
    }

    private static string Clean(string value, string name) => string.IsNullOrWhiteSpace(value) ? throw new BadHttpRequestException($"{name} is required.") : value.Trim();
    private sealed record FileRow(byte[]? Data, string MimeType, string FileName);
}
