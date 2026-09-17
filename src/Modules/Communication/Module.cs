using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Features.Chat.CreateChatConversation;
using SmartSchool.Modules.Communication.Features.Chat.GetChatConversations;
using SmartSchool.Modules.Communication.Features.Chat.GetChatMessages;
using SmartSchool.Modules.Communication.Features.Chat.MarkChatConversationRead;
using SmartSchool.Modules.Communication.Features.Chat.SendChatMessage;
using SmartSchool.Modules.Communication.Features.Notification;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.Modules.Communication.Realtime;

namespace SmartSchool.Modules.Communication;

public static class Module
{
    public static IServiceCollection AddCommunicationModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddModuleDbContext<CommunicationDbContext, ICommunicationDbContext>(
            configuration,
            ModuleConstants.Schema);
        services.AddFeaturePersistence(typeof(Module).Assembly);
        services.AddSignalR();
        return services;
    }

    public static IEndpointRouteBuilder MapCommunicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
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
        MarkChatConversationRead.MapEndpoint(endpoints);

        return endpoints;
    }

    public static IEndpointRouteBuilder MapCommunicationHubs(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<ChatHub>("/hubs/chat");
        endpoints.MapHub<NotificationHub>("/hubs/notifications");
        return endpoints;
    }
}
