using Microsoft.AspNetCore.Mvc;

namespace SmartSchool.Modules.AICore.Agents;

internal static class AgentEndpoints
{
    public static IEndpointRouteBuilder MapAgentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/ai/agents/run",
                RunAsync)
            .RequireAuthorization()
            .WithTags("AI Agents")
            .WithName("RunSmartSchoolAgent");

        return endpoints;
    }

    private static async Task<IResult> RunAsync(
        AgentRunRequest request,
        [FromServices] IAgentWorkflowService workflowService,
        CancellationToken cancellationToken)
    {
        var response = await workflowService.RunAsync(request, cancellationToken);
        return Results.Ok(response);
    }
}
