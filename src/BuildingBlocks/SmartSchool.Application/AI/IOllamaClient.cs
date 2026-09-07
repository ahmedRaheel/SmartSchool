namespace SmartSchool.Application.AI;

public interface IOllamaClient
{
    Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken);

    Task<float[][]> EmbedBatchAsync(
        IReadOnlyCollection<string> texts,
        CancellationToken cancellationToken);

    Task<(string Answer, string Model)> GenerateAsync(
        string prompt,
        CancellationToken cancellationToken);
}
