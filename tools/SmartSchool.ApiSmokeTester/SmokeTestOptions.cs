using System.Text.Json;

namespace SmartSchool.ApiSmokeTester;

internal sealed class SmokeTestOptions
{
    public string BaseUrl { get; set; } = "http://localhost:7001";
    public string IdentityBaseUrl { get; set; } = "http://localhost:7101";
    public string OpenApiPath { get; set; } = "/openapi/v1.json";
    public string? BearerToken { get; set; }
    public string BearerTokenEnvironmentVariable { get; set; } = "SMARTSCHOOL_API_TOKEN";
    public bool IncludeMutatingEndpoints { get; set; } = true;
    public bool IncludeDeleteEndpoints { get; set; } = true;
    public bool Strict { get; set; }
    public bool IgnoreTlsErrors { get; set; }
    public int TimeoutSeconds { get; set; } = 30;
    public string ReportDirectory { get; set; } = "artifacts/api-smoke";
    public bool SelfContainedFixture { get; set; } = true;
    public bool CleanupFixture { get; set; } = true;
    public string RepositoryRoot { get; set; } = Directory.GetCurrentDirectory();
    public string? BootstrapSuperAdminEmail { get; set; }
    public string? BootstrapSuperAdminPassword { get; set; } = "YourStrongPassword@123";
    public string LoginClientId { get; set; } = "smartschool-login-api";
    public string? LoginClientSecret { get; set; } = "development-login-api-secret-change-me";

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
                case "--identity-url":
                case "--identity-base-url":
                    options.IdentityBaseUrl = RequireValue(args, ref index, argument);
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
                case "--repo-root":
                    options.RepositoryRoot = RequireValue(args, ref index, argument);
                    break;
                case "--no-fixture":
                    options.SelfContainedFixture = false;
                    break;
                case "--keep-fixture":
                    options.CleanupFixture = false;
                    break;
                default:
                    throw new ArgumentException($"Unknown argument: {argument}");
            }
        }

        if (!string.IsNullOrWhiteSpace(settingsPath))
        {
            options.ApplySettingsFile(settingsPath);
            options.ApplyCommandLineOverrides(args);
        }

        options.RepositoryRoot = Path.GetFullPath(options.RepositoryRoot);
        options.LoadDevelopmentDefaults();

        if (!options.SelfContainedFixture
            && string.IsNullOrWhiteSpace(options.BearerToken))
        {
            options.BearerToken = Environment.GetEnvironmentVariable(
                options.BearerTokenEnvironmentVariable);
        }

        options.BaseUrl = options.BaseUrl.TrimEnd('/');
        options.IdentityBaseUrl = options.IdentityBaseUrl.TrimEnd('/');

        if (!options.OpenApiPath.StartsWith("/", StringComparison.Ordinal))
        {
            options.OpenApiPath = "/" + options.OpenApiPath;
        }

        return options;
    }

    private void LoadDevelopmentDefaults()
    {
        var apiSettings = Path.Combine(
            RepositoryRoot,
            "src",
            "SmartSchool.Api",
            "appsettings.Development.json");

        if (File.Exists(apiSettings))
        {
            using var document = JsonDocument.Parse(File.ReadAllText(apiSettings));
            var root = document.RootElement;

            if (IdentityBaseUrl.Equals(
                    "http://localhost:7101",
                    StringComparison.OrdinalIgnoreCase)
                && TryRead(root, out var authority, "Identity", "Authority")
                && !string.IsNullOrWhiteSpace(authority))
            {
                IdentityBaseUrl = authority!;
            }
        }

        var identitySettings = Path.Combine(
            RepositoryRoot,
            "src",
            "SmartSchool.Identity.Api",
            "appsettings.Development.json");

        if (File.Exists(identitySettings))
        {
            using var document = JsonDocument.Parse(File.ReadAllText(identitySettings));
            var root = document.RootElement;

            if (string.IsNullOrWhiteSpace(BootstrapSuperAdminEmail)
                && TryRead(
                    root,
                    out var email,
                    "BootstrapSuperAdmin",
                    "Email"))
            {
                BootstrapSuperAdminEmail = email;
            }

            if (string.IsNullOrWhiteSpace(BootstrapSuperAdminPassword)
                && TryRead(
                    root,
                    out var password,
                    "BootstrapSuperAdmin",
                    "Password"))
            {
                BootstrapSuperAdminPassword = password;
            }

            if (TryRead(
                    root,
                    out var loginClientId,
                    "LoginApiClient",
                    "ClientId")
                && !string.IsNullOrWhiteSpace(loginClientId))
            {
                LoginClientId = loginClientId!;
            }

            if (string.IsNullOrWhiteSpace(LoginClientSecret)
                && TryRead(
                    root,
                    out var loginClientSecret,
                    "LoginApiClient",
                    "ClientSecret"))
            {
                LoginClientSecret = loginClientSecret;
            }
        }

        IdentityBaseUrl =
            Environment.GetEnvironmentVariable(
                "SMARTSCHOOL_SMOKE_IDENTITY_URL")
            ?? IdentityBaseUrl;

        BootstrapSuperAdminEmail =
            Environment.GetEnvironmentVariable(
                "SMARTSCHOOL_SMOKE_BOOTSTRAP_EMAIL")
            ?? BootstrapSuperAdminEmail;

        BootstrapSuperAdminPassword =
            Environment.GetEnvironmentVariable(
                "SMARTSCHOOL_SMOKE_BOOTSTRAP_PASSWORD")
            ?? BootstrapSuperAdminPassword;

        LoginClientSecret =
            Environment.GetEnvironmentVariable(
                "SMARTSCHOOL_SMOKE_LOGIN_CLIENT_SECRET")
            ?? LoginClientSecret;
    }

    private static bool TryRead(
        JsonElement root,
        out string? value,
        params string[] path)
    {
        var current = root;
        foreach (var part in path)
        {
            if (current.ValueKind != JsonValueKind.Object
                || !current.TryGetProperty(part, out current))
            {
                value = null;
                return false;
            }
        }

        value = current.ValueKind == JsonValueKind.String
            ? current.GetString()
            : null;
        return !string.IsNullOrWhiteSpace(value);
    }

    private void ApplySettingsFile(string settingsPath)
    {
        if (!File.Exists(settingsPath))
        {
            throw new FileNotFoundException(
                "Smoke-test settings file was not found.",
                settingsPath);
        }

        var fromFile = JsonSerializer.Deserialize<SmokeTestOptions>(
            File.ReadAllText(settingsPath),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (fromFile is null)
        {
            return;
        }

        BaseUrl = fromFile.BaseUrl;
        IdentityBaseUrl = fromFile.IdentityBaseUrl;
        OpenApiPath = fromFile.OpenApiPath;
        BearerToken = fromFile.BearerToken;
        BearerTokenEnvironmentVariable = fromFile.BearerTokenEnvironmentVariable;
        IncludeMutatingEndpoints = fromFile.IncludeMutatingEndpoints;
        IncludeDeleteEndpoints = fromFile.IncludeDeleteEndpoints;
        Strict = fromFile.Strict;
        IgnoreTlsErrors = fromFile.IgnoreTlsErrors;
        TimeoutSeconds = fromFile.TimeoutSeconds;
        ReportDirectory = fromFile.ReportDirectory;
        SelfContainedFixture = fromFile.SelfContainedFixture;
        CleanupFixture = fromFile.CleanupFixture;
        RepositoryRoot = fromFile.RepositoryRoot;
        BootstrapSuperAdminEmail = fromFile.BootstrapSuperAdminEmail;
        BootstrapSuperAdminPassword = fromFile.BootstrapSuperAdminPassword;
        LoginClientId = fromFile.LoginClientId;
        LoginClientSecret = fromFile.LoginClientSecret;
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
                case "--identity-url":
                case "--identity-base-url":
                    IdentityBaseUrl = RequireValue(args, ref index, argument);
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
                case "--repo-root":
                    RepositoryRoot = RequireValue(args, ref index, argument);
                    break;
                case "--no-fixture":
                    SelfContainedFixture = false;
                    break;
                case "--keep-fixture":
                    CleanupFixture = false;
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
