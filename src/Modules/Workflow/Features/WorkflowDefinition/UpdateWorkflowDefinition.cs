using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.WorkflowDefinition;

public static class UpdateWorkflowDefinition
{
    public sealed record StepRequest(string Name,string StepType,string? ApproverRole,string? ActionCode,bool IsRequired=true);
    public sealed record Request(Guid Id,Guid? TenantId,string Name,string? Description,string TriggerType,string EntityType,string Status,IReadOnlyList<StepRequest> Steps):IRequest<Result>;
    public sealed class Validator:AbstractValidator<Request>{public Validator(){RuleFor(x=>x.Id).NotEmpty();RuleFor(x=>x.Name).NotEmpty().MaximumLength(250);RuleFor(x=>x.Steps).NotEmpty();}}
    public interface IUpdateWorkflowDefinition{Task<WorkflowDefinitionEntity?> GetAsync(Guid tenantId,Guid id,CancellationToken cancellationToken);Task<bool> HasRunningInstancesAsync(Guid tenantId,Guid id,CancellationToken cancellationToken);Task ReplaceStepsAsync(WorkflowDefinitionEntity entity,IReadOnlyList<WorkflowStepEntity> steps,CancellationToken cancellationToken);}
    internal sealed class Command(IWorkflowDbContext db):IUpdateWorkflowDefinition
    {
        public Task<WorkflowDefinitionEntity?> GetAsync(Guid tenantId,Guid id,CancellationToken cancellationToken)=>db.WorkflowDefinitions.SingleOrDefaultAsync(x=>x.TenantId==tenantId&&x.WorkflowDefinitionId==id&&x.IsActive,cancellationToken);
        public Task<bool> HasRunningInstancesAsync(Guid tenantId,Guid id,CancellationToken cancellationToken)=>db.WorkflowInstances.AnyAsync(x=>x.TenantId==tenantId&&x.WorkflowDefinitionId==id&&x.IsActive&&x.Status=="IN_PROGRESS",cancellationToken);
        public async Task ReplaceStepsAsync(WorkflowDefinitionEntity entity,IReadOnlyList<WorkflowStepEntity> steps,CancellationToken cancellationToken){await using var tx=await db.Database.BeginTransactionAsync(cancellationToken);var old=await db.WorkflowSteps.Where(x=>x.TenantId==entity.TenantId&&x.WorkflowDefinitionId==entity.WorkflowDefinitionId).ToListAsync(cancellationToken);db.WorkflowSteps.RemoveRange(old);await db.WorkflowSteps.AddRangeAsync(steps,cancellationToken);await db.SaveChangesAsync(cancellationToken);await tx.CommitAsync(cancellationToken);}
    }
    public sealed class Handler(IUpdateWorkflowDefinition command,ITenantScope tenantScope):IRequestHandler<Request,Result>
    {
        public async Task<Result> HandleAsync(Request request,CancellationToken cancellationToken){var tenantId=tenantScope.Resolve(request.TenantId);if(!tenantId.HasValue)return Result.Failure(Error.Validation("Tenant context is required."));var entity=await command.GetAsync(tenantId.Value,request.Id,cancellationToken);if(entity is null)return Result.Failure(Error.NotFound("Workflow definition was not found."));if(await command.HasRunningInstancesAsync(tenantId.Value,request.Id,cancellationToken))return Result.Failure(Error.Conflict("A running workflow instance uses this definition. Finish it before editing the definition."));entity.UpdateDetails(request.Name,request.Description,request.TriggerType,request.EntityType,request.Status);var steps=request.Steps.Select((x,i)=>WorkflowStepEntity.Create(tenantId.Value,entity.WorkflowDefinitionId,$"{entity.Code}-S{i+1:00}",x.Name,i+1,x.StepType,x.ApproverRole,x.ActionCode,x.IsRequired)).ToList();await command.ReplaceStepsAsync(entity,steps,cancellationToken);return Result.Success();}
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints){endpoints.MapPut(ApiRoutes.EntityById(ModuleConstants.RouteSegment,"workflow-definition"),async(Guid id,Request body,IMediator mediator,CancellationToken cancellationToken)=>(await mediator.SendAsync<Request,Result>(body with{Id=id},cancellationToken)).ToHttpResult()).WithName("UpdateWorkflowDefinition").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.WorkflowAdministration);return endpoints;}
}
