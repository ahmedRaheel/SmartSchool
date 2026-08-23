using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions;

public static class Module
{
	public static IServiceCollection AddAdmissionsModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IAdmissionDecisionCommand, AdmissionDecisionCommand>();
		services.AddScoped<IAdmissionDecisionQuery, AdmissionDecisionQuery>();
		services.AddScoped<IApplicantCommand, ApplicantCommand>();
		services.AddScoped<IApplicantQuery, ApplicantQuery>();
		services.AddScoped<IApplicationCommand, ApplicationCommand>();
		services.AddScoped<IApplicationQuery, ApplicationQuery>();
		services.AddScoped<IAdmissionDecisionCommand, AdmissionDecisionCommand>();
		services.AddScoped<IAdmissionDecisionQuery, AdmissionDecisionQuery>();
		services.AddScoped<IApplicantCommand, ApplicantCommand>();	
		services.AddScoped<IApplicantQuery, ApplicantQuery>();
		services.AddScoped<IInquiryCommand, InquiryCommand>();
		services.AddScoped<IInquiryQuery, InquiryQuery>();
		return services;
	}

	public static IEndpointRouteBuilder MapAdmissionsEndpoints(
		this IEndpointRouteBuilder endpoints)
	{

		return endpoints;
	}
}
