using SmartSchool.Modules.Communication.Features.Chat.SendChatMessage;
using SmartSchool.Modules.Communication.Features.Chat.CreateChatConversation;
using SmartSchool.Modules.Communication.Features.Chat.GetChatMessages;
using SmartSchool.Modules.Communication.Features.Chat.GetChatConversations;
using SmartSchool.Modules.Communication.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Features.Conversation;
using SmartSchool.Modules.Communication.Features.Chat;
using SmartSchool.Modules.Communication.Features.ConversationParticipant;
using SmartSchool.Modules.Communication.Features.Message;
using SmartSchool.Modules.Communication.Features.MessageReceipt;
using SmartSchool.Modules.Communication.Features.Notification;
using SmartSchool.Modules.Communication.Realtime;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication;

public static class Module
{
    public static IServiceCollection AddCommunicationModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddScoped<ICommunicationDbContext>(serviceProvider =>
            serviceProvider.GetRequiredService<CommunicationDbContext>());

        services.AddFeaturePersistence(typeof(Module).Assembly);
        services.AddSignalR();
        return services;
    }

    public static IEndpointRouteBuilder MapCommunicationEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateConversation.MapEndpoint(endpoints);
        GetConversationById.MapEndpoint(endpoints);
        GetConversationBySubjectId.MapEndpoint(endpoints);
        GetConversationByStudentId.MapEndpoint(endpoints);
        GetConversationByClassSectionId.MapEndpoint(endpoints);
        GetConversationByCampusId.MapEndpoint(endpoints);
        GetConversationPage.MapEndpoint(endpoints);
        UpdateConversation.MapEndpoint(endpoints);
        DeleteConversation.MapEndpoint(endpoints);
        CreateConversationParticipant.MapEndpoint(endpoints);
        GetConversationParticipantById.MapEndpoint(endpoints);
        GetConversationParticipantByConversationId.MapEndpoint(endpoints);
        GetConversationParticipantPage.MapEndpoint(endpoints);
        UpdateConversationParticipant.MapEndpoint(endpoints);
        DeleteConversationParticipant.MapEndpoint(endpoints);
        CreateMessage.MapEndpoint(endpoints);
        GetMessageById.MapEndpoint(endpoints);
        GetMessageByReplyToMessageId.MapEndpoint(endpoints);
        GetMessageByConversationId.MapEndpoint(endpoints);
        GetMessagePage.MapEndpoint(endpoints);
        UpdateMessage.MapEndpoint(endpoints);
        DeleteMessage.MapEndpoint(endpoints);
        CreateMessageReceipt.MapEndpoint(endpoints);
        GetMessageReceiptById.MapEndpoint(endpoints);
        GetMessageReceiptByMessageId.MapEndpoint(endpoints);
        GetMessageReceiptPage.MapEndpoint(endpoints);
        UpdateMessageReceipt.MapEndpoint(endpoints);
        DeleteMessageReceipt.MapEndpoint(endpoints);
        CreateNotification.MapEndpoint(endpoints);
        GetNotificationById.MapEndpoint(endpoints);
        GetNotificationPage.MapEndpoint(endpoints);
        UpdateNotification.MapEndpoint(endpoints);
        DeleteNotification.MapEndpoint(endpoints);
        MarkNotificationAsRead.MapEndpoint(endpoints);
        MarkAllNotificationsAsRead.MapEndpoint(endpoints);
        GetUnreadNotificationCount.MapEndpoint(endpoints);
        GetChatConversations.MapEndpoint(endpoints);
        GetChatMessages.MapEndpoint(endpoints);
        CreateChatConversation.MapEndpoint(endpoints);
        SendChatMessage.MapEndpoint(endpoints);

        return endpoints;
    }
}
