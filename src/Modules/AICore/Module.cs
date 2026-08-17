using SmartSchool.Modules.AICore.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Persistence;
using FluentValidation;
using SmartSchool.Modules.AICore.Features.AiExecutionLog;
using SmartSchool.Modules.AICore.Features.KnowledgeChunk;
using SmartSchool.Modules.AICore.Features.KnowledgeCollection;
using SmartSchool.Modules.AICore.Features.KnowledgeDocument;
using SmartSchool.Modules.AICore.Features.ModelConfiguration;
using SmartSchool.Modules.AICore.Features.PromptTemplate;
using SmartSchool.Modules.AICore.Features.ToolDefinition;

namespace SmartSchool.Modules.AICore;

public static class Module
{
    public static IServiceCollection AddAICoreModule(
        this IServiceCollection services)
    {
        services.AddScoped<IAiExecutionLogQuery, AiExecutionLogQuery>();
        services.AddScoped<IAiExecutionLogCommand, AiExecutionLogCommand>();
        services.AddScoped<IKnowledgeChunkQuery, KnowledgeChunkQuery>();
        services.AddScoped<IKnowledgeChunkCommand, KnowledgeChunkCommand>();
        services.AddScoped<IKnowledgeCollectionQuery, KnowledgeCollectionQuery>();
        services.AddScoped<IKnowledgeCollectionCommand, KnowledgeCollectionCommand>();
        services.AddScoped<IKnowledgeDocumentQuery, KnowledgeDocumentQuery>();
        services.AddScoped<IKnowledgeDocumentCommand, KnowledgeDocumentCommand>();
        services.AddScoped<IModelConfigurationQuery, ModelConfigurationQuery>();
        services.AddScoped<IModelConfigurationCommand, ModelConfigurationCommand>();
        services.AddScoped<IPromptTemplateQuery, PromptTemplateQuery>();
        services.AddScoped<IPromptTemplateCommand, PromptTemplateCommand>();
        services.AddScoped<IToolDefinitionQuery, ToolDefinitionQuery>();
        services.AddScoped<IToolDefinitionCommand, ToolDefinitionCommand>();
        services.AddScoped<IValidator<CreateAiExecutionLog.Request>, CreateAiExecutionLog.Validator>();
        services.AddScoped<IValidator<UpdateAiExecutionLog.Request>, UpdateAiExecutionLog.Validator>();
        services.AddScoped<IValidator<CreateKnowledgeChunk.Request>, CreateKnowledgeChunk.Validator>();
        services.AddScoped<IValidator<UpdateKnowledgeChunk.Request>, UpdateKnowledgeChunk.Validator>();
        services.AddScoped<IValidator<CreateKnowledgeCollection.Request>, CreateKnowledgeCollection.Validator>();
        services.AddScoped<IValidator<UpdateKnowledgeCollection.Request>, UpdateKnowledgeCollection.Validator>();
        services.AddScoped<IValidator<CreateKnowledgeDocument.Request>, CreateKnowledgeDocument.Validator>();
        services.AddScoped<IValidator<UpdateKnowledgeDocument.Request>, UpdateKnowledgeDocument.Validator>();
        services.AddScoped<IValidator<CreateModelConfiguration.Request>, CreateModelConfiguration.Validator>();
        services.AddScoped<IValidator<UpdateModelConfiguration.Request>, UpdateModelConfiguration.Validator>();
        services.AddScoped<IValidator<CreatePromptTemplate.Request>, CreatePromptTemplate.Validator>();
        services.AddScoped<IValidator<UpdatePromptTemplate.Request>, UpdatePromptTemplate.Validator>();
        services.AddScoped<IValidator<CreateToolDefinition.Request>, CreateToolDefinition.Validator>();
        services.AddScoped<IValidator<UpdateToolDefinition.Request>, UpdateToolDefinition.Validator>();


        services.AddScoped<IRequestHandler<CreateAiExecutionLog.Request, Result<AiExecutionLogResponse>>, CreateAiExecutionLog.Handler>();
        services.AddScoped<IRequestHandler<GetAiExecutionLogById.Query, Result<AiExecutionLogResponse>>, GetAiExecutionLogById.Handler>();
        services.AddScoped<IRequestHandler<GetAiExecutionLogPage.Query, Result<PagedResult<AiExecutionLogResponse>>>, GetAiExecutionLogPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateAiExecutionLog.Request, Result<AiExecutionLogResponse>>, UpdateAiExecutionLog.Handler>();
        services.AddScoped<IRequestHandler<DeleteAiExecutionLog.Command, Result<DeleteAiExecutionLog.Response>>, DeleteAiExecutionLog.Handler>();
        services.AddScoped<IRequestHandler<CreateKnowledgeChunk.Request, Result<KnowledgeChunkResponse>>, CreateKnowledgeChunk.Handler>();
        services.AddScoped<IRequestHandler<GetKnowledgeChunkById.Query, Result<KnowledgeChunkResponse>>, GetKnowledgeChunkById.Handler>();
        services.AddScoped<IRequestHandler<GetKnowledgeChunkPage.Query, Result<PagedResult<KnowledgeChunkResponse>>>, GetKnowledgeChunkPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateKnowledgeChunk.Request, Result<KnowledgeChunkResponse>>, UpdateKnowledgeChunk.Handler>();
        services.AddScoped<IRequestHandler<DeleteKnowledgeChunk.Command, Result<DeleteKnowledgeChunk.Response>>, DeleteKnowledgeChunk.Handler>();
        services.AddScoped<IRequestHandler<CreateKnowledgeCollection.Request, Result<KnowledgeCollectionResponse>>, CreateKnowledgeCollection.Handler>();
        services.AddScoped<IRequestHandler<GetKnowledgeCollectionById.Query, Result<KnowledgeCollectionResponse>>, GetKnowledgeCollectionById.Handler>();
        services.AddScoped<IRequestHandler<GetKnowledgeCollectionPage.Query, Result<PagedResult<KnowledgeCollectionResponse>>>, GetKnowledgeCollectionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateKnowledgeCollection.Request, Result<KnowledgeCollectionResponse>>, UpdateKnowledgeCollection.Handler>();
        services.AddScoped<IRequestHandler<DeleteKnowledgeCollection.Command, Result<DeleteKnowledgeCollection.Response>>, DeleteKnowledgeCollection.Handler>();
        services.AddScoped<IRequestHandler<CreateKnowledgeDocument.Request, Result<KnowledgeDocumentResponse>>, CreateKnowledgeDocument.Handler>();
        services.AddScoped<IRequestHandler<GetKnowledgeDocumentById.Query, Result<KnowledgeDocumentResponse>>, GetKnowledgeDocumentById.Handler>();
        services.AddScoped<IRequestHandler<GetKnowledgeDocumentPage.Query, Result<PagedResult<KnowledgeDocumentResponse>>>, GetKnowledgeDocumentPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateKnowledgeDocument.Request, Result<KnowledgeDocumentResponse>>, UpdateKnowledgeDocument.Handler>();
        services.AddScoped<IRequestHandler<DeleteKnowledgeDocument.Command, Result<DeleteKnowledgeDocument.Response>>, DeleteKnowledgeDocument.Handler>();
        services.AddScoped<IRequestHandler<CreateModelConfiguration.Request, Result<ModelConfigurationResponse>>, CreateModelConfiguration.Handler>();
        services.AddScoped<IRequestHandler<GetModelConfigurationById.Query, Result<ModelConfigurationResponse>>, GetModelConfigurationById.Handler>();
        services.AddScoped<IRequestHandler<GetModelConfigurationPage.Query, Result<PagedResult<ModelConfigurationResponse>>>, GetModelConfigurationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateModelConfiguration.Request, Result<ModelConfigurationResponse>>, UpdateModelConfiguration.Handler>();
        services.AddScoped<IRequestHandler<DeleteModelConfiguration.Command, Result<DeleteModelConfiguration.Response>>, DeleteModelConfiguration.Handler>();
        services.AddScoped<IRequestHandler<CreatePromptTemplate.Request, Result<PromptTemplateResponse>>, CreatePromptTemplate.Handler>();
        services.AddScoped<IRequestHandler<GetPromptTemplateById.Query, Result<PromptTemplateResponse>>, GetPromptTemplateById.Handler>();
        services.AddScoped<IRequestHandler<GetPromptTemplatePage.Query, Result<PagedResult<PromptTemplateResponse>>>, GetPromptTemplatePage.Handler>();
        services.AddScoped<IRequestHandler<UpdatePromptTemplate.Request, Result<PromptTemplateResponse>>, UpdatePromptTemplate.Handler>();
        services.AddScoped<IRequestHandler<DeletePromptTemplate.Command, Result<DeletePromptTemplate.Response>>, DeletePromptTemplate.Handler>();
        services.AddScoped<IRequestHandler<CreateToolDefinition.Request, Result<ToolDefinitionResponse>>, CreateToolDefinition.Handler>();
        services.AddScoped<IRequestHandler<GetToolDefinitionById.Query, Result<ToolDefinitionResponse>>, GetToolDefinitionById.Handler>();
        services.AddScoped<IRequestHandler<GetToolDefinitionPage.Query, Result<PagedResult<ToolDefinitionResponse>>>, GetToolDefinitionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateToolDefinition.Request, Result<ToolDefinitionResponse>>, UpdateToolDefinition.Handler>();
        services.AddScoped<IRequestHandler<DeleteToolDefinition.Command, Result<DeleteToolDefinition.Response>>, DeleteToolDefinition.Handler>();

        return services;
    }

    public static IEndpointRouteBuilder MapAICoreEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateAiExecutionLog.MapEndpoint(endpoints);
        GetAiExecutionLogById.MapEndpoint(endpoints);
        GetAiExecutionLogPage.MapEndpoint(endpoints);
        UpdateAiExecutionLog.MapEndpoint(endpoints);
        DeleteAiExecutionLog.MapEndpoint(endpoints);
        CreateKnowledgeChunk.MapEndpoint(endpoints);
        GetKnowledgeChunkById.MapEndpoint(endpoints);
        GetKnowledgeChunkPage.MapEndpoint(endpoints);
        UpdateKnowledgeChunk.MapEndpoint(endpoints);
        DeleteKnowledgeChunk.MapEndpoint(endpoints);
        CreateKnowledgeCollection.MapEndpoint(endpoints);
        GetKnowledgeCollectionById.MapEndpoint(endpoints);
        GetKnowledgeCollectionPage.MapEndpoint(endpoints);
        UpdateKnowledgeCollection.MapEndpoint(endpoints);
        DeleteKnowledgeCollection.MapEndpoint(endpoints);
        CreateKnowledgeDocument.MapEndpoint(endpoints);
        GetKnowledgeDocumentById.MapEndpoint(endpoints);
        GetKnowledgeDocumentPage.MapEndpoint(endpoints);
        UpdateKnowledgeDocument.MapEndpoint(endpoints);
        DeleteKnowledgeDocument.MapEndpoint(endpoints);
        CreateModelConfiguration.MapEndpoint(endpoints);
        GetModelConfigurationById.MapEndpoint(endpoints);
        GetModelConfigurationPage.MapEndpoint(endpoints);
        UpdateModelConfiguration.MapEndpoint(endpoints);
        DeleteModelConfiguration.MapEndpoint(endpoints);
        CreatePromptTemplate.MapEndpoint(endpoints);
        GetPromptTemplateById.MapEndpoint(endpoints);
        GetPromptTemplatePage.MapEndpoint(endpoints);
        UpdatePromptTemplate.MapEndpoint(endpoints);
        DeletePromptTemplate.MapEndpoint(endpoints);
        CreateToolDefinition.MapEndpoint(endpoints);
        GetToolDefinitionById.MapEndpoint(endpoints);
        GetToolDefinitionPage.MapEndpoint(endpoints);
        UpdateToolDefinition.MapEndpoint(endpoints);
        DeleteToolDefinition.MapEndpoint(endpoints);

        return endpoints;
    }
}
