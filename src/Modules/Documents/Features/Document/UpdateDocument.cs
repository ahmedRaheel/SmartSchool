using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.Document;

public static class UpdateDocument
{
    public sealed record Request(Guid DocumentId, Guid DocumentTypeId, Guid? RequiredDocumentTypeId, string? Title, bool IsConfidential)
        : IRequest<Result>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(request => request.DocumentId).NotEmpty();
            RuleFor(request => request.DocumentTypeId).NotEmpty();
            RuleFor(request => request.Title).MaximumLength(250);
        }
    }

    public interface IUpdateDocumentCommand
    {
        Task<bool> ExecuteAsync(Request request, Guid tenantId, Guid? campusId, CancellationToken cancellationToken);
    }

    internal sealed class UpdateDocumentCommand(IDocumentsDbContext dbContext) : IUpdateDocumentCommand
    {
        public async Task<bool> ExecuteAsync(Request request, Guid tenantId, Guid? campusId, CancellationToken cancellationToken)
        {
            var document = await dbContext.DocumentFiles.SingleOrDefaultAsync(
                entity => entity.DocumentId == request.DocumentId &&
                          entity.TenantId == tenantId &&
                          (campusId == null || entity.CampusId == campusId),
                cancellationToken);
            if (document is null)
            {
                return false;
            }

            document.UpdateMetadata(request.DocumentTypeId, request.RequiredDocumentTypeId, request.Title, request.IsConfidential);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public sealed class Handler(IUpdateDocumentCommand command, ICurrentUser currentUser) : IRequestHandler<Request, Result>
    {
        public async Task<Result> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (currentUser.TenantId is not Guid tenantId)
            {
                return Result.Failure(Error.Validation("Tenant context is required."));
            }

            return await command.ExecuteAsync(request, tenantId, currentUser.BranchId, cancellationToken)
                ? Result.Success()
                : Result.Failure(Error.NotFound("Document was not found."));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/documents/{documentId:guid}",
                async (Guid documentId, RequestBody body, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Request(documentId, body.DocumentTypeId, body.RequiredDocumentTypeId, body.Title, body.IsConfidential);
                    return (await mediator.SendAsync<Request, Result>(request, cancellationToken)).ToHttpResult();
                })
            .WithName("UpdateDocument")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    public sealed record RequestBody(Guid DocumentTypeId, Guid? RequiredDocumentTypeId, string? Title, bool IsConfidential);
}
