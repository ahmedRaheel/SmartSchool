using SmartSchool.Application.Identity;
using SmartSchool.Modules.AICore.Cag;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Api.Features;

/// <summary>Provides the general-purpose authorized SmartSchool AI assistant endpoint.</summary>
public static class AiAssistantEndpoints
{
    /// <summary>Maps the general AI assistant endpoint.</summary>
    public static IEndpointRouteBuilder MapAiAssistantEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/ai/assistant/ask", AskAsync)
            .WithTags("AI - Assistant")
            .RequireAuthorization(SmartSchoolPolicies.AllAuthenticatedActors);
        return endpoints;
    }

    private static async Task<IResult> AskAsync(
        AskRequest request,
        ITenantScope tenantScope,
        IAiAssistantService assistantService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return Results.BadRequest(new { message = "Question is required." });
        var tenantId = tenantScope.IsSuperAdmin ? request.TenantId : tenantScope.Resolve(request.TenantId);
        if (!tenantId.HasValue)
            return Results.BadRequest(new { message = "SuperAdmin must select a tenant." });

        var collections = request.Collections is { Count: > 0 }
            ? request.Collections
            : ["operations", "policy", "academic"];

        var result = await assistantService.AskAsync(
            new AiAssistantRequest(tenantId.Value, tenantScope.UserId, request.SchoolId, request.Assistant ?? "general", request.Question.Trim(), collections,
                "You are the SmartSchool assistant. Use only authorized school context and state clearly when verified context is insufficient."),
            cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>Represents a general AI assistant question.</summary>
    public sealed record AskRequest(Guid? TenantId, string Question, Guid? SchoolId, string? Assistant, IReadOnlyCollection<string>? Collections);
}
