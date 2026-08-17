using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Contracts;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.KnowledgeChunk;

public static class CreateKnowledgeChunk
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<KnowledgeChunkResponse>>;

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
        IKnowledgeChunkQuery entityQuery,
        IKnowledgeChunkCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<KnowledgeChunkResponse>>
    {
        public async Task<Result<KnowledgeChunkResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<KnowledgeChunkResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<KnowledgeChunkResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(KnowledgeChunk), request.Code)));
            }

            var entity = new KnowledgeChunk
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<KnowledgeChunkResponse>.Success(KnowledgeChunkResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "knowledge-chunk"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<KnowledgeChunkResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateKnowledgeChunk")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
