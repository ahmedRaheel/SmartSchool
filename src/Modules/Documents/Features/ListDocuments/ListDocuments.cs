using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Documents.Features.ListDocuments;
public static class ListDocuments
{
    public sealed record Request(string EntityType, Guid EntityId, Guid? TenantId) : IRequest<IResult>;

    public sealed record Response(string DocumentNumber,string FileName,string MimeType,long SizeBytes,string Category,string DocumentType,string? Title,int Version,string Status,string Purpose,bool IsPrimary,DateTimeOffset UploadedAt);
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/documents/files/entity/{entityType}/{entityId:guid}", HandleAsync).WithTags("Documents").RequireAuthorization();
    public interface IListDocumentsQuery
    {
        Task<IResult> ExecuteAsync(string entityType, Guid entityId, Guid? tenantId, CancellationToken cancellationToken);
    }

    internal sealed class ListDocumentsQuery(ITenantScope tenantScope, IDbConnectionFactory connectionFactory) : IListDocumentsQuery
    {
        public async Task<IResult> ExecuteAsync(string entityType, Guid entityId, Guid? tenantId, CancellationToken cancellationToken)
        {
        var resolvedTenantId=tenantScope.Resolve(tenantId); if(!resolvedTenantId.HasValue) return Results.BadRequest();
        const string sql="""SELECT d.document_number AS "DocumentNumber",d.original_file_name AS "FileName",d.mime_type AS "MimeType",d.size_bytes AS "SizeBytes",d.category AS "Category",d.document_type AS "DocumentType",d.title AS "Title",d.version_no AS "Version",d.status AS "Status",l.purpose AS "Purpose",l.is_primary AS "IsPrimary",d.created_at AS "UploadedAt" FROM document.document d JOIN document.document_link l ON l.document_id=d.document_id AND l.tenant_id=d.tenant_id WHERE d.tenant_id=@TenantId AND l.entity_type=@EntityType AND l.entity_id=@EntityId AND d.status='ACTIVE' ORDER BY l.is_primary DESC,d.created_at DESC""";
        await using var connection=await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows=await connection.QueryAsync<Response>(new CommandDefinition(sql,new{TenantId=resolvedTenantId.Value,EntityType=entityType.ToUpperInvariant(),EntityId=entityId},cancellationToken:cancellationToken));
        return Results.Ok(rows);
    
        }
    }

    public sealed class Handler(IListDocumentsQuery query) : IRequestHandler<Request, IResult>
    {
        public Task<IResult> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request.EntityType, request.EntityId, request.TenantId, cancellationToken);
    }

    private static Task<IResult> HandleAsync(string entityType, Guid entityId, Guid? tenantId, CancellationToken cancellationToken, IMediator mediator) =>
        mediator.SendAsync<Request, IResult>(new Request(entityType, entityId, tenantId), cancellationToken);
}
