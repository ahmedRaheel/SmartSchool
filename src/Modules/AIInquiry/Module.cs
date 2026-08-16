using FluentValidation;
using SmartSchool.Modules.AIInquiry.Features.HumanHandoff;
using SmartSchool.Modules.AIInquiry.Features.InquiryConversation;
using SmartSchool.Modules.AIInquiry.Features.InquiryMessage;
using SmartSchool.Modules.AIInquiry.Features.LeadCapture;

namespace SmartSchool.Modules.AIInquiry;

public static class Module
{
    public static IServiceCollection AddAIInquiryModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateHumanHandoff.Handler>();
        services.AddScoped<GetHumanHandoffById.Handler>();
        services.AddScoped<GetHumanHandoffPage.Handler>();
        services.AddScoped<UpdateHumanHandoff.Handler>();
        services.AddScoped<DeleteHumanHandoff.Handler>();
        services.AddScoped<IValidator<CreateHumanHandoff.Request>, CreateHumanHandoff.Validator>();
        services.AddScoped<IValidator<UpdateHumanHandoff.Request>, UpdateHumanHandoff.Validator>();
        services.AddScoped<CreateInquiryConversation.Handler>();
        services.AddScoped<GetInquiryConversationById.Handler>();
        services.AddScoped<GetInquiryConversationPage.Handler>();
        services.AddScoped<UpdateInquiryConversation.Handler>();
        services.AddScoped<DeleteInquiryConversation.Handler>();
        services.AddScoped<IValidator<CreateInquiryConversation.Request>, CreateInquiryConversation.Validator>();
        services.AddScoped<IValidator<UpdateInquiryConversation.Request>, UpdateInquiryConversation.Validator>();
        services.AddScoped<CreateInquiryMessage.Handler>();
        services.AddScoped<GetInquiryMessageById.Handler>();
        services.AddScoped<GetInquiryMessagePage.Handler>();
        services.AddScoped<UpdateInquiryMessage.Handler>();
        services.AddScoped<DeleteInquiryMessage.Handler>();
        services.AddScoped<IValidator<CreateInquiryMessage.Request>, CreateInquiryMessage.Validator>();
        services.AddScoped<IValidator<UpdateInquiryMessage.Request>, UpdateInquiryMessage.Validator>();
        services.AddScoped<CreateLeadCapture.Handler>();
        services.AddScoped<GetLeadCaptureById.Handler>();
        services.AddScoped<GetLeadCapturePage.Handler>();
        services.AddScoped<UpdateLeadCapture.Handler>();
        services.AddScoped<DeleteLeadCapture.Handler>();
        services.AddScoped<IValidator<CreateLeadCapture.Request>, CreateLeadCapture.Validator>();
        services.AddScoped<IValidator<UpdateLeadCapture.Request>, UpdateLeadCapture.Validator>();

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
