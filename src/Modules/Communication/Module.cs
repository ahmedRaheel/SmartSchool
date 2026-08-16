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

        services.AddScoped<CreateConversation.Handler>();
        services.AddScoped<GetConversationById.Handler>();
        services.AddScoped<GetConversationPage.Handler>();
        services.AddScoped<UpdateConversation.Handler>();
        services.AddScoped<DeleteConversation.Handler>();
        services.AddScoped<IValidator<CreateConversation.Request>, CreateConversation.Validator>();
        services.AddScoped<IValidator<UpdateConversation.Request>, UpdateConversation.Validator>();
        services.AddScoped<CreateConversationParticipant.Handler>();
        services.AddScoped<GetConversationParticipantById.Handler>();
        services.AddScoped<GetConversationParticipantPage.Handler>();
        services.AddScoped<UpdateConversationParticipant.Handler>();
        services.AddScoped<DeleteConversationParticipant.Handler>();
        services.AddScoped<IValidator<CreateConversationParticipant.Request>, CreateConversationParticipant.Validator>();
        services.AddScoped<IValidator<UpdateConversationParticipant.Request>, UpdateConversationParticipant.Validator>();
        services.AddScoped<CreateMessage.Handler>();
        services.AddScoped<GetMessageById.Handler>();
        services.AddScoped<GetMessagePage.Handler>();
        services.AddScoped<UpdateMessage.Handler>();
        services.AddScoped<DeleteMessage.Handler>();
        services.AddScoped<IValidator<CreateMessage.Request>, CreateMessage.Validator>();
        services.AddScoped<IValidator<UpdateMessage.Request>, UpdateMessage.Validator>();
        services.AddScoped<CreateMessageReceipt.Handler>();
        services.AddScoped<GetMessageReceiptById.Handler>();
        services.AddScoped<GetMessageReceiptPage.Handler>();
        services.AddScoped<UpdateMessageReceipt.Handler>();
        services.AddScoped<DeleteMessageReceipt.Handler>();
        services.AddScoped<IValidator<CreateMessageReceipt.Request>, CreateMessageReceipt.Validator>();
        services.AddScoped<IValidator<UpdateMessageReceipt.Request>, UpdateMessageReceipt.Validator>();
        services.AddScoped<CreateNotification.Handler>();
        services.AddScoped<GetNotificationById.Handler>();
        services.AddScoped<GetNotificationPage.Handler>();
        services.AddScoped<UpdateNotification.Handler>();
        services.AddScoped<DeleteNotification.Handler>();
        services.AddScoped<IValidator<CreateNotification.Request>, CreateNotification.Validator>();
        services.AddScoped<IValidator<UpdateNotification.Request>, UpdateNotification.Validator>();

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
