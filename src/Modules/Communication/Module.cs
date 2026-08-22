using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Features.Conversation;
using SmartSchool.Modules.Communication.Features.ConversationParticipant;
using SmartSchool.Modules.Communication.Features.Message;
using SmartSchool.Modules.Communication.Features.MessageReceipt;
using SmartSchool.Modules.Communication.Features.Notification;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.Modules.Communication.Realtime;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication;

public static class Module
{
	public static IServiceCollection AddCommunicationModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddSignalR();
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
		MarkNotificationAsRead.MapEndpoint(endpoints);
		MarkAllNotificationsAsRead.MapEndpoint(endpoints);
		GetUnreadNotificationCount.MapEndpoint(endpoints);

		return endpoints;
	}
}
