using System.Text;
using System.Text.Json;

namespace SmartSchool.ApiSmokeTester;

internal static class ReportWriter
{
    public static async Task<string> WriteAsync(
        IReadOnlyList<SmokeResult> results,
        SmokeTestOptions options,
        SmokeTestFixture? fixture,
        CancellationToken cancellationToken)
    {
        var timestamp = DateTimeOffset.UtcNow.ToString("yyyyMMdd-HHmmss");
        var reportDirectory = Path.GetFullPath(
            Path.Combine(options.ReportDirectory, timestamp));
        Directory.CreateDirectory(reportDirectory);

        await WriteJsonAsync(results, reportDirectory, cancellationToken);
        await WriteCsvAsync(results, reportDirectory, cancellationToken);
        await WriteMarkdownAsync(
            results,
            options,
            fixture,
            reportDirectory,
            cancellationToken);

        if (fixture is not null)
        {
            await WriteFixtureSummaryAsync(
                fixture,
                reportDirectory,
                cancellationToken);
        }

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
            "Result,Actor,Method,Path,StatusCode,DurationMilliseconds,OperationId,Message,RequestUri,ResponseBody");

        foreach (var result in results)
        {
            builder.AppendLine(
                string.Join(
                    ',',
                    Csv(result.Result.ToString()),
                    Csv(result.Actor),
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
        SmokeTestFixture? fixture,
        string directory,
        CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# SmartSchool API Smoke-Test Report");
        builder.AppendLine();
        builder.AppendLine($"- Base URL: `{options.BaseUrl}`");
        builder.AppendLine($"- Identity URL: `{options.IdentityBaseUrl}`");
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

        if (fixture is not null)
        {
            builder.AppendLine($"- Fixture run: `{fixture.RunId}`");
            builder.AppendLine($"- Fixture tenant: `{fixture.TenantId}`");
            builder.AppendLine($"- Disposable actors: **{fixture.Actors.Count}**");
            builder.AppendLine($"- Fixture provisioning warnings: **{fixture.ProvisioningWarnings.Count}**");
        }

        builder.AppendLine();

        if (fixture is not null && fixture.ProvisioningWarnings.Count > 0)
        {
            builder.AppendLine("## Fixture provisioning warnings");
            builder.AppendLine();
            foreach (var warning in fixture.ProvisioningWarnings)
            {
                builder.AppendLine($"- {warning}");
            }
            builder.AppendLine();
        }

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
                    + $"actor `{result.Actor}`, "
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
        builder.AppendLine("| Result | Actor | Method | Endpoint | HTTP | ms |");
        builder.AppendLine("|---|---|---|---|---:|---:|");

        foreach (var result in results)
        {
            builder.AppendLine(
                $"| {result.Result} | {result.Actor} | {result.Method} | "
                + $"`{EscapeMarkdown(result.Path)}` | "
                + $"{result.StatusCode?.ToString() ?? string.Empty} | "
                + $"{result.DurationMilliseconds} |");
        }

        await File.WriteAllTextAsync(
            Path.Combine(directory, "api-smoke-report.md"),
            builder.ToString(),
            cancellationToken);
    }

    private static async Task WriteFixtureSummaryAsync(
        SmokeTestFixture fixture,
        string directory,
        CancellationToken cancellationToken)
    {
        var safe = new
        {
            fixture.RunId,
            fixture.TenantId,
            fixture.SchoolId,
            fixture.CampusId,
            fixture.AcademicSystemId,
            fixture.AcademicYearId,
            fixture.GradeLevelId,
            fixture.ClassSectionId,
            ProvisioningWarnings = fixture.ProvisioningWarnings.ToArray(),
            Actors = fixture.Actors.Values
                .Select(
                    actor => new
                    {
                        actor.Kind,
                        actor.UserId,
                        actor.BusinessEntityId,
                        actor.TenantId,
                        actor.SchoolId,
                        actor.BranchId
                    })
                .OrderBy(actor => actor.Kind)
                .ToArray()
        };

        await File.WriteAllTextAsync(
            Path.Combine(directory, "fixture-summary.json"),
            JsonSerializer.Serialize(
                safe,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }),
            cancellationToken);
    }

    private static int Count(
        IReadOnlyList<SmokeResult> results,
        SmokeResultKind resultKind) =>
        results.Count(result => result.Result == resultKind);

    private static string Csv(string value) =>
        '"' + value.Replace("\"", "\"\"", StringComparison.Ordinal) + '"';

    private static string EscapeMarkdown(string value) =>
        value.Replace("|", "\\|", StringComparison.Ordinal);

    private static string Truncate(
        string value,
        int maximumLength) =>
        value.Length <= maximumLength
            ? value
            : value[..maximumLength] + "... [truncated]";
}
