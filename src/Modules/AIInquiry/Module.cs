using SmartSchool.Modules.AIInquiry.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIInquiry.Persistence;
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
        services.AddScoped<IHumanHandoffQuery, HumanHandoffQuery>();
        services.AddScoped<IHumanHandoffCommand, HumanHandoffCommand>();
        services.AddScoped<IInquiryConversationQuery, InquiryConversationQuery>();
        services.AddScoped<IInquiryConversationCommand, InquiryConversationCommand>();
        services.AddScoped<IInquiryMessageQuery, InquiryMessageQuery>();
        services.AddScoped<IInquiryMessageCommand, InquiryMessageCommand>();
        services.AddScoped<ILeadCaptureQuery, LeadCaptureQuery>();
        services.AddScoped<ILeadCaptureCommand, LeadCaptureCommand>();
        services.AddScoped<IValidator<CreateHumanHandoff.Request>, CreateHumanHandoff.Validator>();
        services.AddScoped<IValidator<UpdateHumanHandoff.Request>, UpdateHumanHandoff.Validator>();
        services.AddScoped<IValidator<CreateInquiryConversation.Request>, CreateInquiryConversation.Validator>();
        services.AddScoped<IValidator<UpdateInquiryConversation.Request>, UpdateInquiryConversation.Validator>();
        services.AddScoped<IValidator<CreateInquiryMessage.Request>, CreateInquiryMessage.Validator>();
        services.AddScoped<IValidator<UpdateInquiryMessage.Request>, UpdateInquiryMessage.Validator>();
        services.AddScoped<IValidator<CreateLeadCapture.Request>, CreateLeadCapture.Validator>();
        services.AddScoped<IValidator<UpdateLeadCapture.Request>, UpdateLeadCapture.Validator>();


        services.AddScoped<IRequestHandler<CreateHumanHandoff.Request, Result<HumanHandoffResponse>>, CreateHumanHandoff.Handler>();
        services.AddScoped<IRequestHandler<GetHumanHandoffById.Query, Result<HumanHandoffResponse>>, GetHumanHandoffById.Handler>();
        services.AddScoped<IRequestHandler<GetHumanHandoffPage.Query, Result<PagedResult<HumanHandoffResponse>>>, GetHumanHandoffPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateHumanHandoff.Request, Result<HumanHandoffResponse>>, UpdateHumanHandoff.Handler>();
        services.AddScoped<IRequestHandler<DeleteHumanHandoff.Command, Result<DeleteHumanHandoff.Response>>, DeleteHumanHandoff.Handler>();
        services.AddScoped<IRequestHandler<CreateInquiryConversation.Request, Result<InquiryConversationResponse>>, CreateInquiryConversation.Handler>();
        services.AddScoped<IRequestHandler<GetInquiryConversationById.Query, Result<InquiryConversationResponse>>, GetInquiryConversationById.Handler>();
        services.AddScoped<IRequestHandler<GetInquiryConversationPage.Query, Result<PagedResult<InquiryConversationResponse>>>, GetInquiryConversationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateInquiryConversation.Request, Result<InquiryConversationResponse>>, UpdateInquiryConversation.Handler>();
        services.AddScoped<IRequestHandler<DeleteInquiryConversation.Command, Result<DeleteInquiryConversation.Response>>, DeleteInquiryConversation.Handler>();
        services.AddScoped<IRequestHandler<CreateInquiryMessage.Request, Result<InquiryMessageResponse>>, CreateInquiryMessage.Handler>();
        services.AddScoped<IRequestHandler<GetInquiryMessageById.Query, Result<InquiryMessageResponse>>, GetInquiryMessageById.Handler>();
        services.AddScoped<IRequestHandler<GetInquiryMessagePage.Query, Result<PagedResult<InquiryMessageResponse>>>, GetInquiryMessagePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateInquiryMessage.Request, Result<InquiryMessageResponse>>, UpdateInquiryMessage.Handler>();
        services.AddScoped<IRequestHandler<DeleteInquiryMessage.Command, Result<DeleteInquiryMessage.Response>>, DeleteInquiryMessage.Handler>();
        services.AddScoped<IRequestHandler<CreateLeadCapture.Request, Result<LeadCaptureResponse>>, CreateLeadCapture.Handler>();
        services.AddScoped<IRequestHandler<GetLeadCaptureById.Query, Result<LeadCaptureResponse>>, GetLeadCaptureById.Handler>();
        services.AddScoped<IRequestHandler<GetLeadCapturePage.Query, Result<PagedResult<LeadCaptureResponse>>>, GetLeadCapturePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateLeadCapture.Request, Result<LeadCaptureResponse>>, UpdateLeadCapture.Handler>();
        services.AddScoped<IRequestHandler<DeleteLeadCapture.Command, Result<DeleteLeadCapture.Response>>, DeleteLeadCapture.Handler>();

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
