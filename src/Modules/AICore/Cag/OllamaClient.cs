using System.Net.Http.Json;

namespace SmartSchool.Modules.AICore.Cag;

internal interface IOllamaClient
{
    /// <summary>Creates an embedding for the supplied text.</summary>
    Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken);
    /// <summary>Generates a model response for the supplied prompt.</summary>
    Task<(string Answer, string Model)> GenerateAsync(string prompt, CancellationToken cancellationToken);
}

internal sealed class OllamaClient(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IOllamaClient
{
    private sealed record EmbeddingResponse(float[][] Embeddings);
    private sealed record GenerateResponse(string Response);

    /// <summary>Creates an embedding using the configured Ollama embedding model.</summary>
    public async Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("api/embed", new
        {
            model = configuration["AI:Ollama:EmbeddingModel"] ?? "nomic-embed-text",
            input = text
        }, cancellationToken);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<EmbeddingResponse>(cancellationToken: cancellationToken);
        return result?.Embeddings is { Length: > 0 } && result.Embeddings[0].Length > 0
            ? result.Embeddings[0]
            : throw new InvalidOperationException("Ollama returned an empty embedding.");
    }

    /// <summary>Generates a response using the configured Ollama chat model.</summary>
    public async Task<(string Answer, string Model)> GenerateAsync(string prompt, CancellationToken cancellationToken)
    {
        var model = configuration["AI:Ollama:ChatModel"] ?? "llama3.2";
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("api/generate", new { model, prompt, stream = false }, cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GenerateResponse>(cancellationToken: cancellationToken);
        return (result?.Response ?? string.Empty, model);
    }

    private HttpClient CreateClient()
    {
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri((configuration["AI:Ollama:BaseUrl"] ?? throw new InvalidOperationException("AI:Ollama:BaseUrl configuration is required.")).TrimEnd('/') + "/");
        return client;
    }
}
