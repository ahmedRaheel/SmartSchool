using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using SmartSchool.Modules.AICore.Rag.Ollama;

namespace SmartSchool.Modules.AICore.Cag;

public interface IOllamaClient
{
    Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken);
    Task<float[][]> EmbedBatchAsync(IReadOnlyCollection<string> texts, CancellationToken cancellationToken);
    Task<(string Answer, string Model)> GenerateAsync(string prompt, CancellationToken cancellationToken);
}

internal sealed class OllamaClient(
    IHttpClientFactory httpClientFactory,
    IOptionsMonitor<OllamaRagOptions> options) : IOllamaClient
{
    private sealed record EmbeddingResponse(float[][] Embeddings);
    private sealed record GenerateResponse(string Response);

    public async Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        var embeddings = await EmbedBatchAsync([text], cancellationToken).ConfigureAwait(false);
        return embeddings[0];
    }

    public async Task<float[][]> EmbedBatchAsync(
        IReadOnlyCollection<string> texts,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(texts);
        if (texts.Count == 0)
        {
            return [];
        }

        var current = options.CurrentValue;
        var client = httpClientFactory.CreateClient("Ollama");
        using var response = await client.PostAsJsonAsync(
            "api/embed",
            new { model = current.EmbeddingModel, input = texts },
            cancellationToken).ConfigureAwait(false);

        await EnsureSuccessAsync(response, current.EmbeddingModel, "embedding", cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<EmbeddingResponse>(
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (result?.Embeddings is not { Length: > 0 } embeddings || embeddings.Length != texts.Count)
        {
            throw new InvalidOperationException(
                $"Ollama returned {result?.Embeddings?.Length ?? 0} embeddings for {texts.Count} inputs using model '{current.EmbeddingModel}'.");
        }

        return embeddings;
    }

    public async Task<(string Answer, string Model)> GenerateAsync(
        string prompt,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);
        var current = options.CurrentValue;
        var client = httpClientFactory.CreateClient("Ollama");
        using var response = await client.PostAsJsonAsync(
            "api/generate",
            new
            {
                model = current.ChatModel,
                prompt = "/no_think\n" + prompt,
                stream = false,
                keep_alive = "30m",
                options = new { temperature = 0.1, num_predict = 256 }
            },
            cancellationToken).ConfigureAwait(false);

        await EnsureSuccessAsync(response, current.ChatModel, "generation", cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<GenerateResponse>(
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return (result?.Response ?? string.Empty, current.ChatModel);
    }

    private static async Task EnsureSuccessAsync(
        HttpResponseMessage response,
        string model,
        string operation,
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        throw new InvalidOperationException(
            $"Ollama {operation} failed for model '{model}'. HTTP {(int)response.StatusCode} ({response.StatusCode}). Response: {body}");
    }
}
