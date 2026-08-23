namespace SmartSchool.Modules.AICore.Cag;

public sealed record AiAssistantRequest(
    Guid TenantId,
    Guid? UserId,
    Guid? SchoolId,
    string Assistant,
    string Question,
    IReadOnlyCollection<string> Collections,
    string SystemPrompt);

public sealed record AiCitation(
    Guid Id,
    string DocumentName,
    string Collection,
    double? Score);

public sealed record AiAssistantResponse(
    string Assistant,
    string Answer,
    string Model,
    string ContextStrategy,
    IReadOnlyCollection<AiCitation> Citations);

internal sealed record CachedContext(
    string Context,
    IReadOnlyCollection<AiCitation> Citations,
    bool FitsContextWindow,
    DateTimeOffset CreatedAtUtc);
