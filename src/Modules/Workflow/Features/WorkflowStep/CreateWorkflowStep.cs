using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.WorkflowStep;

public static class CreateWorkflowStep
{
    /// <summary>
    /// Represents the response returned by this WorkflowStepEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    Guid WorkflowDefinitionId,
    string Name,
    int StepOrder,
    string StepType,
    string? ApproverRole,
    string? ActionCode,
    bool IsRequired);

    public sealed record Request(
        Guid TenantId,
        Guid WorkflowDefinitionId,
        string Name,      
        int StepOrder,
        string StepType,
        string? ApproverRole,
        string? ActionCode,
        bool IsRequired) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public interface ICreateWorkflowStepCommand
    {
        Task AddAsync(
                WorkflowStepEntity entity,
                CancellationToken cancellationToken);
}

    internal sealed class CreateWorkflowStepCommand(IWorkflowDbContext dbContext) : ICreateWorkflowStepCommand
    {
        public async Task AddAsync(
                WorkflowStepEntity entity,
                CancellationToken cancellationToken)
            {
                await dbContext.WorkflowSteps
                    .AddAsync(entity, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
    }

    public sealed class Handler(ICreateWorkflowStepCommand command, IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var code = await numberGenerator.NextAsync("WorkflowStep", "WS", request.TenantId, 3, cancellationToken);

            var entity = WorkflowStepEntity.Create(
                request.TenantId,
                request.WorkflowDefinitionId,
                code,
                request.Name, 
                request.StepOrder,
                request.StepType,
                request.ApproverRole,
                request.ActionCode,
                request.IsRequired);

            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "workflow-step"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateWorkflowStep")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(WorkflowStepEntity entity)
    {
       return new Response(
            entity.TenantId,
            entity.WorkflowStepId,
            entity.Code,
            entity.WorkflowDefinitionId,
            entity.Name,
            entity.StepOrder,
            entity.StepType,
            entity.ApproverRole,
            entity.ActionCode,
            entity.IsRequired);
    }
}
