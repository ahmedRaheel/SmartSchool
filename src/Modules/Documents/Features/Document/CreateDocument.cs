using Dapper;
using System.Security.Cryptography;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.Document;

public static class CreateDocument
{
    private const long MaxFileSize = 25 * 1024 * 1024;

    public sealed record Request(HttpRequest HttpRequest) : IRequest<Result<Response>>;

    public sealed record OwnerScope(Guid? CampusId, Guid? UserId);

    public sealed record Response(
        Guid DocumentId,
        string DocumentNumber,
        Guid DocumentTypeId,
        DocumentOwnerType OwnerType,
        Guid OwnerId,
        string FileName,
        long SizeBytes);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator() { RuleFor(x => x.HttpRequest).NotNull(); }
    }

    public interface ICreateDocumentQuery
    {
        Task<bool> TypesBelongToTenantAsync(Guid tenantId, Guid documentTypeId, Guid? requiredDocumentTypeId, CancellationToken cancellationToken);
        Task<OwnerScope?> GetOwnerAsync(Guid tenantId, Guid ownerId, DocumentOwnerType ownerType, CancellationToken cancellationToken);
    }

    internal sealed class CreateDocumentQuery(IDbConnectionFactory connectionFactory) : ICreateDocumentQuery
    {
        public async Task<bool> TypesBelongToTenantAsync(Guid tenantId, Guid documentTypeId, Guid? requiredDocumentTypeId, CancellationToken cancellationToken)
        {
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            const string sql = """
                SELECT EXISTS (SELECT 1 FROM document.document_type
                    WHERE document_type_id = @DocumentTypeId AND tenant_id = @TenantId AND is_active = TRUE)
                AND (@RequiredTypeId IS NULL OR EXISTS (SELECT 1 FROM document.required_document_type
                    WHERE required_document_type_id = @RequiredTypeId AND tenant_id = @TenantId AND is_active = TRUE));
                """;
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql,
                new { DocumentTypeId = documentTypeId, TenantId = tenantId, RequiredTypeId = requiredDocumentTypeId }, cancellationToken: cancellationToken));
        }
        public async Task<OwnerScope?> GetOwnerAsync(Guid tenantId, Guid ownerId, DocumentOwnerType ownerType, CancellationToken cancellationToken)
        {
            var ownerSql = ownerType switch
            {
                DocumentOwnerType.StudentDocument => "SELECT branch_id AS CampusId, user_id AS UserId FROM student.student WHERE student_id = @OwnerId AND tenant_id = @TenantId AND is_active = TRUE",
                DocumentOwnerType.AdmissionDocument => "SELECT branch_id AS CampusId, NULL::uuid AS UserId FROM admission.student_application WHERE application_id = @OwnerId AND tenant_id = @TenantId AND is_active = TRUE",
                DocumentOwnerType.TeacherDocument or DocumentOwnerType.EmployeeDocument or DocumentOwnerType.ExaminerDocument => "SELECT branch_id AS CampusId, user_id AS UserId FROM hr.employee WHERE employee_id = @OwnerId AND tenant_id = @TenantId AND is_active = TRUE",
                DocumentOwnerType.ParentDocument => "SELECT NULL::uuid AS CampusId, user_id AS UserId FROM student.guardian WHERE guardian_id = @OwnerId AND tenant_id = @TenantId AND is_active = TRUE",
                DocumentOwnerType.CampusDocument => "SELECT campus_id AS CampusId, NULL::uuid AS UserId FROM org.campus WHERE campus_id = @OwnerId AND tenant_id = @TenantId AND is_active = TRUE",
                DocumentOwnerType.DriverDocument => "SELECT e.branch_id AS CampusId, e.user_id AS UserId FROM transport.driver d LEFT JOIN hr.employee e ON e.employee_id = d.employee_id AND e.tenant_id = d.tenant_id WHERE d.driver_id = @OwnerId AND d.tenant_id = @TenantId AND d.is_active = TRUE",
                _ => null
            };
            if (ownerSql is null) return null;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<OwnerScope>(new CommandDefinition(ownerSql,
                new { OwnerId = ownerId, TenantId = tenantId }, cancellationToken: cancellationToken));
        }
    }

    public interface ICreateDocumentCommand
    {
        Task AddAsync(DocumentFileEntity document, CancellationToken cancellationToken);
    }

    internal sealed class CreateDocumentCommand(IDocumentsDbContext dbContext) : ICreateDocumentCommand
    {
        public async Task AddAsync(DocumentFileEntity document, CancellationToken cancellationToken)
        {
            await dbContext.DocumentFiles.AddAsync(document, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(
        ICreateDocumentCommand command,
        IBusinessNumberGenerator numberGenerator,
        ICurrentUser currentUser,
        ITenantScope tenantScope,
        ICreateDocumentQuery query)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (tenantScope.Resolve(Guid.TryParse(request.HttpRequest.Query["tenantId"], out var requestedTenantId) ? requestedTenantId : null) is not Guid tenantId)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            if (!request.HttpRequest.HasFormContentType)
            {
                return Result<Response>.Failure(Error.Validation("multipart/form-data is required."));
            }

            var form = await request.HttpRequest.ReadFormAsync(cancellationToken);
            var file = form.Files.GetFile("file");
            if (file is null || file.Length == 0 || file.Length > MaxFileSize)
            {
                return Result<Response>.Failure(Error.Validation("A file up to 25 MB is required."));
            }

            if (!Guid.TryParse(form["documentTypeId"], out var documentTypeId) ||
                !Guid.TryParse(form["ownerId"], out var ownerId) ||
                !Enum.TryParse<DocumentOwnerType>(form["ownerType"], true, out var ownerType) || !Enum.IsDefined(ownerType))
            {
                return Result<Response>.Failure(Error.Validation("documentTypeId, ownerId and a valid ownerType are required."));
            }

            var requiredDocumentTypeId = Guid.TryParse(form["requiredDocumentTypeId"], out var requiredTypeId)
                ? requiredTypeId
                : (Guid?)null;

            if (!await query.TypesBelongToTenantAsync(tenantId, documentTypeId, requiredDocumentTypeId, cancellationToken))
                return Result<Response>.Failure(Error.Validation("The document type does not belong to this tenant."));
            var owner = await query.GetOwnerAsync(tenantId, ownerId, ownerType, cancellationToken);
            if (owner is null || currentUser.BranchId.HasValue && owner.CampusId.HasValue && currentUser.BranchId != owner.CampusId)
            {
                return Result<Response>.Failure(Error.Validation("The document owner is outside the current campus."));
            }
            if (!Authorization.DocumentPermissions.CanManage(currentUser) && owner.UserId != currentUser.UserId)
            {
                throw new UnauthorizedAccessException("You cannot upload documents for another person.");
            }

            await using var input = file.OpenReadStream();
            using var memory = new MemoryStream();
            await input.CopyToAsync(memory, cancellationToken);
            var bytes = memory.ToArray();
            var extension = Path.GetExtension(file.FileName);
            var documentNumber = await numberGenerator.NextAsync("Document", "DOC", tenantId, 7, cancellationToken);
            var storedFileName = $"{Guid.NewGuid():N}{extension}";
            var mimeType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
            var sha256 = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

            var document = DocumentFileEntity.Create(
                tenantId,
                owner.CampusId ?? currentUser.BranchId,
                documentTypeId,
                requiredDocumentTypeId,
                ownerId,
                ownerType,
                documentNumber,
                Path.GetFileName(file.FileName),
                storedFileName,
                extension,
                mimeType,
                file.Length,
                sha256,
                bytes,
                form["title"],
                bool.TryParse(form["isConfidential"], out var confidential) && confidential,
                currentUser.UserId);

            await command.AddAsync(document, cancellationToken);

            return Result<Response>.Success(new Response(
                document.DocumentId,
                document.DocumentNumber,
                document.DocumentTypeId,
                document.OwnerType,
                document.OwnerId,
                document.OriginalFileName,
                document.SizeBytes));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/documents",
                async (HttpRequest httpRequest, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(new Request(httpRequest), cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateDocument")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization()
            .DisableAntiforgery();

        return endpoints;
    }
}
