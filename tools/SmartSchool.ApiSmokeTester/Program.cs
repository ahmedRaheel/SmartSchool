using System.Net.Http.Headers;
using SmartSchool.ApiSmokeTester;

try
{
    var options = SmokeTestOptions.Parse(args);

    Console.WriteLine("SmartSchool API endpoint smoke tester");
    Console.WriteLine($"Base URL: {options.BaseUrl}");
    Console.WriteLine($"OpenAPI: {options.OpenApiPath}");
    Console.WriteLine(
        $"Mode: {(options.IncludeMutatingEndpoints ? "all operations" : "read-only")}");
    Console.WriteLine("Authentication: SMARTSCHOOL_API_TOKEN / --token");
    Console.WriteLine();

    if (string.IsNullOrWhiteSpace(options.BearerToken))
    {
        throw new InvalidOperationException(
            "SMARTSCHOOL_API_TOKEN is required. Set a valid SmartSchool access token before running the smoke test.");
    }

    var handler = new HttpClientHandler();

    if (options.IgnoreTlsErrors)
    {
        handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    }

    using var httpClient = new HttpClient(handler)
    {
        Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds + 5)
    };

    await ValidateConfiguredTokenAsync(httpClient, options);

    var openApiUri = options.BaseUrl + options.OpenApiPath;
    Console.WriteLine($"Discovering endpoints from {openApiUri} ...");

    var openApiJson = await httpClient.GetStringAsync(openApiUri);
    var openApi = OpenApiDocumentModel.Parse(openApiJson);

    Console.WriteLine($"Discovered {openApi.Operations.Count} API operations.");
    Console.WriteLine();

    var runner = new SmokeTestRunner(httpClient, options, openApi);
    var results = await runner.RunAsync(CancellationToken.None);
    var reportDirectory = await ReportWriter.WriteAsync(
        results,
        options,
        CancellationToken.None);

    var serverFailures = results.Count(
        result => result.Result == SmokeResultKind.FailedServerError);
    var timeouts = results.Count(
        result => result.Result == SmokeResultKind.Timeout);
    var networkErrors = results.Count(
        result => result.Result == SmokeResultKind.NetworkError);
    var passed = results.Count(
        result => result.Result == SmokeResultKind.Passed);
    var handled = results.Count(
        result => result.Result == SmokeResultKind.ValidationHandled);
    var authenticationBlocked = results.Count(
        result => result.Result == SmokeResultKind.AuthenticationBlocked);
    var skipped = results.Count(
        result => result.Result == SmokeResultKind.Skipped);

    Console.WriteLine();
    Console.WriteLine("============================================================");
    Console.WriteLine("SmartSchool API smoke-test summary");
    Console.WriteLine("============================================================");
    Console.WriteLine($"Discovered              : {results.Count}");
    Console.WriteLine($"Passed                  : {passed}");
    Console.WriteLine($"Handled 4xx             : {handled}");
    Console.WriteLine($"Authentication blocked  : {authenticationBlocked}");
    Console.WriteLine($"Server failures (5xx)   : {serverFailures}");
    Console.WriteLine($"Timeouts                : {timeouts}");
    Console.WriteLine($"Network errors          : {networkErrors}");
    Console.WriteLine($"Skipped                 : {skipped}");
    Console.WriteLine($"Reports                 : {reportDirectory}");

    var shouldFail = serverFailures > 0
                     || timeouts > 0
                     || networkErrors > 0;

    if (options.Strict)
    {
        shouldFail = shouldFail
                     || handled > 0
                     || authenticationBlocked > 0
                     || skipped > 0;
    }

    Environment.ExitCode = shouldFail ? 1 : 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception);
    Environment.ExitCode = 2;
}

static async Task ValidateConfiguredTokenAsync(
    HttpClient httpClient,
    SmokeTestOptions options)
{
    using var request = new HttpRequestMessage(
        HttpMethod.Get,
        options.BaseUrl + "/api/testing/auth-probe");
    request.Headers.Authorization = new AuthenticationHeaderValue(
        "Bearer",
        options.BearerToken!);
    request.Headers.TryAddWithoutValidation("X-Smoke-Test", "true");

    using var response = await httpClient.SendAsync(request);

    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(
            "Authentication probe endpoint is unavailable; continuing with the configured token.");
        Console.ResetColor();
        Console.WriteLine();
        return;
    }

    var body = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        throw new InvalidOperationException(
            $"SMARTSCHOOL_API_TOKEN failed API authentication preflight. "
            + $"HTTP {(int)response.StatusCode}. {body}");
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Configured bearer token passed API authentication preflight.");
    Console.ResetColor();

    if (options.KnownValues.Count > 0)
    {
        Console.WriteLine(
            "Token-derived IDs: "
            + string.Join(
                ", ",
                options.KnownValues.Keys
                    .OrderBy(value => value, StringComparer.OrdinalIgnoreCase)));
    }

    Console.WriteLine();
}
