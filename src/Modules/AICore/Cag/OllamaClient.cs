using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using SmartSchool.Application.AI;
using SmartSchool.Modules.AICore.Rag.Ollama;

namespace SmartSchool.Modules.AICore.Cag;

internal sealed class OllamaClient(
    IHttpClientFactory httpClientFactory,
    IOptionsMonitor<OllamaRagOptions> options) : IOllamaClient
{
    private sealed record EmbeddingResponse(float[][] Embeddings);
    private sealed record GenerateResponse(string Response);
    private sealed record TagsResponse(OllamaModel[] Models);
    private sealed record OllamaModel(string Name);

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

        var model = current.ChatModel;
        using var firstResponse = await PostGenerateAsync(client, model, prompt, cancellationToken)
            .ConfigureAwait(false);

        if (firstResponse.IsSuccessStatusCode)
        {
            var firstResult = await firstResponse.Content.ReadFromJsonAsync<GenerateResponse>(
                cancellationToken: cancellationToken).ConfigureAwait(false);
            return (firstResult?.Response ?? string.Empty, model);
        }

        if (firstResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var fallbackModel = await ResolveFallbackChatModelAsync(
                client,
                current.ChatModel,
                cancellationToken).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(fallbackModel) &&
                !string.Equals(fallbackModel, current.ChatModel, StringComparison.OrdinalIgnoreCase))
            {
                using var retryResponse = await PostGenerateAsync(
                    client,
                    fallbackModel,
                    prompt,
                    cancellationToken).ConfigureAwait(false);

                await EnsureSuccessAsync(retryResponse, fallbackModel, "generation", cancellationToken)
                    .ConfigureAwait(false);

                var retryResult = await retryResponse.Content.ReadFromJsonAsync<GenerateResponse>(
                    cancellationToken: cancellationToken).ConfigureAwait(false);
                return (retryResult?.Response ?? string.Empty, fallbackModel);
            }
        }

        await EnsureSuccessAsync(firstResponse, current.ChatModel, "generation", cancellationToken)
            .ConfigureAwait(false);

        return (string.Empty, current.ChatModel);
    }

    private static Task<HttpResponseMessage> PostGenerateAsync(
        HttpClient client,
        string model,
        string prompt,
        CancellationToken cancellationToken) =>
        client.PostAsJsonAsync(
            "api/generate",
            new
            {
                model,
                prompt = "/no_think\n" + prompt,
                stream = false,
                keep_alive = "30m",
                options = new { temperature = 0.1, num_predict = 256 }
            },
            cancellationToken);

    private static async Task<string?> ResolveFallbackChatModelAsync(
        HttpClient client,
        string configuredModel,
        CancellationToken cancellationToken)
    {
        try
        {
            var tags = await client.GetFromJsonAsync<TagsResponse>(
                "api/tags",
                cancellationToken).ConfigureAwait(false);

            var installed = (tags?.Models ?? [])
                .Select(model => model.Name)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (installed.Length == 0)
            {
                return null;
            }

            string[] preferred =
            [
                "qwen3:8b",
                "qwen3:4b",
                "gemma3:1b",
                "llama3.2:3b",
                "llama3.2:1b"
            ];

            foreach (var candidate in preferred)
            {
                var match = installed.FirstOrDefault(name =>
                    string.Equals(name, candidate, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrWhiteSpace(match) &&
                    !string.Equals(match, configuredModel, StringComparison.OrdinalIgnoreCase))
                {
                    return match;
                }
            }

            return installed.FirstOrDefault(name =>
                !string.Equals(name, configuredModel, StringComparison.OrdinalIgnoreCase) &&
                !name.Contains("embed", StringComparison.OrdinalIgnoreCase) &&
                !name.Contains("minilm", StringComparison.OrdinalIgnoreCase) &&
                !name.Contains("nomic", StringComparison.OrdinalIgnoreCase));
        }
        catch (HttpRequestException)
        {
            return null;
        }
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
        throw new HttpRequestException(
            $"Ollama {operation} failed for model '{model}'. HTTP {(int)response.StatusCode} ({response.StatusCode}). Response: {body}",
            inner: null,
            statusCode: response.StatusCode);
    }
}
