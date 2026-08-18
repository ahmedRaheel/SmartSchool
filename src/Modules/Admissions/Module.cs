
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Admissions.Features.AdmissionDecision;
using SmartSchool.Modules.Admissions.Features.Applicant;
using SmartSchool.Modules.Admissions.Features.Application;
using SmartSchool.Modules.Admissions.Features.Inquiry;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions;

public static class Module
{
    public static IServiceCollection AddAdmissionsModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddScoped<IAdmissionDecisionQuery, AdmissionDecisionQuery>();
        services.AddScoped<IAdmissionDecisionCommand, AdmissionDecisionCommand>();
        services.AddScoped<IApplicantQuery, ApplicantQuery>();
        services.AddScoped<IApplicantCommand, ApplicantCommand>();
        services.AddScoped<IApplicationQuery, ApplicationQuery>();
        services.AddScoped<IApplicationCommand, ApplicationCommand>();
        services.AddScoped<IInquiryQuery, InquiryQuery>();
        services.AddScoped<IInquiryCommand, InquiryCommand>();

        return services;
    }

    public static IEndpointRouteBuilder MapAdmissionsEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateAdmissionDecision.MapEndpoint(endpoints);
        GetAdmissionDecisionById.MapEndpoint(endpoints);
        GetAdmissionDecisionPage.MapEndpoint(endpoints);
        UpdateAdmissionDecision.MapEndpoint(endpoints);
        DeleteAdmissionDecision.MapEndpoint(endpoints);
        CreateApplicant.MapEndpoint(endpoints);
        GetApplicantById.MapEndpoint(endpoints);
        GetApplicantPage.MapEndpoint(endpoints);
        UpdateApplicant.MapEndpoint(endpoints);
        DeleteApplicant.MapEndpoint(endpoints);
        CreateApplication.MapEndpoint(endpoints);
        GetApplicationById.MapEndpoint(endpoints);
        GetApplicationPage.MapEndpoint(endpoints);
        UpdateApplication.MapEndpoint(endpoints);
        DeleteApplication.MapEndpoint(endpoints);
        CreateInquiry.MapEndpoint(endpoints);
        GetInquiryById.MapEndpoint(endpoints);
        GetInquiryPage.MapEndpoint(endpoints);
        UpdateInquiry.MapEndpoint(endpoints);
        DeleteInquiry.MapEndpoint(endpoints);

        return endpoints;
    }
}
