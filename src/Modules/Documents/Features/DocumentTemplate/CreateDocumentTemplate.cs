using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Contracts;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Features.DocumentTemplate;

public static class CreateDocumentTemplate
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<DocumentTemplateResponse>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public sealed class Handler(
        IDocumentTemplateQuery entityQuery,
        IDocumentTemplateCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<DocumentTemplateResponse>>
    {
        public async Task<Result<DocumentTemplateResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<DocumentTemplateResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<DocumentTemplateResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(DocumentTemplate), request.Code)));
            }

            var entity = new DocumentTemplate
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<DocumentTemplateResponse>.Success(DocumentTemplateResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "document-template"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<DocumentTemplateResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateDocumentTemplate")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
