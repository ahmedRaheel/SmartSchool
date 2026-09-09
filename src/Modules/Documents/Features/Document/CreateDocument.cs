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

    public sealed record Response(
        Guid DocumentId,
        string DocumentNumber,
        Guid DocumentTypeId,
        DocumentOwnerType OwnerType,
        Guid OwnerId,
        string FileName,
        long SizeBytes);

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
        ICurrentUser currentUser)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (currentUser.TenantId is not Guid tenantId)
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
                !Enum.TryParse<DocumentOwnerType>(form["ownerType"], true, out var ownerType))
            {
                return Result<Response>.Failure(Error.Validation("documentTypeId, ownerId and a valid ownerType are required."));
            }

            var requiredDocumentTypeId = Guid.TryParse(form["requiredDocumentTypeId"], out var requiredTypeId)
                ? requiredTypeId
                : (Guid?)null;

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
                currentUser.BranchId,
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
