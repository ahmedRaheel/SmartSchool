namespace SmartSchool.Modules.AICore.Rag.Ollama;

/// <summary>Configuration for the locally hosted Ollama RAG runtime.</summary>
public sealed class OllamaRagOptions
{
    /// <summary>Gets the configuration section name.</summary>
    public const string SectionName = "AI:Ollama";

    /// <summary>Gets or sets the local Ollama endpoint.</summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>Gets or sets the chat model.</summary>
    public string ChatModel { get; set; } = "llama3.2";

    /// <summary>Gets or sets the embedding model.</summary>
    public string EmbeddingModel { get; set; } = "nomic-embed-text";

    /// <summary>Gets or sets the retrieval result count.</summary>
    public int TopK { get; set; } = 5;

    /// <summary>Gets or sets whether citations are mandatory.</summary>
    public bool RequireCitations { get; set; } = true;
}
