using Microsoft.AspNetCore.Mvc;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Cag;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Api.Features;

/// <summary>
/// Maps actor-specific chatbot endpoints onto the shared CAG-first AI pipeline.
/// </summary>
public static class RagChatbotEndpoints
{
    private static readonly IReadOnlyDictionary<string, BotDefinition> Bots =
        new Dictionary<string, BotDefinition>(StringComparer.OrdinalIgnoreCase)
        {
            ["student"] = new("student", [SmartSchoolRoles.Student, SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin], ["learning", "academic", "policy"], "You are a study assistant. Teach clearly and use only authorized school knowledge."),
            ["teacher"] = new("teacher", [SmartSchoolRoles.Teacher, SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin], ["learning", "academic", "teacher", "policy"], "Assist teachers with learning material, class operations and approved school policy."),
            ["parent"] = new("parent", [SmartSchoolRoles.Parent, SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin], ["parent", "policy", "fees", "academic"], "Assist parents without exposing information about unrelated students."),
            ["admissions"] = new("admissions", [SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.AdmissionOfficer], ["admissions", "fees", "policy"], "Answer admissions questions only from approved school knowledge."),
            ["admin"] = new("admin", [SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Principal], ["operations", "policy", "academic", "fees", "hr"], "Assist administrators using authorized operational and policy knowledge.")
        };

    /// <summary>Maps the actor chatbot API endpoints.</summary>
    public static IEndpointRouteBuilder MapRagChatbotEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/chatbots/{bot}/ask", AskAsync)
            .WithTags("AI - Chatbots")
            .RequireAuthorization();
        return endpoints;
    }

    private static async Task<IResult> AskAsync(
        string bot,
        AskRequest request,
        [FromServices] ICurrentUser currentUser,
        [FromServices] ITenantScope tenantScope,
        [FromServices] IAiAssistantService assistantService,
        [FromServices] IIntegrationEventPublisher eventPublisher,
        CancellationToken cancellationToken)
    {
        if (!Bots.TryGetValue(bot, out var definition))
        {
            return Results.NotFound(new { message = "Unknown chatbot." });
        }

        if (!definition.Roles.Any(currentUser.IsInRole))
        {
            return Results.Forbid();
        }

        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return Results.BadRequest(new { message = "Question is required." });
        }

        var tenantId = currentUser.IsSuperAdmin ? request.TenantId : tenantScope.Resolve(request.TenantId);

        if (!tenantId.HasValue)
        {
            return Results.BadRequest(new { message = "SuperAdmin must select a tenant." });
        }

        var response = await assistantService.AskAsync(
            new AiAssistantRequest(
                tenantId.Value,
                tenantScope.UserId,
                currentUser.IsSuperAdmin ? request.SchoolId : currentUser.SchoolId,
                definition.Name,
                request.Question.Trim(),
                definition.Collections,
                definition.SystemPrompt),
            cancellationToken);

        await eventPublisher.PublishAsync(
            KafkaTopics.ChatbotQuestionAsked,
            new ChatbotQuestionAskedEvent(tenantId.Value, tenantScope.UserId, definition.Name, response.ContextStrategy, response.Citations.Count),
            cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>Represents a chatbot question scoped to a tenant and optional school.</summary>
    public sealed record AskRequest(Guid? TenantId, string Question, Guid? SchoolId = null);

    private sealed record BotDefinition(string Name, string[] Roles, string[] Collections, string SystemPrompt);
    private sealed record ChatbotQuestionAskedEvent(Guid TenantId, Guid UserId, string Bot, string ContextStrategy, int CitationCount);
}
