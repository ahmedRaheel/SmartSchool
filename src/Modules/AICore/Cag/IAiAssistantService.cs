namespace SmartSchool.Modules.AICore.Cag;

public interface IAiAssistantService
{
    Task<AiAssistantResponse> AskAsync(AiAssistantRequest request, CancellationToken cancellationToken);
    Task InvalidateKnowledgeAsync(Guid tenantId, string collection, CancellationToken cancellationToken);
}
