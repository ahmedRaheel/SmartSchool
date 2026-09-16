using SmartSchool.SharedKernel.Constants;
using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Features.WorkflowDefinition;

public static class GetWorkflowDefinitionPage
{
    public sealed record StepResponse(Guid Id, string Code, string Name, int StepOrder, string StepType, string? ApproverRole, string? ActionCode, bool IsRequired);
    public sealed record Response(Guid TenantId, Guid Id, string Code, string Name, string? Description, string TriggerType, string EntityType, string Status, int Version, IReadOnlyList<StepResponse> Steps);
    public sealed record Query(Guid? TenantId, int Page = 1, int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;
    private sealed record DefinitionRow(Guid TenantId, Guid Id, string Code, string Name, string? Description, string TriggerType, string EntityType, string Status, int Version);
    private sealed record StepRow(Guid WorkflowDefinitionId, Guid Id, string Code, string Name, int StepOrder, string StepType, string? ApproverRole, string? ActionCode, bool IsRequired);

    public interface IGetWorkflowDefinitionPage { Task<PagedResult<Response>> ExecuteAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken); }
    internal sealed class QueryService(IDbConnectionFactory connectionFactory) : IGetWorkflowDefinitionPage
    {
        public async Task<PagedResult<Response>> ExecuteAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
        {
            const string countSql = "SELECT COUNT(*) FROM workflow.workflowdefinition WHERE tenant_id=@TenantId AND is_active=TRUE";
            const string defsSql = """
                SELECT tenant_id AS "TenantId", workflow_definition_id AS "Id", code AS "Code", name AS "Name", description AS "Description", trigger_type AS "TriggerType", entity_type AS "EntityType", status AS "Status", version AS "Version"
                FROM workflow.workflowdefinition WHERE tenant_id=@TenantId AND is_active=TRUE
                ORDER BY name LIMIT @PageSize OFFSET @Offset;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var parameters = new { TenantId = tenantId, PageSize = pageSize, Offset = (page - 1) * pageSize };
            var total = await connection.ExecuteScalarAsync<long>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
            var defs = (await connection.QueryAsync<DefinitionRow>(new CommandDefinition(defsSql, parameters, cancellationToken: cancellationToken))).AsList();
            if (defs.Count == 0) return new PagedResult<Response>([], page, pageSize, total);
            const string stepsSql = """
                SELECT workflow_definition_id AS "WorkflowDefinitionId", workflow_step_id AS "Id", code AS "Code", name AS "Name", step_order AS "StepOrder", step_type AS "StepType", approver_role AS "ApproverRole", action_code AS "ActionCode", is_required AS "IsRequired"
                FROM workflow.workflowstep WHERE tenant_id=@TenantId AND is_active=TRUE AND workflow_definition_id = ANY(@Ids) ORDER BY step_order;
                """;
            var rows = (await connection.QueryAsync<StepRow>(new CommandDefinition(stepsSql, new { TenantId = tenantId, Ids = defs.Select(x => x.Id).ToArray() }, cancellationToken: cancellationToken))).AsList();
            var lookup = rows.GroupBy(x => x.WorkflowDefinitionId).ToDictionary(g => g.Key, g => (IReadOnlyList<StepResponse>)g.Select(x => new StepResponse(x.Id,x.Code,x.Name,x.StepOrder,x.StepType,x.ApproverRole,x.ActionCode,x.IsRequired)).ToList());
            var items = defs.Select(x => new Response(x.TenantId,x.Id,x.Code,x.Name,x.Description,x.TriggerType,x.EntityType,x.Status,x.Version,lookup.GetValueOrDefault(x.Id, []))).ToList();
            return new PagedResult<Response>(items, page, pageSize, total);
        }
    }
    public sealed class Handler(IGetWorkflowDefinitionPage query, ITenantScope tenantScope) : IRequestHandler<Query, Result<PagedResult<Response>>>
    {
        public async Task<Result<PagedResult<Response>>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var tenantId=tenantScope.Resolve(request.TenantId); if(!tenantId.HasValue) return Result<PagedResult<Response>>.Failure(Error.Validation("Tenant context is required."));
            var page=new PageRequest(request.Page,request.PageSize);
            return Result<PagedResult<Response>>.Success(await query.ExecuteAsync(tenantId.Value,page.NormalizedPage,page.NormalizedPageSize,cancellationToken));
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment,"workflow-definition"), async(Guid? tenantId,int page,int pageSize,IMediator mediator,CancellationToken cancellationToken)=>(await mediator.SendAsync<Query,Result<PagedResult<Response>>>(new Query(tenantId,page,pageSize),cancellationToken)).ToHttpResult())
            .WithName("GetWorkflowDefinitionPage").WithTags(ModuleConstants.Name).RequireAuthorization(); return endpoints;
    }
}
