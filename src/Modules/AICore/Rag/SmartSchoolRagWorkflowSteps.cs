namespace SmartSchool.Modules.AICore.Rag;

/// <summary>
/// Fast deterministic pre-flight guardrail. Expensive model calls are never made for an
/// invalid tenant or empty question.
/// </summary>
internal sealed class RagGuardrailStep : IRagWorkflowStep
{
    public int Order => 10;

    public string Name => "guardrail";

    public Task<RagWorkflowContext> ExecuteAsync(
        RagWorkflowContext context,
        CancellationToken cancellationToken)
    {
        var rejected = context.TenantId == Guid.Empty ||
            string.IsNullOrWhiteSpace(context.Question) ||
            context.Collections.Count == 0;

        return Task.FromResult(rejected
            ? context with
            {
                IsRejected = true,
                Answer = "The request does not contain enough authorized information to run the assistant."
            }
            : context);
    }
}

/// <summary>
/// Adds the Qwen no-thinking directive. For a grounded school RAG answer we want low
/// latency and deterministic retrieval, not a long reasoning trace.
/// </summary>
internal sealed class FastModelPromptStep : IRagWorkflowStep
{
    public int Order => 20;

    public string Name => "fast-model-prompt";

    public Task<RagWorkflowContext> ExecuteAsync(
        RagWorkflowContext context,
        CancellationToken cancellationToken)
    {
        var prompt = $"""
            /no_think
            {context.SystemPrompt}

            Answer only from retrieved SmartSchool knowledge.
            If the evidence is insufficient, say so.
            Never use data from another tenant.
            Keep the answer concise and cite supporting chunks.
            """;

        return Task.FromResult(context with { SystemPrompt = prompt });
    }
}
