using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Contracts;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.WorkflowStep;

public static class CreateWorkflowStep
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<WorkflowStepResponse>>;

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
        IWorkflowStepQuery entityQuery,
        IWorkflowStepCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<WorkflowStepResponse>>
    {
        public async Task<Result<WorkflowStepResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<WorkflowStepResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<WorkflowStepResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(WorkflowStep), request.Code)));
            }

            var entity = new WorkflowStep
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<WorkflowStepResponse>.Success(WorkflowStepResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "workflow-step"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<WorkflowStepResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateWorkflowStep")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
