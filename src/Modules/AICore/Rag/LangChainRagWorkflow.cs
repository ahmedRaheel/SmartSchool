using System.Diagnostics;

namespace SmartSchool.Modules.AICore.Rag;

/// <summary>
/// Defines the deterministic workflow used by SmartSchool RAG assistants.
/// LangChain is kept at the orchestration boundary while tenant-aware retrieval remains
/// in SmartSchool so authorization cannot be bypassed by an LLM or tool call.
/// </summary>
internal sealed class LangChainRagWorkflow
{
    private readonly IReadOnlyList<IRagWorkflowStep> _steps;

    public LangChainRagWorkflow(IEnumerable<IRagWorkflowStep> steps)
    {
        _steps = steps.OrderBy(step => step.Order).ToArray();
    }

    public async Task<RagWorkflowContext> ExecuteAsync(
        RagWorkflowContext context,
        CancellationToken cancellationToken)
    {
        foreach (var step in _steps)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var started = Stopwatch.GetTimestamp();
            context = await step.ExecuteAsync(context, cancellationToken);
            context = context.WithTiming(step.Name, Stopwatch.GetElapsedTime(started));

            if (context.IsRejected)
            {
                break;
            }
        }

        return context;
    }
}

internal interface IRagWorkflowStep
{
    int Order { get; }

    string Name { get; }

    Task<RagWorkflowContext> ExecuteAsync(
        RagWorkflowContext context,
        CancellationToken cancellationToken);
}

internal sealed record RagWorkflowContext(
    Guid TenantId,
    Guid? SchoolId,
    Guid? UserId,
    string Assistant,
    string Question,
    IReadOnlyCollection<string> Collections,
    string SystemPrompt,
    string? Context = null,
    string? Answer = null,
    string? Model = null,
    bool IsRejected = false,
    IReadOnlyCollection<string>? Citations = null,
    IReadOnlyDictionary<string, TimeSpan>? Timings = null)
{
    public RagWorkflowContext WithTiming(string step, TimeSpan elapsed)
    {
        var timings = Timings is null
            ? new Dictionary<string, TimeSpan>()
            : new Dictionary<string, TimeSpan>(Timings);

        timings[step] = elapsed;
        return this with { Timings = timings };
    }
}
