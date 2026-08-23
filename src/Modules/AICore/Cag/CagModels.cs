namespace SmartSchool.Modules.AICore.Cag;

/// <summary>Describes an authorized request to the shared AI assistant.</summary>
public sealed record AiAssistantRequest(Guid TenantId, Guid? UserId, Guid? SchoolId, string Assistant, string Question, IReadOnlyCollection<string> Collections, string SystemPrompt);

/// <summary>Identifies a knowledge item used to ground an AI response.</summary>
public sealed record AiCitation(Guid Id, string DocumentName, string Collection, double? Score);

/// <summary>Represents the grounded response returned by the AI assistant.</summary>
public sealed record AiAssistantResponse(string Assistant, string Answer, string Model, string ContextStrategy, IReadOnlyCollection<AiCitation> Citations);

internal sealed record CachedContext(string Context, IReadOnlyCollection<AiCitation> Citations, bool FitsContextWindow, DateTimeOffset CreatedAtUtc);
