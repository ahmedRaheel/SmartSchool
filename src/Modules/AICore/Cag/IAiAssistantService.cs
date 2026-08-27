namespace SmartSchool.Modules.AICore.Cag;

/// <summary>Provides the tenant-scoped CAG-first AI assistant pipeline.</summary>
public interface IAiAssistantService
{
    /// <summary>Answers a question from authorized cached context, falling back to vector retrieval when necessary.</summary>
    Task<AiAssistantResponse> AskAsync(AiAssistantRequest request, CancellationToken cancellationToken);

    /// <summary>Invalidates the cached knowledge version for a tenant collection.</summary>
    Task InvalidateKnowledgeAsync(Guid tenantId, string collection, CancellationToken cancellationToken);
}
