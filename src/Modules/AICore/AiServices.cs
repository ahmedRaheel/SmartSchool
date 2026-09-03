using System.Threading.Tasks;
namespace SmartSchool.Modules.AICore;

public sealed record AiRequest(
    Guid TenantId,
    Guid? UserId,
    string Assistant,
    string MessageEntity);

public sealed record AiResponse(
    string Answer,
    IReadOnlyList<string> Citations,
    int InputTokens,
    int OutputTokens);

public interface IAiGateway
{
    Task<AiResponse> CompleteAsync(
        AiRequest request,
        CancellationToken cancellationToken);
}

public interface IRagRetriever
{
    Task<IReadOnlyList<string>> RetrieveAsync(
        Guid tenantId,
        string assistant,
        string query,
        CancellationToken cancellationToken);
}

public interface IAiToolExecutor
{
    Task<object?> ExecuteAsync(
        Guid tenantId,
        Guid? userId,
        string tool,
        object arguments,
        CancellationToken cancellationToken);
}
