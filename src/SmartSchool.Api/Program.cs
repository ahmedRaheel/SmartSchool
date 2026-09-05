using ModelContextProtocol.AspNetCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Scalar.AspNetCore;
using Serilog;

using SmartSchool.Api.Features;
using SmartSchool.Api.Observability;
using SmartSchool.Api.Seed;
using SmartSchool.Application;
using SmartSchool.Infrastructure;
using SmartSchool.Infrastructure.DependencyInjection;
using SmartSchool.Infrastructure.Identity;
using SmartSchool.Infrastructure.Options;
using SmartSchool.Infrastructure.Persistence;
using SmartSchool.SharedKernel.Constants;

using SmartSchool.Modules.AICore;
using SmartSchool.Modules.AIInquiry;
using SmartSchool.Modules.AIParent;
using SmartSchool.Modules.AIPrediction;
using SmartSchool.Modules.AITutor;
using SmartSchool.Modules.Activities;
using SmartSchool.Modules.Admissions;
using SmartSchool.Modules.Audit;
using SmartSchool.Modules.Communication;
using SmartSchool.Modules.Communication.Realtime;
using SmartSchool.Modules.Documents;
using SmartSchool.Modules.Examinations;
using SmartSchool.Modules.Finance;
using SmartSchool.Modules.HR;
using SmartSchool.Modules.Inventory;
using SmartSchool.Modules.Learning;
using SmartSchool.Modules.Library;
using SmartSchool.Modules.Organization;
using SmartSchool.Modules.Payroll;
using SmartSchool.Modules.Reference;
using SmartSchool.Modules.Students;
using SmartSchool.Modules.Transport;
using SmartSchool.Modules.Workflow;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Accept enum names from the React UI while retaining numeric enum support.
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.AddSmartSchoolPlatform();

var portalUrl = builder.Configuration.GetValue<string>("PortalUrl")
    ?? throw new InvalidOperationException("PortalUrl configuration is required.");

var identityOptions = builder.Configuration
    .GetRequiredSection(AuthenticationOptions.SectionName)
    .Get<AuthenticationOptions>()
    ?? throw new InvalidOperationException("Identity configuration is required.");

//
// Authentication
//
// IMPORTANT:
// AddSmartSchoolPlatform must NOT also register JwtBearer authentication.
// There should be one canonical bearer configuration.
//

builder.Services.Configure<JwtBearerOptions>(
    JwtBearerDefaults.AuthenticationScheme,
    options =>
    {
        options.Authority = identityOptions.Authority;
        options.MetadataAddress = identityOptions.MetadataAddress;
        options.Audience = identityOptions.Audience;
        options.RequireHttpsMetadata = identityOptions.RequireHttpsMetadata;
        options.MapInboundClaims = false;

        options.TokenValidationParameters ??=
            new TokenValidationParameters();

        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidIssuer = identityOptions.ValidIssuer;

        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidAudience = identityOptions.Audience;

        options.TokenValidationParameters.ValidateLifetime = true;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;

        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";

        options.Events ??= new JwtBearerEvents();

        options.Events.OnMessageReceived = context =>
        {
            var accessToken =
                context.Request.Query["access_token"];

            if (!string.IsNullOrWhiteSpace(accessToken) &&
                context.HttpContext.Request.Path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        };

        options.Events.OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("SmartSchool.Authentication");

            logger.LogError(
                context.Exception,
                "JWT authentication failed.");

            return Task.CompletedTask;
        };

        options.Events.OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("SmartSchool.Authentication");

            logger.LogInformation(
                "JWT validated. Subject={Subject}, Roles={Roles}",
                context.Principal?.FindFirst("sub")?.Value,
                string.Join(
                    ",",
                    context.Principal?
                        .FindAll("role")
                        .Select(x => x.Value)
                    ?? []));

            return Task.CompletedTask;
        };
    });




//
// Authorization policies
//
builder.Services.AddSmartSchoolAuthorization();

builder.Services.AddOpenApi();

builder.Services.AddSmartSchoolObservability(
    builder.Configuration,
    "SmartSchool.Api");

builder.Services.AddCors(
    options =>
        options.AddPolicy(
            "Portal",
            policy =>
                policy
                    .WithOrigins(portalUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders(
                        "X-Correlation-ID",
                        "X-Trace-Id")));

builder.Services.AddScoped<SampleActorSeeder>();

builder.Services.AddHttpClient("Ollama", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration["AI:Ollama:BaseUrl"]
        ?? throw new InvalidOperationException("AI:Ollama:BaseUrl configuration is required.");
    var timeoutSeconds = configuration.GetValue("AI:Ollama:TimeoutSeconds", 180);

    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
});

builder.Services
    .AddHttpClient(
        ApplicationConstants.MachineLearningHttpClient,
        (serviceProvider, client) =>
        {
            var options =
                serviceProvider
                    .GetRequiredService<
                        IOptionsMonitor<MachineLearningOptions>>()
                    .CurrentValue;

            client.BaseAddress =
                new Uri(options.BaseUrl);

            client.Timeout =
                TimeSpan.FromSeconds(
                    options.TimeoutSeconds);
        })
    .AddStandardResilienceHandler();

//
// Modules
//
builder.Services.AddModuleDbContexts(builder.Configuration);

builder.Services.AddAICoreModule(builder.Configuration);
builder.Services.AddAIInquiryModule();
builder.Services.AddAIParentModule();
builder.Services.AddAIPredictionModule();
builder.Services.AddAITutorModule();
builder.Services.AddActivitiesModule();
builder.Services.AddAdmissionsModule();
builder.Services.AddAuditModule();
builder.Services.AddCommunicationModule();
builder.Services.AddDocumentsModule();
builder.Services.AddExaminationsModule();
builder.Services.AddFinanceModule();
builder.Services.AddHRModule();
builder.Services.AddInventoryModule();
builder.Services.AddLearningModule();
builder.Services.AddLibraryModule();
builder.Services.AddOrganizationModule();
builder.Services.AddReferenceModule();
builder.Services.AddStudentsModule();

builder.Services.AddTransportModule();
builder.Services.AddWorkflowModule();

builder.Services.AddHostedService<KafkaCommunicationConsumer>();
builder.Services.AddHostedService<KafkaCagInvalidationConsumer>();

var app = builder.Build();

//
// Development
//
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(
        options =>
            options.WithTitle(
                "SmartSchool API"));

    using var scope =
        app.Services.CreateScope();

    var persistenceOptions =
        scope.ServiceProvider
            .GetRequiredService<
                IOptions<PersistenceOptions>>()
            .Value;

    if (
        persistenceOptions.Provider
        == PersistenceProvider.Mock)
    {
        var mockDatabaseSeeder =
            scope.ServiceProvider
                .GetRequiredService<
                    MockDatabaseSeeder>();

        await mockDatabaseSeeder.SeedAsync();
    }

    var sampleActorSeeder =
        scope.ServiceProvider
            .GetRequiredService<
                SampleActorSeeder>();

    await sampleActorSeeder.SeedAsync();
}

//
// HTTP pipeline
//
app.UseMiddleware<
    SmartSchool.Api.Middleware.ResultResponseMiddleware>();

//app.UseMiddleware<
//    SmartSchool.Api.Middleware.BusinessContactValidationMiddleware>();

app.UseExceptionHandler();

app.UseCors("Portal");

app.UseCorrelationId();
app.UseTelemetryResponseHeaders();

app.UseSerilogRequestLogging();

//
// Authentication MUST run before authorization.
//
app.UseAuthentication();
app.UseAuthorization();

//
// Health
//
app.MapGet(
    ApiRoutes.Health,
    () =>
        Results.Ok(
            new
            {
                Status =
                    ApplicationConstants.HealthStatusOk,

                Product =
                    ApplicationConstants.ProductName
            }));

app.MapSmartSchoolHealth();

//
// Application endpoints
//
app.MapDashboardEndpoints();
app.MapPlatformFeatureEndpoints();
app.MapAiAssistantEndpoints();
app.MapRagChatbotEndpoints();
app.MapWorkflowCatalogEndpoints();
app.MapClientTelemetryEndpoints();
app.MapApplicationLogEndpoints();
app.MapActorProfileEndpoints();

app.MapAICoreEndpoints();
app.MapMcp("/mcp").RequireAuthorization();
app.MapAIInquiryEndpoints();
app.MapAIParentEndpoints();
app.MapAIPredictionEndpoints();
app.MapAITutorEndpoints();

app.MapActivitiesEndpoints();
app.MapAdmissionsEndpoints();
app.MapAuditEndpoints();

app.MapCommunicationEndpoints();

//
// SignalR
//
app.MapHub<NotificationHub>(
        "/hubs/notifications")
    .RequireAuthorization();

app.MapHub<ChatHub>(
        "/hubs/chat")
    .RequireAuthorization();

app.MapDocumentsEndpoints();
app.MapExaminationsEndpoints();
app.MapFinanceEndpoints();
app.MapHREndpoints();
app.MapInventoryEndpoints();
app.MapLearningEndpoints();
app.MapLibraryEndpoints();
app.MapOrganizationEndpoints();
app.MapReferenceEndpoints();
app.MapStudentsEndpoints();
app.MapTeachersEndpoints();
app.MapTransportEndpoints();
app.MapWorkflowEndpoints();


app.Run();
