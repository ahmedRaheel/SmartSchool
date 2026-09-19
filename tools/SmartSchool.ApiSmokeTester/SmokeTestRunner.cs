using System.Diagnostics;

namespace SmartSchool.ApiSmokeTester;

internal sealed class SmokeTestRunner(
    HttpClient httpClient,
    SmokeTestOptions options,
    OpenApiDocumentModel openApi,
    SmokeTestFixture? fixture)
{
    private readonly SmokeRequestFactory _requestFactory = new(options, openApi);

    public async Task<IReadOnlyList<SmokeResult>> RunAsync(
        CancellationToken cancellationToken)
    {
        var results = new List<SmokeResult>();

        var operations = openApi.Operations
            .OrderBy(OperationPhase)
            .ThenBy(operation => operation.Path, StringComparer.OrdinalIgnoreCase)
            .ThenBy(operation => operation.Method, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        foreach (var operation in operations)
        {
            if (ShouldSkip(operation, out var skipMessage))
            {
                results.Add(
                    new SmokeResult(
                        operation.Method,
                        operation.Path,
                        operation.Path,
                        operation.OperationId,
                        string.Empty,
                        SmokeResultKind.Skipped,
                        null,
                        0,
                        skipMessage,
                        string.Empty));
                continue;
            }

            var result = fixture is null
                ? await ExecuteOnceAsync(
                    operation,
                    actor: null,
                    options.KnownValues,
                    cancellationToken)
                : await ExecuteWithActorsAsync(
                    operation,
                    fixture,
                    cancellationToken);

            results.Add(result);
            PrintResult(result);
        }

        return results;
    }

    private async Task<SmokeResult> ExecuteWithActorsAsync(
        OpenApiOperation operation,
        SmokeTestFixture smokeFixture,
        CancellationToken cancellationToken)
    {
        SmokeResult? lastAuthenticationResult = null;

        foreach (var actorKind in SmokeActorResolver.GetCandidates(operation))
        {
            if (!smokeFixture.Actors.TryGetValue(actorKind, out var actor))
            {
                continue;
            }

            var result = await ExecuteOnceAsync(
                operation,
                actor,
                smokeFixture.ValuesFor(actor),
                cancellationToken);

            if (result.Result != SmokeResultKind.AuthenticationBlocked)
            {
                return result;
            }

            lastAuthenticationResult = result;
        }

        return lastAuthenticationResult
               ?? new SmokeResult(
                   operation.Method,
                   operation.Path,
                   operation.Path,
                   operation.OperationId,
                   string.Empty,
                   SmokeResultKind.AuthenticationBlocked,
                   403,
                   0,
                   "No disposable smoke-test actor was authorized for this endpoint.",
                   string.Empty);
    }

    private async Task<SmokeResult> ExecuteOnceAsync(
        OpenApiOperation operation,
        SmokeActor? actor,
        IReadOnlyDictionary<string, string> knownValues,
        CancellationToken cancellationToken)
    {
        using var request =
            _requestFactory.Create(operation, actor, knownValues);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            using var timeoutSource =
                CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken);
            timeoutSource.CancelAfter(
                TimeSpan.FromSeconds(options.TimeoutSeconds));

            using var response = await httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseContentRead,
                timeoutSource.Token);

            stopwatch.Stop();

            var responseBody =
                await response.Content.ReadAsStringAsync(
                    timeoutSource.Token);
            var statusCode = (int)response.StatusCode;
            var resultKind = Classify(statusCode);

            return new SmokeResult(
                operation.Method,
                operation.Path,
                request.RequestUri?.ToString() ?? operation.Path,
                operation.OperationId,
                actor?.Kind.ToString() ?? "ConfiguredToken",
                resultKind,
                statusCode,
                stopwatch.ElapsedMilliseconds,
                GetMessage(resultKind, statusCode),
                Truncate(responseBody, 4000));
        }
        catch (OperationCanceledException)
            when (!cancellationToken.IsCancellationRequested)
        {
            stopwatch.Stop();

            return new SmokeResult(
                operation.Method,
                operation.Path,
                request.RequestUri?.ToString() ?? operation.Path,
                operation.OperationId,
                actor?.Kind.ToString() ?? "ConfiguredToken",
                SmokeResultKind.Timeout,
                null,
                stopwatch.ElapsedMilliseconds,
                $"Timed out after {options.TimeoutSeconds} seconds.",
                string.Empty);
        }
        catch (HttpRequestException exception)
        {
            stopwatch.Stop();

            return new SmokeResult(
                operation.Method,
                operation.Path,
                request.RequestUri?.ToString() ?? operation.Path,
                operation.OperationId,
                actor?.Kind.ToString() ?? "ConfiguredToken",
                SmokeResultKind.NetworkError,
                null,
                stopwatch.ElapsedMilliseconds,
                exception.Message,
                string.Empty);
        }
        catch (Exception exception)
        {
            stopwatch.Stop();

            return new SmokeResult(
                operation.Method,
                operation.Path,
                request.RequestUri?.ToString() ?? operation.Path,
                operation.OperationId,
                actor?.Kind.ToString() ?? "ConfiguredToken",
                SmokeResultKind.NetworkError,
                null,
                stopwatch.ElapsedMilliseconds,
                exception.ToString(),
                string.Empty);
        }
    }

    private static int OperationPhase(OpenApiOperation operation) =>
        operation.Method switch
        {
            "GET" or "HEAD" or "OPTIONS" => 0,
            "POST" => 1,
            "PUT" or "PATCH" => 2,
            "DELETE" => 3,
            _ => 2
        };

    private bool ShouldSkip(
        OpenApiOperation operation,
        out string message)
    {
        var mutating =
            operation.Method is "POST" or "PUT" or "PATCH" or "DELETE";

        if (mutating && !options.IncludeMutatingEndpoints)
        {
            message = "Mutating endpoint disabled by read-only mode.";
            return true;
        }

        if (operation.Method == "DELETE"
            && !options.IncludeDeleteEndpoints)
        {
            message = "DELETE endpoint disabled.";
            return true;
        }

        if (operation.Path.StartsWith(
                "/api/testing/",
                StringComparison.OrdinalIgnoreCase))
        {
            message = "Test-fixture lifecycle endpoint is excluded from the smoke matrix.";
            return true;
        }

        message = string.Empty;
        return false;
    }

    private static SmokeResultKind Classify(int statusCode)
    {
        if (statusCode >= 500)
        {
            return SmokeResultKind.FailedServerError;
        }

        if (statusCode is 401 or 403)
        {
            return SmokeResultKind.AuthenticationBlocked;
        }

        if (statusCode >= 400)
        {
            return SmokeResultKind.ValidationHandled;
        }

        return SmokeResultKind.Passed;
    }

    private static string GetMessage(
        SmokeResultKind resultKind,
        int statusCode) =>
        resultKind switch
        {
            SmokeResultKind.Passed =>
                "Endpoint completed without a client/server error.",
            SmokeResultKind.AuthenticationBlocked =>
                "Endpoint was reached but authentication/authorization blocked execution.",
            SmokeResultKind.ValidationHandled =>
                "Endpoint handled the generated smoke-test data without a server error.",
            SmokeResultKind.FailedServerError =>
                $"Endpoint returned server error HTTP {statusCode}.",
            _ => string.Empty
        };

    private static string Truncate(
        string value,
        int maximumLength) =>
        value.Length <= maximumLength
            ? value
            : value[..maximumLength] + "... [truncated]";

    private static void PrintResult(SmokeResult result)
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = result.Result switch
        {
            SmokeResultKind.Passed => ConsoleColor.Green,
            SmokeResultKind.ValidationHandled => ConsoleColor.DarkYellow,
            SmokeResultKind.AuthenticationBlocked => ConsoleColor.Yellow,
            SmokeResultKind.FailedServerError => ConsoleColor.Red,
            SmokeResultKind.NetworkError => ConsoleColor.Red,
            SmokeResultKind.Timeout => ConsoleColor.Red,
            _ => ConsoleColor.DarkGray
        };

        Console.WriteLine(
            $"[{result.Result,-21}] {result.Method,-6} {result.Path} "
            + $"{(result.StatusCode.HasValue ? $"HTTP {result.StatusCode}" : string.Empty)} "
            + $"{result.DurationMilliseconds} ms "
            + $"actor={result.Actor}");

        Console.ForegroundColor = previousColor;
    }
}
