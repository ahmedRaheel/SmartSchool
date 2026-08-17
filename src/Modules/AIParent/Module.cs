using SmartSchool.Modules.AIParent.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIParent.Persistence;
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
        services.AddScoped<IParentConversationQuery, ParentConversationQuery>();
        services.AddScoped<IParentConversationCommand, ParentConversationCommand>();
        services.AddScoped<IParentMessageQuery, ParentMessageQuery>();
        services.AddScoped<IParentMessageCommand, ParentMessageCommand>();
        services.AddScoped<IParentToolExecutionQuery, ParentToolExecutionQuery>();
        services.AddScoped<IParentToolExecutionCommand, ParentToolExecutionCommand>();
        services.AddScoped<IValidator<CreateParentConversation.Request>, CreateParentConversation.Validator>();
        services.AddScoped<IValidator<UpdateParentConversation.Request>, UpdateParentConversation.Validator>();
        services.AddScoped<IValidator<CreateParentMessage.Request>, CreateParentMessage.Validator>();
        services.AddScoped<IValidator<UpdateParentMessage.Request>, UpdateParentMessage.Validator>();
        services.AddScoped<IValidator<CreateParentToolExecution.Request>, CreateParentToolExecution.Validator>();
        services.AddScoped<IValidator<UpdateParentToolExecution.Request>, UpdateParentToolExecution.Validator>();


        services.AddScoped<IRequestHandler<CreateParentConversation.Request, Result<ParentConversationResponse>>, CreateParentConversation.Handler>();
        services.AddScoped<IRequestHandler<GetParentConversationById.Query, Result<ParentConversationResponse>>, GetParentConversationById.Handler>();
        services.AddScoped<IRequestHandler<GetParentConversationPage.Query, Result<PagedResult<ParentConversationResponse>>>, GetParentConversationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateParentConversation.Request, Result<ParentConversationResponse>>, UpdateParentConversation.Handler>();
        services.AddScoped<IRequestHandler<DeleteParentConversation.Command, Result<DeleteParentConversation.Response>>, DeleteParentConversation.Handler>();
        services.AddScoped<IRequestHandler<CreateParentMessage.Request, Result<ParentMessageResponse>>, CreateParentMessage.Handler>();
        services.AddScoped<IRequestHandler<GetParentMessageById.Query, Result<ParentMessageResponse>>, GetParentMessageById.Handler>();
        services.AddScoped<IRequestHandler<GetParentMessagePage.Query, Result<PagedResult<ParentMessageResponse>>>, GetParentMessagePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateParentMessage.Request, Result<ParentMessageResponse>>, UpdateParentMessage.Handler>();
        services.AddScoped<IRequestHandler<DeleteParentMessage.Command, Result<DeleteParentMessage.Response>>, DeleteParentMessage.Handler>();
        services.AddScoped<IRequestHandler<CreateParentToolExecution.Request, Result<ParentToolExecutionResponse>>, CreateParentToolExecution.Handler>();
        services.AddScoped<IRequestHandler<GetParentToolExecutionById.Query, Result<ParentToolExecutionResponse>>, GetParentToolExecutionById.Handler>();
        services.AddScoped<IRequestHandler<GetParentToolExecutionPage.Query, Result<PagedResult<ParentToolExecutionResponse>>>, GetParentToolExecutionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateParentToolExecution.Request, Result<ParentToolExecutionResponse>>, UpdateParentToolExecution.Handler>();
        services.AddScoped<IRequestHandler<DeleteParentToolExecution.Command, Result<DeleteParentToolExecution.Response>>, DeleteParentToolExecution.Handler>();

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
