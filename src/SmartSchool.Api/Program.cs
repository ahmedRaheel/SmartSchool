using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Serilog;
using SmartSchool.Infrastructure;
using SmartSchool.Infrastructure.Options;
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

builder.Services
    .AddAuthentication(AuthenticationConstants.BearerScheme)
    .AddJwtBearer(
        options =>
        {
            var identityOptions = builder.Configuration
                .GetSection(IdentityOptions.SectionName)
                .Get<IdentityOptions>()
                ?? throw new InvalidOperationException(
                    "Identity configuration is missing.");

            options.Authority =
                identityOptions.Authority;

            options.Audience =
                identityOptions.Audience;

            options.RequireHttpsMetadata =
                identityOptions.RequireHttpsMetadata;
        });

builder.Services.AddAuthorization();

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
