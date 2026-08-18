
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIParent.Features.ParentConversation;
using SmartSchool.Modules.AIParent.Features.ParentMessage;
using SmartSchool.Modules.AIParent.Features.ParentToolExecution;
using SmartSchool.Modules.AIParent.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent;

public static class Module
{
	public static IServiceCollection AddAIParentModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IParentConversationQuery, ParentConversationQuery>();
		services.AddScoped<IParentConversationCommand, ParentConversationCommand>();
		services.AddScoped<IParentMessageQuery, ParentMessageQuery>();
		services.AddScoped<IParentMessageCommand, ParentMessageCommand>();
		services.AddScoped<IParentToolExecutionQuery, ParentToolExecutionQuery>();
		services.AddScoped<IParentToolExecutionCommand, ParentToolExecutionCommand>();

		return services;
	}

	public static IEndpointRouteBuilder MapAIParentEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateParentConversation.MapEndpoint(endpoints);
		GetParentConversationById.MapEndpoint(endpoints);
		GetParentConversationPage.MapEndpoint(endpoints);
		UpdateParentConversation.MapEndpoint(endpoints);
		DeleteParentConversation.MapEndpoint(endpoints);
		CreateParentMessage.MapEndpoint(endpoints);
		GetParentMessageById.MapEndpoint(endpoints);
		GetParentMessagePage.MapEndpoint(endpoints);
		UpdateParentMessage.MapEndpoint(endpoints);
		DeleteParentMessage.MapEndpoint(endpoints);
		CreateParentToolExecution.MapEndpoint(endpoints);
		GetParentToolExecutionById.MapEndpoint(endpoints);
		GetParentToolExecutionPage.MapEndpoint(endpoints);
		UpdateParentToolExecution.MapEndpoint(endpoints);
		DeleteParentToolExecution.MapEndpoint(endpoints);

		return endpoints;
	}
}
