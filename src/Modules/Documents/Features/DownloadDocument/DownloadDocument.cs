using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Documents.Features.DownloadDocument;
public static class DownloadDocument
{
    public sealed record Request(Guid DocumentId, Guid? TenantId) : IRequest<IResult>;

    private sealed record Response(byte[]? Data, string MimeType, string FileName);
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/documents/files/{documentId:guid}", HandleAsync).WithTags("Documents").RequireAuthorization();
    public interface IDownloadDocumentQuery
    {
        Task<IResult> ExecuteAsync(Guid documentId, Guid? tenantId, CancellationToken cancellationToken);
    }

    internal sealed class DownloadDocumentQuery(ITenantScope tenantScope, IDbConnectionFactory connectionFactory) : IDownloadDocumentQuery
    {
        public async Task<IResult> ExecuteAsync(Guid documentId, Guid? tenantId, CancellationToken cancellationToken)
        {
        var resolvedTenantId = tenantScope.Resolve(tenantId); if (!resolvedTenantId.HasValue) return Results.BadRequest();
        const string sql = "SELECT blob_data AS \"Data\", mime_type AS \"MimeType\", original_file_name AS \"FileName\" FROM document.document WHERE tenant_id=@TenantId AND document_id=@DocumentId AND status='ACTIVE'";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var response = await connection.QuerySingleOrDefaultAsync<Response>(new CommandDefinition(sql, new { TenantId = resolvedTenantId.Value, DocumentId = documentId }, cancellationToken: cancellationToken));
        return response?.Data is null ? Results.NotFound() : Results.File(response.Data, response.MimeType, response.FileName);
    
        }
    }

    public sealed class Handler(IDownloadDocumentQuery query) : IRequestHandler<Request, IResult>
    {
        public Task<IResult> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request.DocumentId, request.TenantId, cancellationToken);
    }

    private static Task<IResult> HandleAsync(Guid documentId, Guid? tenantId, CancellationToken cancellationToken, IMediator mediator) =>
        mediator.SendAsync<Request, IResult>(new Request(documentId, tenantId), cancellationToken);
}
