using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.DocumentSetup;

public static class CreateRequiredDocumentType
{
    public sealed record Request(string Name, string? Description) : IRequest<Result<Response>>;
    public sealed record Response(Guid RequiredDocumentTypeId, string Code, string Name);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(request => request.Name).NotEmpty().MaximumLength(150);
            RuleFor(request => request.Description).MaximumLength(500);
        }
    }

    public interface ICreateRequiredDocumentTypeCommand
    {
        Task AddAsync(RequiredDocumentTypeEntity requiredDocumentType, CancellationToken cancellationToken);
    }

    internal sealed class CreateRequiredDocumentTypeCommand(IDocumentsDbContext dbContext) : ICreateRequiredDocumentTypeCommand
    {
        public async Task AddAsync(RequiredDocumentTypeEntity requiredDocumentType, CancellationToken cancellationToken)
        {
            await dbContext.RequiredDocumentTypes.AddAsync(requiredDocumentType, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(
        ICreateRequiredDocumentTypeCommand command,
        IBusinessNumberGenerator numberGenerator,
        ICurrentUser currentUser) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (currentUser.TenantId is not Guid tenantId)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var code = await numberGenerator.NextAsync("RequiredDocumentType", "RDT", tenantId, 3, cancellationToken);
            var entity = RequiredDocumentTypeEntity.Create(tenantId, currentUser.BranchId, code, request.Name, request.Description);
            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(entity.RequiredDocumentTypeId, entity.Code, entity.Name));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/documents/required-document-types", async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
            (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithName("CreateRequiredDocumentType").WithTags(ModuleConstants.Name).RequireAuthorization();
        return endpoints;
    }
}
