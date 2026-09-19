using System.Text.Json;

namespace SmartSchool.ApiSmokeTester;

internal sealed class SmokeTestOptions
{
    public string BaseUrl { get; set; } = "http://localhost:61342";

    public string OpenApiPath { get; set; } = "/openapi/v1.json";

    public string? BearerToken { get; set; }

    public string BearerTokenEnvironmentVariable { get; set; } = "SMARTSCHOOL_API_TOKEN";

    public bool IncludeMutatingEndpoints { get; set; } = true;

    public bool IncludeDeleteEndpoints { get; set; } = true;

    public bool Strict { get; set; }

    public bool IgnoreTlsErrors { get; set; }

    public int TimeoutSeconds { get; set; } = 30;

    public string ReportDirectory { get; set; } = "artifacts/api-smoke";

    public Dictionary<string, string> KnownValues { get; set; } =
        new(StringComparer.OrdinalIgnoreCase);

    public static SmokeTestOptions Parse(string[] args)
    {
        var options = new SmokeTestOptions();
        string? settingsPath = null;

        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];

            switch (argument)
            {
                case "--settings":
                    settingsPath = RequireValue(args, ref index, argument);
                    break;
                case "--base-url":
                    options.BaseUrl = RequireValue(args, ref index, argument);
                    break;
                case "--openapi":
                    options.OpenApiPath = RequireValue(args, ref index, argument);
                    break;
                case "--read-only":
                    options.IncludeMutatingEndpoints = false;
                    options.IncludeDeleteEndpoints = false;
                    break;
                case "--all":
                    options.IncludeMutatingEndpoints = true;
                    options.IncludeDeleteEndpoints = true;
                    break;
                case "--no-delete":
                    options.IncludeDeleteEndpoints = false;
                    break;
                case "--strict":
                    options.Strict = true;
                    break;
                case "--ignore-tls-errors":
                    options.IgnoreTlsErrors = true;
                    break;
                case "--timeout":
                    options.TimeoutSeconds = int.Parse(
                        RequireValue(args, ref index, argument),
                        System.Globalization.CultureInfo.InvariantCulture);
                    break;
                case "--report-dir":
                    options.ReportDirectory = RequireValue(args, ref index, argument);
                    break;
                case "--token":
                    options.BearerToken = RequireValue(args, ref index, argument);
                    break;
                default:
                    throw new ArgumentException($"Unknown argument: {argument}");
            }
        }

        if (!string.IsNullOrWhiteSpace(settingsPath))
        {
            options.ApplySettingsFile(settingsPath);

            // Command-line options should win over settings file.
            options.ApplyCommandLineOverrides(args);
        }

        if (string.IsNullOrWhiteSpace(options.BearerToken))
        {
            options.BearerToken = Environment.GetEnvironmentVariable(
                options.BearerTokenEnvironmentVariable);
        }

        options.ApplyBearerTokenScopeValues();

        options.BaseUrl = options.BaseUrl.TrimEnd('/');

        if (!options.OpenApiPath.StartsWith("/", StringComparison.Ordinal))
        {
            options.OpenApiPath = "/" + options.OpenApiPath;
        }

        return options;
    }

    private void ApplyBearerTokenScopeValues()
    {
        if (string.IsNullOrWhiteSpace(BearerToken)) return;
        var token = BearerToken.Trim();
        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token["Bearer ".Length..].Trim();

        var parts = token.Split('.');
        if (parts.Length < 2) return;

        try
        {
            var payload = parts[1].Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2:
                    payload += "==";
                    break;
                case 3:
                    payload += "=";
                    break;
                default:
                    break;
            }

            using var document = JsonDocument.Parse(
                System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload)));

            var claims = document.RootElement;
            ApplyClaim(claims, "tenantId", "tenantId", "tenant_id", "tenant");
            ApplyClaim(claims, "schoolId", "schoolId", "school_id", "school");
            ApplyClaim(claims, "branchId", "branchId", "branch_id", "branch");
            ApplyClaim(claims, "campusId", "campusId", "campus_id");
            ApplyClaim(claims, "userId", "userId", "user_id", "sub");
        }
        catch
        {
            // Opaque or malformed tokens are allowed; the API will classify auth failures.
        }
    }

    private void ApplyClaim(JsonElement claims, string knownValueName, params string[] claimNames)
    {
        foreach (var claimName in claimNames)
        {
            if (!claims.TryGetProperty(claimName, out var claim)) continue;
            var value = claim.ValueKind switch
            {
                JsonValueKind.String => claim.GetString(),
                JsonValueKind.Number => claim.GetRawText(),
                _ => null
            };

            if (!string.IsNullOrWhiteSpace(value))
            {
                KnownValues[knownValueName] = value;
                return;
            }
        }
    }

    private void ApplySettingsFile(string settingsPath)
    {
        if (!File.Exists(settingsPath))
        {
            throw new FileNotFoundException(
                "Smoke-test settings file was not found.",
                settingsPath);
        }

        var json = File.ReadAllText(settingsPath);
        var fromFile = JsonSerializer.Deserialize<SmokeTestOptions>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (fromFile is null)
        {
            return;
        }

        BaseUrl = fromFile.BaseUrl;
        OpenApiPath = fromFile.OpenApiPath;
        BearerToken = fromFile.BearerToken;
        BearerTokenEnvironmentVariable = fromFile.BearerTokenEnvironmentVariable;
        IncludeMutatingEndpoints = fromFile.IncludeMutatingEndpoints;
        IncludeDeleteEndpoints = fromFile.IncludeDeleteEndpoints;
        Strict = fromFile.Strict;
        IgnoreTlsErrors = fromFile.IgnoreTlsErrors;
        TimeoutSeconds = fromFile.TimeoutSeconds;
        ReportDirectory = fromFile.ReportDirectory;
        KnownValues = new Dictionary<string, string>(
            fromFile.KnownValues,
            StringComparer.OrdinalIgnoreCase);
    }

    private void ApplyCommandLineOverrides(string[] args)
    {
        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];

            switch (argument)
            {
                case "--settings":
                    index++;
                    break;
                case "--base-url":
                    BaseUrl = RequireValue(args, ref index, argument);
                    break;
                case "--openapi":
                    OpenApiPath = RequireValue(args, ref index, argument);
                    break;
                case "--read-only":
                    IncludeMutatingEndpoints = false;
                    IncludeDeleteEndpoints = false;
                    break;
                case "--all":
                    IncludeMutatingEndpoints = true;
                    IncludeDeleteEndpoints = true;
                    break;
                case "--no-delete":
                    IncludeDeleteEndpoints = false;
                    break;
                case "--strict":
                    Strict = true;
                    break;
                case "--ignore-tls-errors":
                    IgnoreTlsErrors = true;
                    break;
                case "--timeout":
                    TimeoutSeconds = int.Parse(
                        RequireValue(args, ref index, argument),
                        System.Globalization.CultureInfo.InvariantCulture);
                    break;
                case "--report-dir":
                    ReportDirectory = RequireValue(args, ref index, argument);
                    break;
                case "--token":
                    BearerToken = RequireValue(args, ref index, argument);
                    break;
                default:
                    break;
            }
        }
    }

    private static string RequireValue(
        string[] args,
        ref int index,
        string argument)
    {
        if (index + 1 >= args.Length)
        {
            throw new ArgumentException($"Missing value for {argument}.");
        }

        index++;
        return args[index];
    }
}
