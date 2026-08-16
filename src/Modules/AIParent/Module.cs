using FluentValidation;
using SmartSchool.Modules.AIParent.Features.ParentConversation;
using SmartSchool.Modules.AIParent.Features.ParentMessage;
using SmartSchool.Modules.AIParent.Features.ParentToolExecution;

namespace SmartSchool.Modules.AIParent;

public static class Module
{
    public static IServiceCollection AddAIParentModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateParentConversation.Handler>();
        services.AddScoped<GetParentConversationById.Handler>();
        services.AddScoped<GetParentConversationPage.Handler>();
        services.AddScoped<UpdateParentConversation.Handler>();
        services.AddScoped<DeleteParentConversation.Handler>();
        services.AddScoped<IValidator<CreateParentConversation.Request>, CreateParentConversation.Validator>();
        services.AddScoped<IValidator<UpdateParentConversation.Request>, UpdateParentConversation.Validator>();
        services.AddScoped<CreateParentMessage.Handler>();
        services.AddScoped<GetParentMessageById.Handler>();
        services.AddScoped<GetParentMessagePage.Handler>();
        services.AddScoped<UpdateParentMessage.Handler>();
        services.AddScoped<DeleteParentMessage.Handler>();
        services.AddScoped<IValidator<CreateParentMessage.Request>, CreateParentMessage.Validator>();
        services.AddScoped<IValidator<UpdateParentMessage.Request>, UpdateParentMessage.Validator>();
        services.AddScoped<CreateParentToolExecution.Handler>();
        services.AddScoped<GetParentToolExecutionById.Handler>();
        services.AddScoped<GetParentToolExecutionPage.Handler>();
        services.AddScoped<UpdateParentToolExecution.Handler>();
        services.AddScoped<DeleteParentToolExecution.Handler>();
        services.AddScoped<IValidator<CreateParentToolExecution.Request>, CreateParentToolExecution.Validator>();
        services.AddScoped<IValidator<UpdateParentToolExecution.Request>, UpdateParentToolExecution.Validator>();

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
