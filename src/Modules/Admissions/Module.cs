using SmartSchool.Modules.Admissions.Persistence;
using FluentValidation;
using SmartSchool.Modules.Admissions.Features.AdmissionDecision;
using SmartSchool.Modules.Admissions.Features.Applicant;
using SmartSchool.Modules.Admissions.Features.Application;
using SmartSchool.Modules.Admissions.Features.Inquiry;

namespace SmartSchool.Modules.Admissions;

public static class Module
{
    public static IServiceCollection AddAdmissionsModule(
        this IServiceCollection services)
    {
        services.AddScoped<IAdmissionDecisionQuery, AdmissionDecisionQuery>();
        services.AddScoped<IAdmissionDecisionCommand, AdmissionDecisionCommand>();
        services.AddScoped<IApplicantQuery, ApplicantQuery>();
        services.AddScoped<IApplicantCommand, ApplicantCommand>();
        services.AddScoped<IApplicationQuery, ApplicationQuery>();
        services.AddScoped<IApplicationCommand, ApplicationCommand>();
        services.AddScoped<IInquiryQuery, InquiryQuery>();
        services.AddScoped<IInquiryCommand, InquiryCommand>();

        services.AddScoped<CreateAdmissionDecision.Handler>();
        services.AddScoped<GetAdmissionDecisionById.Handler>();
        services.AddScoped<GetAdmissionDecisionPage.Handler>();
        services.AddScoped<UpdateAdmissionDecision.Handler>();
        services.AddScoped<DeleteAdmissionDecision.Handler>();
        services.AddScoped<IValidator<CreateAdmissionDecision.Request>, CreateAdmissionDecision.Validator>();
        services.AddScoped<IValidator<UpdateAdmissionDecision.Request>, UpdateAdmissionDecision.Validator>();
        services.AddScoped<CreateApplicant.Handler>();
        services.AddScoped<GetApplicantById.Handler>();
        services.AddScoped<GetApplicantPage.Handler>();
        services.AddScoped<UpdateApplicant.Handler>();
        services.AddScoped<DeleteApplicant.Handler>();
        services.AddScoped<IValidator<CreateApplicant.Request>, CreateApplicant.Validator>();
        services.AddScoped<IValidator<UpdateApplicant.Request>, UpdateApplicant.Validator>();
        services.AddScoped<CreateApplication.Handler>();
        services.AddScoped<GetApplicationById.Handler>();
        services.AddScoped<GetApplicationPage.Handler>();
        services.AddScoped<UpdateApplication.Handler>();
        services.AddScoped<DeleteApplication.Handler>();
        services.AddScoped<IValidator<CreateApplication.Request>, CreateApplication.Validator>();
        services.AddScoped<IValidator<UpdateApplication.Request>, UpdateApplication.Validator>();
        services.AddScoped<CreateInquiry.Handler>();
        services.AddScoped<GetInquiryById.Handler>();
        services.AddScoped<GetInquiryPage.Handler>();
        services.AddScoped<UpdateInquiry.Handler>();
        services.AddScoped<DeleteInquiry.Handler>();
        services.AddScoped<IValidator<CreateInquiry.Request>, CreateInquiry.Validator>();
        services.AddScoped<IValidator<UpdateInquiry.Request>, UpdateInquiry.Validator>();

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
