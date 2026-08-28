using System.Diagnostics;
using System.Text;
using System.Text.Json;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.Application.Identity;
using SmartSchool.Modules.AICore.Cag;

namespace SmartSchool.Modules.AICore.Agents;

public sealed record AgentRunRequest(
    string Agent,
    string Message,
    Guid? StudentId = null);

public sealed record AgentRunResponse(
    string Agent,
    string Answer,
    string Model,
    IReadOnlyCollection<string> ToolsUsed);

public interface IAgentWorkflowService
{
    Task<AgentRunResponse> RunAsync(AgentRunRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Runs bounded SmartSchool agent workflows. Data acquisition is performed through MCP tool classes,
/// which in turn use existing module query abstractions. Ollama is used only for reasoning over that context.
/// </summary>
internal sealed class AgentWorkflowService(
    ICurrentUser currentUser,
    SmartSchoolAgentTools tools,
    IOllamaClient ollamaClient,
    ITenantScope tenantScope,
    IAiExecutionLogCommand executionLogCommand) : IAgentWorkflowService
{
    public async Task<AgentRunResponse> RunAsync(
        AgentRunRequest request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Authentication is required.");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(request.Agent);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Message);

        var tenantId = tenantScope.Resolve()
            ?? throw new UnauthorizedAccessException("A tenant context is required.");
        var startedAt = Stopwatch.GetTimestamp();
        var studentId = ResolveStudentId(request.StudentId);
        var toolsUsed = new List<string>();
        var context = new StringBuilder();

        if (studentId.HasValue)
        {
            var profile = await tools.GetStudentProfileAsync(studentId.Value, cancellationToken);
            toolsUsed.Add("get_student_profile");
            context.AppendLine("STUDENT PROFILE:");
            context.AppendLine(profile);

            var results = await tools.GetStudentExamResultsAsync(studentId.Value, 50, cancellationToken);
            toolsUsed.Add("get_student_exam_results");
            context.AppendLine("EXAM RESULTS:");
            context.AppendLine(results);

            var predictions = await tools.GetStudentPredictionsAsync(studentId.Value, 50, cancellationToken);
            toolsUsed.Add("get_student_predictions");
            context.AppendLine("PREDICTIONS:");
            context.AppendLine(predictions);
        }

        var prompt = BuildPrompt(request, context.ToString());
        var (answer, model) = await ollamaClient.GenerateAsync(prompt, cancellationToken);
        var elapsed = Stopwatch.GetElapsedTime(startedAt);

        var executionId = Guid.NewGuid();
        var metadata = JsonSerializer.Serialize(new
        {
            request.Agent,
            request.StudentId,
            Model = model,
            ToolsUsed = toolsUsed,
            LatencyMs = (long)elapsed.TotalMilliseconds,
            userId = currentUser.UserId
        });

        var executionLog = AiExecutionLogEntity.Create(
            tenantId,
            $"AGENT-{executionId:N}",
            $"{request.Agent.Trim()} agent execution",
            metadata);

        await executionLogCommand.AddAsync(executionLog, cancellationToken);

        return new AgentRunResponse(
            request.Agent.Trim(),
            answer,
            model,
            toolsUsed);
    }

    private Guid? ResolveStudentId(Guid? requestedStudentId)
    {
        if (currentUser.StudentId.HasValue)
        {
            if (requestedStudentId.HasValue && requestedStudentId.Value != currentUser.StudentId.Value)
            {
                throw new UnauthorizedAccessException("Students can only run agents against their own student context.");
            }

            return currentUser.StudentId.Value;
        }

        return requestedStudentId;
    }

    private static string BuildPrompt(AgentRunRequest request, string context)
    {
        return $$"""
            You are the SmartSchool {{request.Agent.Trim()}} agent.
            Use only the supplied SmartSchool tool context for factual school data.
            Do not invent marks, predictions, student details, policies, or identifiers.
            If required information is missing, say what is missing.
            Any consequential action such as admission approval, hiring, striking off a student,
            changing marks, or financial changes requires an authorized human approval workflow.

            TOOL CONTEXT
            {{context}}

            USER REQUEST
            {{request.Message.Trim()}}
            """;
    }
}
