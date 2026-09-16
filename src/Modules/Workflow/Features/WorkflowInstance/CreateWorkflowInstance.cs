using SmartSchool.SharedKernel.Constants;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.SharedKernel;
namespace SmartSchool.Modules.Workflow.Features.WorkflowInstance;
public static class CreateWorkflowInstance
{
 public sealed record Response(Guid TenantId,Guid Id,string Code,string Status,int CurrentStepOrder);
 public sealed record Request(Guid? TenantId,Guid WorkflowDefinitionId,string Name,Guid? EntityId,string? ContextJson):IRequest<Result<Response>>;
 public sealed class Validator:AbstractValidator<Request>{public Validator(){RuleFor(x=>x.WorkflowDefinitionId).NotEmpty();RuleFor(x=>x.Name).NotEmpty().MaximumLength(250);}}
 public interface ICreateWorkflowInstance{Task<WorkflowDefinitionEntity?> GetDefinitionAsync(Guid tenantId,Guid id,CancellationToken cancellationToken);Task<List<WorkflowStepEntity>> GetStepsAsync(Guid tenantId,Guid id,CancellationToken cancellationToken);Task AddAsync(WorkflowInstanceEntity instance,ApprovalEntity? approval,CancellationToken cancellationToken);}
 internal sealed class Command(IWorkflowDbContext db):ICreateWorkflowInstance
 {public Task<WorkflowDefinitionEntity?> GetDefinitionAsync(Guid tenantId,Guid id,CancellationToken cancellationToken)=>db.WorkflowDefinitions.SingleOrDefaultAsync(x=>x.TenantId==tenantId&&x.WorkflowDefinitionId==id&&x.IsActive&&x.Status=="ACTIVE",cancellationToken);public Task<List<WorkflowStepEntity>> GetStepsAsync(Guid tenantId,Guid id,CancellationToken cancellationToken)=>db.WorkflowSteps.Where(x=>x.TenantId==tenantId&&x.WorkflowDefinitionId==id&&x.IsActive).OrderBy(x=>x.StepOrder).ToListAsync(cancellationToken);public async Task AddAsync(WorkflowInstanceEntity instance,ApprovalEntity? approval,CancellationToken cancellationToken){await using var tx=await db.Database.BeginTransactionAsync(cancellationToken);await db.WorkflowInstances.AddAsync(instance,cancellationToken);if(approval is not null)await db.Approvals.AddAsync(approval,cancellationToken);await db.SaveChangesAsync(cancellationToken);await tx.CommitAsync(cancellationToken);}}
 public sealed class Handler(ICreateWorkflowInstance command,IBusinessNumberGenerator numberGenerator,ITenantScope tenantScope,ICurrentUser currentUser):IRequestHandler<Request,Result<Response>>
 {public async Task<Result<Response>> HandleAsync(Request request,CancellationToken cancellationToken){var tenantId=tenantScope.Resolve(request.TenantId);if(!tenantId.HasValue)return Result<Response>.Failure(Error.Validation("Tenant context is required."));var def=await command.GetDefinitionAsync(tenantId.Value,request.WorkflowDefinitionId,cancellationToken);if(def is null)return Result<Response>.Failure(Error.NotFound("Active workflow definition was not found."));var steps=await command.GetStepsAsync(tenantId.Value,def.WorkflowDefinitionId,cancellationToken);if(steps.Count==0)return Result<Response>.Failure(Error.Validation("Workflow definition has no active steps."));var code=await numberGenerator.NextAsync("WorkflowInstance","WFI",tenantId,6,cancellationToken);var instance=WorkflowInstanceEntity.Create(tenantId.Value,def.WorkflowDefinitionId,code,request.Name,def.EntityType,request.EntityId,currentUser.UserId,request.ContextJson);var next=steps.FirstOrDefault(x=>x.StepType.Equals("APPROVAL",StringComparison.OrdinalIgnoreCase));ApprovalEntity? approval=null;if(next is null){instance.Complete();}else{instance.MoveToStep(next.StepOrder);var approvalCode=await numberGenerator.NextAsync("WorkflowApproval","WFA",tenantId,7,cancellationToken);approval=ApprovalEntity.Create(tenantId.Value,instance.WorkflowInstanceId,next.WorkflowStepId,approvalCode,$"{instance.Name}: {next.Name}",next.ApproverRole!);}await command.AddAsync(instance,approval,cancellationToken);return Result<Response>.Success(new Response(instance.TenantId,instance.WorkflowInstanceId,instance.Code,instance.Status,instance.CurrentStepOrder));}}
 public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints){endpoints.MapPost(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment,"workflow-instance"),async(Request body,IMediator mediator,CancellationToken cancellationToken)=>(await mediator.SendAsync<Request,Result<Response>>(body,cancellationToken)).ToHttpResult()).WithName("CreateWorkflowInstance").WithTags(ModuleConstants.Name).RequireAuthorization();return endpoints;}
}
