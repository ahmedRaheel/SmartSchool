using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIParent.Contracts;
using SmartSchool.Modules.AIParent.Models;
using SmartSchool.Modules.AIParent.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIParent.Features.ParentToolExecution;

public static class CreateParentToolExecution
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<ParentToolExecutionResponse>>;

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
        IParentToolExecutionQuery entityQuery,
        IParentToolExecutionCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<ParentToolExecutionResponse>>
    {
        public async Task<Result<ParentToolExecutionResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<ParentToolExecutionResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<ParentToolExecutionResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(ParentToolExecution), request.Code)));
            }

            var entity = new ParentToolExecution
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<ParentToolExecutionResponse>.Success(ParentToolExecutionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "parent-tool-execution"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<ParentToolExecutionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateParentToolExecution")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
