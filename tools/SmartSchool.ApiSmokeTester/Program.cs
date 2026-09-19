using SmartSchool.ApiSmokeTester;

SmokeTestFixture? fixture = null;
SmokeTestFixtureManager? fixtureManager = null;

try
{
    var options = SmokeTestOptions.Parse(args);

    Console.WriteLine("SmartSchool API endpoint smoke tester");
    Console.WriteLine($"Base URL: {options.BaseUrl}");
    Console.WriteLine($"Identity URL: {options.IdentityBaseUrl}");
    Console.WriteLine($"OpenAPI: {options.OpenApiPath}");
    Console.WriteLine(
        $"Mode: {(options.IncludeMutatingEndpoints ? "all operations" : "read-only")}");
    Console.WriteLine(
        $"Fixture: {(options.SelfContainedFixture ? "self-contained" : "disabled")}");
    Console.WriteLine();

    var handler = new HttpClientHandler();

    if (options.IgnoreTlsErrors)
    {
        handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    }

    using var httpClient = new HttpClient(handler)
    {
        Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds + 10)
    };

    if (options.SelfContainedFixture)
    {
        fixtureManager = new SmokeTestFixtureManager(
            httpClient,
            options);
        fixture = await fixtureManager.SetupAsync(
            CancellationToken.None);
    }
    else if (string.IsNullOrWhiteSpace(options.BearerToken))
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(
            "WARNING: fixture mode is disabled and no bearer token is configured.");
        Console.ResetColor();
        Console.WriteLine();
    }

    var openApiUri = options.BaseUrl + options.OpenApiPath;
    Console.WriteLine($"Discovering endpoints from {openApiUri} ...");

    var openApiJson = await httpClient.GetStringAsync(openApiUri);
    var openApi = OpenApiDocumentModel.Parse(openApiJson);

    Console.WriteLine($"Discovered {openApi.Operations.Count} API operations.");
    Console.WriteLine();

    var runner = new SmokeTestRunner(
        httpClient,
        options,
        openApi,
        fixture);

    var results = await runner.RunAsync(
        CancellationToken.None);

    var reportDirectory = await ReportWriter.WriteAsync(
        results,
        options,
        fixture,
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

    var shouldFail =
        serverFailures > 0
        || timeouts > 0
        || networkErrors > 0;

    if (options.Strict)
    {
        shouldFail =
            shouldFail
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
finally
{
    if (fixtureManager is not null)
    {
        try
        {
            await fixtureManager.CleanupAsync(
                fixture,
                CancellationToken.None);
        }
        catch (Exception cleanupException)
        {
            Console.Error.WriteLine(
                $"Smoke fixture cleanup failed: {cleanupException}");
            Environment.ExitCode = 2;
        }
    }
}
