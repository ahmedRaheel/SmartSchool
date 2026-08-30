using SmartSchool.Modules.AIInquiry.Features;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIInquiry.Features.HumanHandoff;
using SmartSchool.Modules.AIInquiry.Features.InquiryConversation;
using SmartSchool.Modules.AIInquiry.Features.InquiryMessage;
using SmartSchool.Modules.AIInquiry.Features.LeadCapture;
using SmartSchool.SharedKernel;


namespace SmartSchool.Modules.AIInquiry;

public static class Module
{
	public static IServiceCollection AddAIInquiryModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

        services.AddFeaturePersistence(typeof(Module).Assembly);
		services.AddScoped<IHumanHandoffCommand, HumanHandoffCommand>();	
		
		services.AddScoped<ILeadCaptureCommand, LeadCaptureCommand>();
		

		return services;
	}

	public static IEndpointRouteBuilder MapAIInquiryEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateHumanHandoff.MapEndpoint(endpoints);
		GetHumanHandoffById.MapEndpoint(endpoints);
		GetHumanHandoffPage.MapEndpoint(endpoints);
		UpdateHumanHandoff.MapEndpoint(endpoints);
		DeleteHumanHandoff.MapEndpoint(endpoints);
		CreateInquiryConversation.MapEndpoint(endpoints);
		GetInquiryConversationById.MapEndpoint(endpoints);
		GetInquiryConversationPage.MapEndpoint(endpoints);
		UpdateInquiryConversation.MapEndpoint(endpoints);
		DeleteInquiryConversation.MapEndpoint(endpoints);
		CreateInquiryMessage.MapEndpoint(endpoints);
		GetInquiryMessageById.MapEndpoint(endpoints);
		GetInquiryMessagePage.MapEndpoint(endpoints);
		UpdateInquiryMessage.MapEndpoint(endpoints);
		DeleteInquiryMessage.MapEndpoint(endpoints);
		CreateLeadCapture.MapEndpoint(endpoints);
		GetLeadCaptureById.MapEndpoint(endpoints);
		GetLeadCapturePage.MapEndpoint(endpoints);
		UpdateLeadCapture.MapEndpoint(endpoints);
		DeleteLeadCapture.MapEndpoint(endpoints);

		
		return endpoints;
	}
}
