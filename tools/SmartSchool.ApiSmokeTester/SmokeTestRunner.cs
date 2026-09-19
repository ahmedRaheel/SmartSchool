using System.Diagnostics;

namespace SmartSchool.ApiSmokeTester;

internal sealed class SmokeTestRunner(
    HttpClient httpClient,
    SmokeTestOptions options,
    OpenApiDocumentModel openApi)
{
    private readonly SmokeRequestFactory _requestFactory = new(options, openApi);

    public async Task<IReadOnlyList<SmokeResult>> RunAsync(
        CancellationToken cancellationToken)
    {
        var results = new List<SmokeResult>();

        foreach (var operation in openApi.Operations)
        {
            if (ShouldSkip(operation, out var skipMessage))
            {
                results.Add(
                    new SmokeResult(
                        operation.Method,
                        operation.Path,
                        operation.Path,
                        operation.OperationId,
                        SmokeResultKind.Skipped,
                        null,
                        0,
                        skipMessage,
                        string.Empty));
                continue;
            }

            using var request = _requestFactory.Create(operation);
            var stopwatch = Stopwatch.StartNew();

            try
            {
                using var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken);
                timeoutSource.CancelAfter(TimeSpan.FromSeconds(options.TimeoutSeconds));

                using var response = await httpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseContentRead,
                    timeoutSource.Token);

                stopwatch.Stop();

                var responseBody = await response.Content.ReadAsStringAsync(
                    timeoutSource.Token);
                var statusCode = (int)response.StatusCode;
                var resultKind = Classify(statusCode);

                results.Add(
                    new SmokeResult(
                        operation.Method,
                        operation.Path,
                        request.RequestUri?.ToString() ?? operation.Path,
                        operation.OperationId,
                        resultKind,
                        statusCode,
                        stopwatch.ElapsedMilliseconds,
                        GetMessage(resultKind, statusCode),
                        Truncate(responseBody, 4000)));
            }
            catch (OperationCanceledException)
                when (!cancellationToken.IsCancellationRequested)
            {
                stopwatch.Stop();

                results.Add(
                    new SmokeResult(
                        operation.Method,
                        operation.Path,
                        request.RequestUri?.ToString() ?? operation.Path,
                        operation.OperationId,
                        SmokeResultKind.Timeout,
                        null,
                        stopwatch.ElapsedMilliseconds,
                        $"Timed out after {options.TimeoutSeconds} seconds.",
                        string.Empty));
            }
            catch (HttpRequestException exception)
            {
                stopwatch.Stop();

                results.Add(
                    new SmokeResult(
                        operation.Method,
                        operation.Path,
                        request.RequestUri?.ToString() ?? operation.Path,
                        operation.OperationId,
                        SmokeResultKind.NetworkError,
                        null,
                        stopwatch.ElapsedMilliseconds,
                        exception.Message,
                        string.Empty));
            }
            catch (Exception exception)
            {
                stopwatch.Stop();

                results.Add(
                    new SmokeResult(
                        operation.Method,
                        operation.Path,
                        request.RequestUri?.ToString() ?? operation.Path,
                        operation.OperationId,
                        SmokeResultKind.NetworkError,
                        null,
                        stopwatch.ElapsedMilliseconds,
                        exception.ToString(),
                        string.Empty));
            }

            PrintResult(results[^1]);
        }

        return results;
    }

    private bool ShouldSkip(
        OpenApiOperation operation,
        out string message)
    {
        var mutating = operation.Method is "POST" or "PUT" or "PATCH" or "DELETE";

        if (mutating && !options.IncludeMutatingEndpoints)
        {
            message = "Mutating endpoint disabled by read-only mode.";
            return true;
        }

        if (operation.Method == "DELETE" && !options.IncludeDeleteEndpoints)
        {
            message = "DELETE endpoint disabled.";
            return true;
        }

        message = string.Empty;
        return false;
    }

    private SmokeResultKind Classify(int statusCode)
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
        int statusCode)
    {
        return resultKind switch
        {
            SmokeResultKind.Passed => "Endpoint completed without a client/server error.",
            SmokeResultKind.AuthenticationBlocked =>
                "Endpoint was reached but authentication/authorization blocked execution.",
            SmokeResultKind.ValidationHandled =>
                "Endpoint handled the generated smoke-test data without a server error.",
            SmokeResultKind.FailedServerError =>
                $"Endpoint returned server error HTTP {statusCode}.",
            _ => string.Empty
        };
    }

    private static string Truncate(string value, int maximumLength)
    {
        return value.Length <= maximumLength
            ? value
            : value[..maximumLength] + "... [truncated]";
    }

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
            + $"{result.DurationMilliseconds} ms");

        Console.ForegroundColor = previousColor;
    }
}
