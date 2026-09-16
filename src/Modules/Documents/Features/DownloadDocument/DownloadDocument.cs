using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Documents.Features.DownloadDocument;
public static class DownloadDocument
{
    public sealed record Request(Guid DocumentId, Guid? TenantId) : IRequest<IResult>;

    private sealed record Response(byte[]? Data, string MimeType, string FileName, string OwnerType, Guid OwnerId, Guid UploadedBy);
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/documents/files/{documentId:guid}", HandleAsync).WithTags("Documents").RequireAuthorization();
    public interface IDownloadDocumentQuery
    {
        Task<IResult> ExecuteAsync(Guid documentId, Guid? tenantId, CancellationToken cancellationToken);
    }

    internal sealed class DownloadDocumentQuery(ITenantScope tenantScope, IDbConnectionFactory connectionFactory, ICurrentUser currentUser) : IDownloadDocumentQuery
    {
        public async Task<IResult> ExecuteAsync(Guid documentId, Guid? tenantId, CancellationToken cancellationToken)
        {
        var resolvedTenantId = tenantScope.Resolve(tenantId); if (!resolvedTenantId.HasValue) return Results.BadRequest();
        const string sql = """
            SELECT d.blob_data AS "Data", d.mime_type AS "MimeType", d.original_file_name AS "FileName",
                d.owner_type AS "OwnerType", d.owner_id AS "OwnerId", d.uploaded_by AS "UploadedBy"
            FROM document.document d
            WHERE d.tenant_id = @TenantId AND d.document_id = @DocumentId AND d.status = 'ACTIVE' AND d.is_active = TRUE
              AND (@CampusId IS NULL OR d.campus_id = @CampusId)
              AND (@CanManage OR d.uploaded_by = @UserId
                OR (d.owner_type = 'StudentDocument' AND d.owner_id = @StudentId)
                OR (d.owner_type IN ('TeacherDocument', 'EmployeeDocument', 'ExaminerDocument') AND d.owner_id = @EmployeeId)
                OR (d.owner_type = 'DriverDocument' AND d.owner_id = @DriverId)
                OR (d.owner_type = 'ParentDocument' AND EXISTS (SELECT 1 FROM student.guardian g WHERE g.tenant_id = d.tenant_id AND g.guardian_id = d.owner_id AND g.user_id = @UserId))
                OR (d.owner_type = 'StudentDocument' AND NOT d.is_confidential AND EXISTS (
                    SELECT 1 FROM student.student_guardian sg JOIN student.guardian g ON g.guardian_id = sg.guardian_id AND g.tenant_id = sg.tenant_id
                    WHERE sg.tenant_id = d.tenant_id AND sg.student_id = d.owner_id AND g.user_id = @UserId AND sg.can_view_academics = TRUE)));
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var response = await connection.QuerySingleOrDefaultAsync<Response>(new CommandDefinition(sql, new { TenantId = resolvedTenantId.Value, DocumentId = documentId,
            CampusId = currentUser.BranchId, CanManage = Authorization.DocumentPermissions.CanManage(currentUser),
            currentUser.UserId, currentUser.StudentId, currentUser.EmployeeId, currentUser.DriverId }, cancellationToken: cancellationToken));
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
