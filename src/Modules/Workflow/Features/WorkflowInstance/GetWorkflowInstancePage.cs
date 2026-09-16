using SmartSchool.SharedKernel.Constants;
using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
namespace SmartSchool.Modules.Workflow.Features.WorkflowInstance;
public static class GetWorkflowInstancePage
{
 public sealed record Response(Guid TenantId,Guid Id,Guid WorkflowDefinitionId,string DefinitionName,string Code,string Name,string EntityType,Guid? EntityId,string Status,int CurrentStepOrder,DateTimeOffset StartedAt,DateTimeOffset? CompletedAt,string? ContextJson);
 public sealed record Query(Guid? TenantId,int Page=1,int PageSize=25):IRequest<Result<PagedResult<Response>>>;
 public interface IGetWorkflowInstancePage{Task<PagedResult<Response>> ExecuteAsync(Guid tenantId,int page,int pageSize,CancellationToken cancellationToken);}
 internal sealed class QueryService(IDbConnectionFactory factory):IGetWorkflowInstancePage{public async Task<PagedResult<Response>> ExecuteAsync(Guid tenantId,int page,int pageSize,CancellationToken cancellationToken){const string countSql="SELECT COUNT(*) FROM workflow.workflowinstance WHERE tenant_id=@TenantId AND is_active=TRUE";const string sql="""SELECT i.tenant_id AS "TenantId",i.workflow_instance_id AS "Id",i.workflow_definition_id AS "WorkflowDefinitionId",d.name AS "DefinitionName",i.code AS "Code",i.name AS "Name",i.entity_type AS "EntityType",i.entity_id AS "EntityId",i.status AS "Status",i.current_step_order AS "CurrentStepOrder",i.started_at AS "StartedAt",i.completed_at AS "CompletedAt",i.context_json::text AS "ContextJson" FROM workflow.workflowinstance i JOIN workflow.workflowdefinition d ON d.workflow_definition_id=i.workflow_definition_id AND d.tenant_id=i.tenant_id WHERE i.tenant_id=@TenantId AND i.is_active=TRUE ORDER BY i.started_at DESC LIMIT @PageSize OFFSET @Offset;""";await using var c=await factory.OpenConnectionAsync(cancellationToken);var p=new{TenantId=tenantId,PageSize=pageSize,Offset=(page-1)*pageSize};var total=await c.ExecuteScalarAsync<long>(new CommandDefinition(countSql,p,cancellationToken:cancellationToken));var items=(await c.QueryAsync<Response>(new CommandDefinition(sql,p,cancellationToken:cancellationToken))).AsList();return new PagedResult<Response>(items,page,pageSize,total);}}
 public sealed class Handler(IGetWorkflowInstancePage query,ITenantScope tenantScope):IRequestHandler<Query,Result<PagedResult<Response>>>{public async Task<Result<PagedResult<Response>>> HandleAsync(Query request,CancellationToken cancellationToken){var tenant=tenantScope.Resolve(request.TenantId);if(!tenant.HasValue)return Result<PagedResult<Response>>.Failure(Error.Validation("Tenant context is required."));var page=new PageRequest(request.Page,request.PageSize);return Result<PagedResult<Response>>.Success(await query.ExecuteAsync(tenant.Value,page.NormalizedPage,page.NormalizedPageSize,cancellationToken));}}
 public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints){endpoints.MapGet(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment,"workflow-instance"),async(Guid? tenantId,int page,int pageSize,IMediator mediator,CancellationToken cancellationToken)=>(await mediator.SendAsync<Query,Result<PagedResult<Response>>>(new Query(tenantId,page,pageSize),cancellationToken)).ToHttpResult()).WithName("GetWorkflowInstancePage").WithTags(ModuleConstants.Name).RequireAuthorization();return endpoints;}
}
