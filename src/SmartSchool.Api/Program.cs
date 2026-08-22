using System.Security.Claims;
using System.Threading.Tasks;

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
using SmartSchool.Modules.Academics;
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
using SmartSchool.Modules.Tenancy;
using SmartSchool.Modules.Transport;
using SmartSchool.Modules.Workflow;

var builder = WebApplication.CreateBuilder(args);

builder.AddSmartSchoolPlatform();

var portalUrl =
	builder.Configuration.GetValue<string>("PortalUrl")
	?? "http://localhost:5173";

//
// Authentication
//
// IMPORTANT:
// AddSmartSchoolPlatform must NOT also register JwtBearer authentication.
// There should be one canonical bearer configuration.
//
var identityAuthority =
	builder.Configuration["Identity:Authority"]
	?? "http://localhost:7101";

var identityMetadataAddress =
	builder.Configuration["Identity:MetadataAddress"];

var identityAudience =
	builder.Configuration["Identity:Audience"]
	?? "smartschool-api";

builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme =
			JwtBearerDefaults.AuthenticationScheme;

		options.DefaultChallengeScheme =
			JwtBearerDefaults.AuthenticationScheme;

		options.DefaultScheme =
			JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(
		JwtBearerDefaults.AuthenticationScheme,
		options =>
		{
			options.Authority = identityAuthority;

			//
			// When SmartSchool.Api runs in Docker, localhost:7101
			// is not reachable from inside the API container.
			//
			// MetadataAddress lets the API retrieve discovery/signing
			// information through Docker while Authority remains the
			// actual JWT issuer.
			//
			if (!string.IsNullOrWhiteSpace(identityMetadataAddress))
			{
				options.MetadataAddress =
					identityMetadataAddress;
			}

			options.Audience =
				identityAudience;

			options.RequireHttpsMetadata =
				builder.Configuration.GetValue(
					"Identity:RequireHttpsMetadata",
					false);

			options.MapInboundClaims = false;

			options.TokenValidationParameters =
				new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer =
						identityAuthority.TrimEnd('/'),

					ValidateAudience = true,
					ValidAudience =
						identityAudience,

					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,

					NameClaimType = "name",
					RoleClaimType = "role",

					ClockSkew =
						TimeSpan.FromMinutes(1)
				};

			options.Events =
				new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						//
						// SignalR WebSocket authentication.
						//
						var accessToken =
							context.Request.Query["access_token"];

						var path =
							context.HttpContext.Request.Path;

						if (
							!string.IsNullOrWhiteSpace(accessToken)
							&& path.StartsWithSegments("/hubs"))
						{
							context.Token =
								accessToken;
						}

						return Task.CompletedTask;
					},

					OnTokenValidated = context =>
					{
						var logger =
							context.HttpContext
								.RequestServices
								.GetRequiredService<ILoggerFactory>()
								.CreateLogger(
									"SmartSchool.Authentication");

						var subject =
							context.Principal?
								.FindFirstValue("sub");

						var roles =
							context.Principal?
								.FindAll("role")
								.Select(claim => claim.Value)
								.ToArray()
							?? [];

						logger.LogInformation(
							"JWT authenticated. Subject={Subject}, Roles={Roles}",
							subject,
							string.Join(",", roles));

						return Task.CompletedTask;
					},

					OnAuthenticationFailed = context =>
					{
						var logger =
							context.HttpContext
								.RequestServices
								.GetRequiredService<ILoggerFactory>()
								.CreateLogger(
									"SmartSchool.Authentication");

						logger.LogError(
							context.Exception,
							"JWT authentication failed.");

						return Task.CompletedTask;
					},

					OnChallenge = context =>
					{
						var logger =
							context.HttpContext
								.RequestServices
								.GetRequiredService<ILoggerFactory>()
								.CreateLogger(
									"SmartSchool.Authentication");

						logger.LogWarning(
							"JWT challenge. Error={Error}, Description={Description}",
							context.Error,
							context.ErrorDescription);

						return Task.CompletedTask;
					}
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
builder.Services.AddAICoreModule();
builder.Services.AddAIInquiryModule();
builder.Services.AddAIParentModule();
builder.Services.AddAIPredictionModule();
builder.Services.AddAITutorModule();

builder.Services.AddAcademicsModule();
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
builder.Services.AddPayrollModule();
builder.Services.AddReferenceModule();
builder.Services.AddStudentsModule();
builder.Services.AddTenancyModule();
builder.Services.AddTransportModule();
builder.Services.AddWorkflowModule();

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
app.MapWorkflowCatalogEndpoints();
app.MapClientTelemetryEndpoints();
app.MapActorProfileEndpoints();

app.MapAICoreEndpoints();
app.MapAIInquiryEndpoints();
app.MapAIParentEndpoints();
app.MapAIPredictionEndpoints();
app.MapAITutorEndpoints();

app.MapAcademicsEndpoints();
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
app.MapPayrollEndpoints();
app.MapReferenceEndpoints();
app.MapStudentsEndpoints();
app.MapTenancyEndpoints();
app.MapTransportEndpoints();
app.MapWorkflowEndpoints();

//
// Temporary authentication diagnostic endpoint.
//
// Remove after authentication has been verified.
//
app.MapGet(
		"/api/debug/auth",
		(HttpContext context) =>
		{
			var claims =
				context.User.Claims
					.Select(
						claim =>
							new
							{
								claim.Type,
								claim.Value
							})
					.ToArray();

			return Results.Ok(
				new
				{
					isAuthenticated =
						context.User.Identity?
							.IsAuthenticated,

					authenticationType =
						context.User.Identity?
							.AuthenticationType,

					subject =
						context.User
							.FindFirstValue("sub"),

					roles =
						context.User
							.FindAll("role")
							.Select(
								claim =>
									claim.Value)
							.ToArray(),

					isSuperAdmin =
						context.User
							.IsInRole(
								"SuperAdmin"),

					Claims =
						claims
				});
		})
	.RequireAuthorization();

app.Run();
