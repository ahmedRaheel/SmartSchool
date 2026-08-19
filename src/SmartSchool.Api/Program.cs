using SmartSchool.Modules.Reference;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using SmartSchool.Infrastructure.DependencyInjection;
using Scalar.AspNetCore;
using SmartSchool.Application;
using SmartSchool.Infrastructure.Identity;
using SmartSchool.Api.Seed;
using Microsoft.Extensions.Options;
using Serilog;
using SmartSchool.Infrastructure;
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
using SmartSchool.Modules.Documents;
using SmartSchool.Modules.Examinations;
using SmartSchool.Modules.Finance;
using SmartSchool.Modules.HR;
using SmartSchool.Modules.Identity;
using SmartSchool.Modules.Inventory;
using SmartSchool.Modules.Learning;
using SmartSchool.Modules.Library;
using SmartSchool.Modules.Organization;
using SmartSchool.Modules.Payroll;
using SmartSchool.Modules.Students;
using SmartSchool.Modules.Tenancy;
using SmartSchool.Modules.Transport;
using SmartSchool.Modules.Workflow;

var builder =
	WebApplication.CreateBuilder(args);

builder.AddSmartSchoolPlatform();

builder.Services.AddOpenApi();

builder.Services.AddSmartSchoolAuthorization();
builder.Services.AddScoped<SampleActorSeeder>();

builder.Services
	.AddHttpClient(
		ApplicationConstants.MachineLearningHttpClient,
		(serviceProvider, client) =>
		{
			var options = serviceProvider
				.GetRequiredService<IOptionsMonitor<MachineLearningOptions>>()
				.CurrentValue;

			client.BaseAddress =
				new Uri(options.BaseUrl);

			client.Timeout =
				TimeSpan.FromSeconds(options.TimeoutSeconds);
		})
	.AddStandardResilienceHandler();

builder.Services.AddAICoreModule();
builder.Services.AddAIInquiryModule();
builder.Services.AddAIParentModule();
builder.Services.AddAIPredictionModule();
builder.Services.AddAITutorModule();
builder.Services.AddAcademicsModule();
builder.Services.AddReferenceModule();
builder.Services.AddActivitiesModule();
builder.Services.AddAdmissionsModule();
builder.Services.AddAuditModule();
builder.Services.AddCommunicationModule();
builder.Services.AddDocumentsModule();
builder.Services.AddExaminationsModule();
builder.Services.AddFinanceModule();
builder.Services.AddHRModule();
builder.Services.AddIdentityModule();
builder.Services.AddInventoryModule();
builder.Services.AddLearningModule();
builder.Services.AddLibraryModule();
builder.Services.AddOrganizationModule();
builder.Services.AddPayrollModule();
builder.Services.AddStudentsModule();
builder.Services.AddTenancyModule();
builder.Services.AddTransportModule();
builder.Services.AddWorkflowModule();


var app =
	builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference(options => options.WithTitle("SmartSchool API"));

	using var scope =
		app.Services.CreateScope();

	var persistenceOptions = scope.ServiceProvider
		.GetRequiredService<IOptions<PersistenceOptions>>()
		.Value;

	if (persistenceOptions.Provider == PersistenceProvider.Mock)
	{
		var mockDatabaseSeeder =
			scope.ServiceProvider.GetRequiredService<MockDatabaseSeeder>();

		await mockDatabaseSeeder.SeedAsync();
	}

	var sampleActorSeeder =
		scope.ServiceProvider.GetRequiredService<SampleActorSeeder>();

	await sampleActorSeeder.SeedAsync();
}

app.UseExceptionHandler();
app.UseCorrelationId();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet(
	ApiRoutes.Health,
	() => Results.Ok(
		new
		{
			Status = ApplicationConstants.HealthStatusOk,
			Product = ApplicationConstants.ProductName
		}));

app.MapAICoreEndpoints();
app.MapAIInquiryEndpoints();
app.MapAIParentEndpoints();
app.MapAIPredictionEndpoints();
app.MapAITutorEndpoints();
app.MapAcademicsEndpoints();
app.MapReferenceEndpoints();
app.MapActivitiesEndpoints();
app.MapAdmissionsEndpoints();
app.MapAuditEndpoints();
app.MapCommunicationEndpoints();
app.MapDocumentsEndpoints();
app.MapExaminationsEndpoints();
app.MapFinanceEndpoints();
app.MapHREndpoints();
app.MapIdentityEndpoints();
app.MapInventoryEndpoints();
app.MapLearningEndpoints();
app.MapLibraryEndpoints();
app.MapOrganizationEndpoints();
app.MapPayrollEndpoints();
app.MapStudentsEndpoints();
app.MapTenancyEndpoints();
app.MapTransportEndpoints();
app.MapWorkflowEndpoints();

app.Run();
