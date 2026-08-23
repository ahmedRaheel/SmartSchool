namespace SmartSchool.Modules.AICore.Cag;

public sealed class AiAssistantOptions
{
    public const string SectionName = "AI:Cag";
    public int ContextCacheMinutes { get; set; } = 30;
    public int RetrievalCacheMinutes { get; set; } = 10;
    public int MaxCachedChunks { get; set; } = 80;
    public int MaxContextCharacters { get; set; } = 48000;
    public int TopK { get; set; } = 5;
}
