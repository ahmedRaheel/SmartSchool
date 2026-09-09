using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.DocumentSetup;

public static class CreateDocumentType
{
    public sealed record Request(DocumentOwnerType OwnerType, string Name, string? Description) : IRequest<Result<Response>>;
    public sealed record Response(Guid DocumentTypeId, string Code, string Name, DocumentOwnerType OwnerType);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(request => request.OwnerType).IsInEnum();
            RuleFor(request => request.Name).NotEmpty().MaximumLength(150);
            RuleFor(request => request.Description).MaximumLength(500);
        }
    }

    public interface ICreateDocumentTypeCommand
    {
        Task AddAsync(DocumentTypeEntity documentType, CancellationToken cancellationToken);
    }

    internal sealed class CreateDocumentTypeCommand(IDocumentsDbContext dbContext) : ICreateDocumentTypeCommand
    {
        public async Task AddAsync(DocumentTypeEntity documentType, CancellationToken cancellationToken)
        {
            await dbContext.DocumentTypes.AddAsync(documentType, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(
        ICreateDocumentTypeCommand command,
        IBusinessNumberGenerator numberGenerator,
        ICurrentUser currentUser) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (currentUser.TenantId is not Guid tenantId)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var code = await numberGenerator.NextAsync("DocumentType", "DTY", tenantId, 3, cancellationToken);
            var entity = DocumentTypeEntity.Create(tenantId, currentUser.BranchId, request.OwnerType, code, request.Name, request.Description);
            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(entity.DocumentTypeId, entity.Code, entity.Name, entity.OwnerType));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/documents/document-types", async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
            (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithName("CreateDocumentType").WithTags(ModuleConstants.Name).RequireAuthorization();
        return endpoints;
    }
}
