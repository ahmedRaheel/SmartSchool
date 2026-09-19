using System.Text;
using System.Text.Json;

namespace SmartSchool.ApiSmokeTester;

internal static class ReportWriter
{
    public static async Task<string> WriteAsync(
        IReadOnlyList<SmokeResult> results,
        SmokeTestOptions options,
        CancellationToken cancellationToken)
    {
        var timestamp = DateTimeOffset.UtcNow.ToString("yyyyMMdd-HHmmss");
        var reportDirectory = Path.GetFullPath(
            Path.Combine(options.ReportDirectory, timestamp));
        Directory.CreateDirectory(reportDirectory);

        await WriteJsonAsync(results, reportDirectory, cancellationToken);
        await WriteCsvAsync(results, reportDirectory, cancellationToken);
        await WriteMarkdownAsync(results, options, reportDirectory, cancellationToken);

        return reportDirectory;
    }

    private static async Task WriteJsonAsync(
        IReadOnlyList<SmokeResult> results,
        string directory,
        CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(
            results,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        await File.WriteAllTextAsync(
            Path.Combine(directory, "api-smoke-report.json"),
            json,
            cancellationToken);
    }

    private static async Task WriteCsvAsync(
        IReadOnlyList<SmokeResult> results,
        string directory,
        CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();
        builder.AppendLine(
            "Result,Method,Path,StatusCode,DurationMilliseconds,OperationId,Message,RequestUri,ResponseBody");

        foreach (var result in results)
        {
            builder.AppendLine(
                string.Join(
                    ',',
                    Csv(result.Result.ToString()),
                    Csv(result.Method),
                    Csv(result.Path),
                    Csv(result.StatusCode?.ToString() ?? string.Empty),
                    Csv(result.DurationMilliseconds.ToString()),
                    Csv(result.OperationId),
                    Csv(result.Message),
                    Csv(result.RequestUri),
                    Csv(result.ResponseBody)));
        }

        await File.WriteAllTextAsync(
            Path.Combine(directory, "api-smoke-report.csv"),
            builder.ToString(),
            cancellationToken);
    }

    private static async Task WriteMarkdownAsync(
        IReadOnlyList<SmokeResult> results,
        SmokeTestOptions options,
        string directory,
        CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# SmartSchool API Smoke-Test Report");
        builder.AppendLine();
        builder.AppendLine($"- Base URL: `{options.BaseUrl}`");
        builder.AppendLine($"- Endpoints discovered: **{results.Count}**");
        builder.AppendLine($"- Passed: **{Count(results, SmokeResultKind.Passed)}**");
        builder.AppendLine(
            $"- Validation handled (4xx): **{Count(results, SmokeResultKind.ValidationHandled)}**");
        builder.AppendLine(
            $"- Authentication blocked: **{Count(results, SmokeResultKind.AuthenticationBlocked)}**");
        builder.AppendLine(
            $"- Server failures: **{Count(results, SmokeResultKind.FailedServerError)}**");
        builder.AppendLine($"- Timeouts: **{Count(results, SmokeResultKind.Timeout)}**");
        builder.AppendLine($"- Network errors: **{Count(results, SmokeResultKind.NetworkError)}**");
        builder.AppendLine($"- Skipped: **{Count(results, SmokeResultKind.Skipped)}**");
        builder.AppendLine();

        var failures = results
            .Where(result => result.Result is
                SmokeResultKind.FailedServerError
                or SmokeResultKind.Timeout
                or SmokeResultKind.NetworkError)
            .ToArray();

        if (failures.Length > 0)
        {
            builder.AppendLine("## Failing endpoints");
            builder.AppendLine();

            foreach (var result in failures)
            {
                builder.AppendLine(
                    $"- **{result.Method} {result.Path}** — {result.Result}, "
                    + $"HTTP {result.StatusCode?.ToString() ?? "n/a"}, "
                    + $"{result.DurationMilliseconds} ms");

                if (!string.IsNullOrWhiteSpace(result.ResponseBody))
                {
                    builder.AppendLine();
                    builder.AppendLine("  ```text");
                    builder.AppendLine(
                        "  " + Truncate(result.ResponseBody, 1200)
                            .Replace("\n", "\n  ", StringComparison.Ordinal));
                    builder.AppendLine("  ```");
                }
            }

            builder.AppendLine();
        }

        builder.AppendLine("## All endpoints");
        builder.AppendLine();
        builder.AppendLine("| Result | Method | Endpoint | HTTP | ms |");
        builder.AppendLine("|---|---|---|---:|---:|");

        foreach (var result in results)
        {
            builder.AppendLine(
                $"| {result.Result} | {result.Method} | `{EscapeMarkdown(result.Path)}` | "
                + $"{result.StatusCode?.ToString() ?? string.Empty} | "
                + $"{result.DurationMilliseconds} |");
        }

        await File.WriteAllTextAsync(
            Path.Combine(directory, "api-smoke-report.md"),
            builder.ToString(),
            cancellationToken);
    }

    private static int Count(
        IReadOnlyList<SmokeResult> results,
        SmokeResultKind resultKind)
    {
        return results.Count(result => result.Result == resultKind);
    }

    private static string Csv(string value)
    {
        return '"' + value.Replace("\"", "\"\"", StringComparison.Ordinal) + '"';
    }

    private static string EscapeMarkdown(string value)
    {
        return value.Replace("|", "\\|", StringComparison.Ordinal);
    }

    private static string Truncate(string value, int maximumLength)
    {
        return value.Length <= maximumLength
            ? value
            : value[..maximumLength] + "... [truncated]";
    }
}
