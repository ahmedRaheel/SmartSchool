using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
namespace SmartSchool.Modules.Workflow.Features.WorkflowDefinition;
public static class DeleteWorkflowDefinition
{
 public sealed record Request(Guid Id,Guid? TenantId):IRequest<Result>;
 public interface IDeleteWorkflowDefinition{Task<WorkflowDefinitionEntity?> GetAsync(Guid tenantId,Guid id,CancellationToken cancellationToken);Task<bool> HasInstancesAsync(Guid tenantId,Guid id,CancellationToken cancellationToken);Task SaveAsync(CancellationToken cancellationToken);}
 internal sealed class Command(IWorkflowDbContext db):IDeleteWorkflowDefinition{public Task<WorkflowDefinitionEntity?> GetAsync(Guid tenantId,Guid id,CancellationToken cancellationToken)=>db.WorkflowDefinitions.SingleOrDefaultAsync(x=>x.TenantId==tenantId&&x.WorkflowDefinitionId==id&&x.IsActive,cancellationToken);public Task<bool> HasInstancesAsync(Guid tenantId,Guid id,CancellationToken cancellationToken)=>db.WorkflowInstances.AnyAsync(x=>x.TenantId==tenantId&&x.WorkflowDefinitionId==id&&x.IsActive,cancellationToken);public Task SaveAsync(CancellationToken cancellationToken)=>db.SaveChangesAsync(cancellationToken);}
 public sealed class Handler(IDeleteWorkflowDefinition command,ITenantScope tenantScope):IRequestHandler<Request,Result>{public async Task<Result> HandleAsync(Request request,CancellationToken cancellationToken){var tenantId=tenantScope.Resolve(request.TenantId);if(!tenantId.HasValue)return Result.Failure(Error.Validation("Tenant context is required."));var entity=await command.GetAsync(tenantId.Value,request.Id,cancellationToken);if(entity is null)return Result.Failure(Error.NotFound("Workflow definition was not found."));if(await command.HasInstancesAsync(tenantId.Value,request.Id,cancellationToken))return Result.Failure(Error.Conflict("Workflow definitions with execution history cannot be deleted. Set the definition to INACTIVE instead."));entity.Deactivate();await command.SaveAsync(cancellationToken);return Result.Success();}}
 public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints){endpoints.MapDelete(ApiRoutes.EntityById(ModuleConstants.RouteSegment,"workflow-definition"),async(Guid id,Guid? tenantId,IMediator mediator,CancellationToken cancellationToken)=>(await mediator.SendAsync<Request,Result>(new Request(id,tenantId),cancellationToken)).ToHttpResult()).WithName("DeleteWorkflowDefinition").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.WorkflowAdministration);return endpoints;}
}
