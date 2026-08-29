using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application;
using SmartSchool.Modules.Admissions.Features;
using SmartSchool.Modules.Admissions.Features.AdmissionDecision;
using SmartSchool.Modules.Admissions.Features.Applicant;
using SmartSchool.Modules.Admissions.Features.Application;
using SmartSchool.Modules.Admissions.Features.Inquiry;

using SmartSchool.Modules.Admissions.Features.DataAccess.AdmissionWorkflow;
using SmartSchool.Modules.Admissions.Features.DataAccess.nquiry;

namespace SmartSchool.Modules.Admissions;

public static class Module
{
    public static IServiceCollection AddAdmissionsModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);

        services.AddFeatureDataAccess(typeof(Module).Assembly);
        return services;
    }

    public static IEndpointRouteBuilder MapAdmissionsEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateAdmissionDecision.MapEndpoint(endpoints);
        CreateApplicant.MapEndpoint(endpoints);
        CreateApplication.MapEndpoint(endpoints);
        CreateInquiry.MapEndpoint(endpoints);
        DeleteAdmissionDecision.MapEndpoint(endpoints);
        DeleteApplicant.MapEndpoint(endpoints);
        DeleteApplication.MapEndpoint(endpoints);
        DeleteInquiry.MapEndpoint(endpoints);
        GetAdmissionDecisionById.MapEndpoint(endpoints);
        GetAdmissionDecisionPage.MapEndpoint(endpoints);
        GetApplicantById.MapEndpoint(endpoints);
        GetApplicantPage.MapEndpoint(endpoints);
        GetApplicationById.MapEndpoint(endpoints);
        GetApplicationPage.MapEndpoint(endpoints);
        GetInquiryById.MapEndpoint(endpoints);
        GetInquiryPage.MapEndpoint(endpoints);
        UpdateAdmissionDecision.MapEndpoint(endpoints);
        UpdateApplicant.MapEndpoint(endpoints);
        UpdateApplication.MapEndpoint(endpoints);
        UpdateInquiry.MapEndpoint(endpoints);
        AdmissionWorkflowEndpoints.MapEndpoints(endpoints);

        return endpoints;
    }
}
