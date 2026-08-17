using SmartSchool.Modules.Admissions.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
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
        services.AddScoped<IValidator<CreateAdmissionDecision.Request>, CreateAdmissionDecision.Validator>();
        services.AddScoped<IValidator<UpdateAdmissionDecision.Request>, UpdateAdmissionDecision.Validator>();
        services.AddScoped<IValidator<CreateApplicant.Request>, CreateApplicant.Validator>();
        services.AddScoped<IValidator<UpdateApplicant.Request>, UpdateApplicant.Validator>();
        services.AddScoped<IValidator<CreateApplication.Request>, CreateApplication.Validator>();
        services.AddScoped<IValidator<UpdateApplication.Request>, UpdateApplication.Validator>();
        services.AddScoped<IValidator<CreateInquiry.Request>, CreateInquiry.Validator>();
        services.AddScoped<IValidator<UpdateInquiry.Request>, UpdateInquiry.Validator>();


        services.AddScoped<IRequestHandler<CreateAdmissionDecision.Request, Result<AdmissionDecisionResponse>>, CreateAdmissionDecision.Handler>();
        services.AddScoped<IRequestHandler<GetAdmissionDecisionById.Query, Result<AdmissionDecisionResponse>>, GetAdmissionDecisionById.Handler>();
        services.AddScoped<IRequestHandler<GetAdmissionDecisionPage.Query, Result<PagedResult<AdmissionDecisionResponse>>>, GetAdmissionDecisionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateAdmissionDecision.Request, Result<AdmissionDecisionResponse>>, UpdateAdmissionDecision.Handler>();
        services.AddScoped<IRequestHandler<DeleteAdmissionDecision.Command, Result<DeleteAdmissionDecision.Response>>, DeleteAdmissionDecision.Handler>();
        services.AddScoped<IRequestHandler<CreateApplicant.Request, Result<ApplicantResponse>>, CreateApplicant.Handler>();
        services.AddScoped<IRequestHandler<GetApplicantById.Query, Result<ApplicantResponse>>, GetApplicantById.Handler>();
        services.AddScoped<IRequestHandler<GetApplicantPage.Query, Result<PagedResult<ApplicantResponse>>>, GetApplicantPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateApplicant.Request, Result<ApplicantResponse>>, UpdateApplicant.Handler>();
        services.AddScoped<IRequestHandler<DeleteApplicant.Command, Result<DeleteApplicant.Response>>, DeleteApplicant.Handler>();
        services.AddScoped<IRequestHandler<CreateApplication.Request, Result<ApplicationResponse>>, CreateApplication.Handler>();
        services.AddScoped<IRequestHandler<GetApplicationById.Query, Result<ApplicationResponse>>, GetApplicationById.Handler>();
        services.AddScoped<IRequestHandler<GetApplicationPage.Query, Result<PagedResult<ApplicationResponse>>>, GetApplicationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateApplication.Request, Result<ApplicationResponse>>, UpdateApplication.Handler>();
        services.AddScoped<IRequestHandler<DeleteApplication.Command, Result<DeleteApplication.Response>>, DeleteApplication.Handler>();
        services.AddScoped<IRequestHandler<CreateInquiry.Request, Result<InquiryResponse>>, CreateInquiry.Handler>();
        services.AddScoped<IRequestHandler<GetInquiryById.Query, Result<InquiryResponse>>, GetInquiryById.Handler>();
        services.AddScoped<IRequestHandler<GetInquiryPage.Query, Result<PagedResult<InquiryResponse>>>, GetInquiryPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateInquiry.Request, Result<InquiryResponse>>, UpdateInquiry.Handler>();
        services.AddScoped<IRequestHandler<DeleteInquiry.Command, Result<DeleteInquiry.Response>>, DeleteInquiry.Handler>();

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
