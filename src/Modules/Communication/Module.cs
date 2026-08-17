using SmartSchool.Modules.Communication.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Persistence;
using FluentValidation;
using SmartSchool.Modules.Communication.Features.Conversation;
using SmartSchool.Modules.Communication.Features.ConversationParticipant;
using SmartSchool.Modules.Communication.Features.Message;
using SmartSchool.Modules.Communication.Features.MessageReceipt;
using SmartSchool.Modules.Communication.Features.Notification;

namespace SmartSchool.Modules.Communication;

public static class Module
{
    public static IServiceCollection AddCommunicationModule(
        this IServiceCollection services)
    {
        services.AddScoped<IConversationQuery, ConversationQuery>();
        services.AddScoped<IConversationCommand, ConversationCommand>();
        services.AddScoped<IConversationParticipantQuery, ConversationParticipantQuery>();
        services.AddScoped<IConversationParticipantCommand, ConversationParticipantCommand>();
        services.AddScoped<IMessageQuery, MessageQuery>();
        services.AddScoped<IMessageCommand, MessageCommand>();
        services.AddScoped<IMessageReceiptQuery, MessageReceiptQuery>();
        services.AddScoped<IMessageReceiptCommand, MessageReceiptCommand>();
        services.AddScoped<INotificationQuery, NotificationQuery>();
        services.AddScoped<INotificationCommand, NotificationCommand>();
        services.AddScoped<IValidator<CreateConversation.Request>, CreateConversation.Validator>();
        services.AddScoped<IValidator<UpdateConversation.Request>, UpdateConversation.Validator>();
        services.AddScoped<IValidator<CreateConversationParticipant.Request>, CreateConversationParticipant.Validator>();
        services.AddScoped<IValidator<UpdateConversationParticipant.Request>, UpdateConversationParticipant.Validator>();
        services.AddScoped<IValidator<CreateMessage.Request>, CreateMessage.Validator>();
        services.AddScoped<IValidator<UpdateMessage.Request>, UpdateMessage.Validator>();
        services.AddScoped<IValidator<CreateMessageReceipt.Request>, CreateMessageReceipt.Validator>();
        services.AddScoped<IValidator<UpdateMessageReceipt.Request>, UpdateMessageReceipt.Validator>();
        services.AddScoped<IValidator<CreateNotification.Request>, CreateNotification.Validator>();
        services.AddScoped<IValidator<UpdateNotification.Request>, UpdateNotification.Validator>();


        services.AddScoped<IRequestHandler<CreateConversation.Request, Result<ConversationResponse>>, CreateConversation.Handler>();
        services.AddScoped<IRequestHandler<GetConversationById.Query, Result<ConversationResponse>>, GetConversationById.Handler>();
        services.AddScoped<IRequestHandler<GetConversationPage.Query, Result<PagedResult<ConversationResponse>>>, GetConversationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateConversation.Request, Result<ConversationResponse>>, UpdateConversation.Handler>();
        services.AddScoped<IRequestHandler<DeleteConversation.Command, Result<DeleteConversation.Response>>, DeleteConversation.Handler>();
        services.AddScoped<IRequestHandler<CreateConversationParticipant.Request, Result<ConversationParticipantResponse>>, CreateConversationParticipant.Handler>();
        services.AddScoped<IRequestHandler<GetConversationParticipantById.Query, Result<ConversationParticipantResponse>>, GetConversationParticipantById.Handler>();
        services.AddScoped<IRequestHandler<GetConversationParticipantPage.Query, Result<PagedResult<ConversationParticipantResponse>>>, GetConversationParticipantPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateConversationParticipant.Request, Result<ConversationParticipantResponse>>, UpdateConversationParticipant.Handler>();
        services.AddScoped<IRequestHandler<DeleteConversationParticipant.Command, Result<DeleteConversationParticipant.Response>>, DeleteConversationParticipant.Handler>();
        services.AddScoped<IRequestHandler<CreateMessage.Request, Result<MessageResponse>>, CreateMessage.Handler>();
        services.AddScoped<IRequestHandler<GetMessageById.Query, Result<MessageResponse>>, GetMessageById.Handler>();
        services.AddScoped<IRequestHandler<GetMessagePage.Query, Result<PagedResult<MessageResponse>>>, GetMessagePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateMessage.Request, Result<MessageResponse>>, UpdateMessage.Handler>();
        services.AddScoped<IRequestHandler<DeleteMessage.Command, Result<DeleteMessage.Response>>, DeleteMessage.Handler>();
        services.AddScoped<IRequestHandler<CreateMessageReceipt.Request, Result<MessageReceiptResponse>>, CreateMessageReceipt.Handler>();
        services.AddScoped<IRequestHandler<GetMessageReceiptById.Query, Result<MessageReceiptResponse>>, GetMessageReceiptById.Handler>();
        services.AddScoped<IRequestHandler<GetMessageReceiptPage.Query, Result<PagedResult<MessageReceiptResponse>>>, GetMessageReceiptPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateMessageReceipt.Request, Result<MessageReceiptResponse>>, UpdateMessageReceipt.Handler>();
        services.AddScoped<IRequestHandler<DeleteMessageReceipt.Command, Result<DeleteMessageReceipt.Response>>, DeleteMessageReceipt.Handler>();
        services.AddScoped<IRequestHandler<CreateNotification.Request, Result<NotificationResponse>>, CreateNotification.Handler>();
        services.AddScoped<IRequestHandler<GetNotificationById.Query, Result<NotificationResponse>>, GetNotificationById.Handler>();
        services.AddScoped<IRequestHandler<GetNotificationPage.Query, Result<PagedResult<NotificationResponse>>>, GetNotificationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateNotification.Request, Result<NotificationResponse>>, UpdateNotification.Handler>();
        services.AddScoped<IRequestHandler<DeleteNotification.Command, Result<DeleteNotification.Response>>, DeleteNotification.Handler>();

        return services;
    }

    public static IEndpointRouteBuilder MapCommunicationEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateConversation.MapEndpoint(endpoints);
        GetConversationById.MapEndpoint(endpoints);
        GetConversationPage.MapEndpoint(endpoints);
        UpdateConversation.MapEndpoint(endpoints);
        DeleteConversation.MapEndpoint(endpoints);
        CreateConversationParticipant.MapEndpoint(endpoints);
        GetConversationParticipantById.MapEndpoint(endpoints);
        GetConversationParticipantPage.MapEndpoint(endpoints);
        UpdateConversationParticipant.MapEndpoint(endpoints);
        DeleteConversationParticipant.MapEndpoint(endpoints);
        CreateMessage.MapEndpoint(endpoints);
        GetMessageById.MapEndpoint(endpoints);
        GetMessagePage.MapEndpoint(endpoints);
        UpdateMessage.MapEndpoint(endpoints);
        DeleteMessage.MapEndpoint(endpoints);
        CreateMessageReceipt.MapEndpoint(endpoints);
        GetMessageReceiptById.MapEndpoint(endpoints);
        GetMessageReceiptPage.MapEndpoint(endpoints);
        UpdateMessageReceipt.MapEndpoint(endpoints);
        DeleteMessageReceipt.MapEndpoint(endpoints);
        CreateNotification.MapEndpoint(endpoints);
        GetNotificationById.MapEndpoint(endpoints);
        GetNotificationPage.MapEndpoint(endpoints);
        UpdateNotification.MapEndpoint(endpoints);
        DeleteNotification.MapEndpoint(endpoints);

        return endpoints;
    }
}
