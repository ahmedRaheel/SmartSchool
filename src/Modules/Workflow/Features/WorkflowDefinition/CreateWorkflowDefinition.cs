using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.WorkflowDefinition;

public static class CreateWorkflowDefinition
{
    public sealed record StepRequest(string Name, string StepType, string? ApproverRole, string? ActionCode, bool IsRequired = true);
    public sealed record Response(Guid TenantId, Guid Id, string Code, string Name, int Version);
    public sealed record Request(Guid? TenantId, string Name, string? Description, string TriggerType, string EntityType, string Status, IReadOnlyList<StepRequest> Steps) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
            RuleFor(x => x.TriggerType).NotEmpty().MaximumLength(50);
            RuleFor(x => x.EntityType).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Status).Must(x => x is "ACTIVE" or "INACTIVE").WithMessage("Status must be ACTIVE or INACTIVE.");
            RuleFor(x => x.Steps).NotEmpty().WithMessage("At least one workflow step is required.");
            RuleForEach(x => x.Steps).ChildRules(step =>
            {
                step.RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
                step.RuleFor(x => x.StepType).Must(x => x.ToUpperInvariant() is "APPROVAL" or "ACTION");
                step.RuleFor(x => x.ApproverRole).NotEmpty().When(x => x.StepType.Equals("APPROVAL", StringComparison.OrdinalIgnoreCase));
            });
        }
    }

    public interface ICreateWorkflowDefinition
    {
        Task AddAsync(WorkflowDefinitionEntity definition, IReadOnlyList<WorkflowStepEntity> steps, CancellationToken cancellationToken);
    }

    internal sealed class CreateWorkflowDefinitionCommand(IWorkflowDbContext dbContext) : ICreateWorkflowDefinition
    {
        public async Task AddAsync(WorkflowDefinitionEntity definition, IReadOnlyList<WorkflowStepEntity> steps, CancellationToken cancellationToken)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.WorkflowDefinitions.AddAsync(definition, cancellationToken);
            await dbContext.WorkflowSteps.AddRangeAsync(steps, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }

    public sealed class Handler(ICreateWorkflowDefinition command, IBusinessNumberGenerator numberGenerator, ITenantScope tenantScope)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue) return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            var code = await numberGenerator.NextAsync("WorkflowDefinition", "WF", tenantId, 4, cancellationToken);
            var entity = WorkflowDefinitionEntity.Create(tenantId.Value, code, request.Name, request.Description, request.TriggerType, request.EntityType, request.Status);
            var steps = request.Steps.Select((x, i) => WorkflowStepEntity.Create(tenantId.Value, entity.WorkflowDefinitionId, $"{code}-S{i + 1:00}", x.Name, i + 1, x.StepType, x.ApproverRole, x.ActionCode, x.IsRequired)).ToList();
            await command.AddAsync(entity, steps, cancellationToken);
            return Result<Response>.Success(new Response(entity.TenantId, entity.WorkflowDefinitionId, entity.Code, entity.Name, entity.Version));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "workflow-definition"), async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
            (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithName("CreateWorkflowDefinition").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.WorkflowAdministration);
        return endpoints;
    }
}
