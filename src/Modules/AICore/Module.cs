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
        services.AddScoped<CreateAiExecutionLog.Handler>();
        services.AddScoped<GetAiExecutionLogById.Handler>();
        services.AddScoped<GetAiExecutionLogPage.Handler>();
        services.AddScoped<UpdateAiExecutionLog.Handler>();
        services.AddScoped<DeleteAiExecutionLog.Handler>();
        services.AddScoped<IValidator<CreateAiExecutionLog.Request>, CreateAiExecutionLog.Validator>();
        services.AddScoped<IValidator<UpdateAiExecutionLog.Request>, UpdateAiExecutionLog.Validator>();
        services.AddScoped<CreateKnowledgeChunk.Handler>();
        services.AddScoped<GetKnowledgeChunkById.Handler>();
        services.AddScoped<GetKnowledgeChunkPage.Handler>();
        services.AddScoped<UpdateKnowledgeChunk.Handler>();
        services.AddScoped<DeleteKnowledgeChunk.Handler>();
        services.AddScoped<IValidator<CreateKnowledgeChunk.Request>, CreateKnowledgeChunk.Validator>();
        services.AddScoped<IValidator<UpdateKnowledgeChunk.Request>, UpdateKnowledgeChunk.Validator>();
        services.AddScoped<CreateKnowledgeCollection.Handler>();
        services.AddScoped<GetKnowledgeCollectionById.Handler>();
        services.AddScoped<GetKnowledgeCollectionPage.Handler>();
        services.AddScoped<UpdateKnowledgeCollection.Handler>();
        services.AddScoped<DeleteKnowledgeCollection.Handler>();
        services.AddScoped<IValidator<CreateKnowledgeCollection.Request>, CreateKnowledgeCollection.Validator>();
        services.AddScoped<IValidator<UpdateKnowledgeCollection.Request>, UpdateKnowledgeCollection.Validator>();
        services.AddScoped<CreateKnowledgeDocument.Handler>();
        services.AddScoped<GetKnowledgeDocumentById.Handler>();
        services.AddScoped<GetKnowledgeDocumentPage.Handler>();
        services.AddScoped<UpdateKnowledgeDocument.Handler>();
        services.AddScoped<DeleteKnowledgeDocument.Handler>();
        services.AddScoped<IValidator<CreateKnowledgeDocument.Request>, CreateKnowledgeDocument.Validator>();
        services.AddScoped<IValidator<UpdateKnowledgeDocument.Request>, UpdateKnowledgeDocument.Validator>();
        services.AddScoped<CreateModelConfiguration.Handler>();
        services.AddScoped<GetModelConfigurationById.Handler>();
        services.AddScoped<GetModelConfigurationPage.Handler>();
        services.AddScoped<UpdateModelConfiguration.Handler>();
        services.AddScoped<DeleteModelConfiguration.Handler>();
        services.AddScoped<IValidator<CreateModelConfiguration.Request>, CreateModelConfiguration.Validator>();
        services.AddScoped<IValidator<UpdateModelConfiguration.Request>, UpdateModelConfiguration.Validator>();
        services.AddScoped<CreatePromptTemplate.Handler>();
        services.AddScoped<GetPromptTemplateById.Handler>();
        services.AddScoped<GetPromptTemplatePage.Handler>();
        services.AddScoped<UpdatePromptTemplate.Handler>();
        services.AddScoped<DeletePromptTemplate.Handler>();
        services.AddScoped<IValidator<CreatePromptTemplate.Request>, CreatePromptTemplate.Validator>();
        services.AddScoped<IValidator<UpdatePromptTemplate.Request>, UpdatePromptTemplate.Validator>();
        services.AddScoped<CreateToolDefinition.Handler>();
        services.AddScoped<GetToolDefinitionById.Handler>();
        services.AddScoped<GetToolDefinitionPage.Handler>();
        services.AddScoped<UpdateToolDefinition.Handler>();
        services.AddScoped<DeleteToolDefinition.Handler>();
        services.AddScoped<IValidator<CreateToolDefinition.Request>, CreateToolDefinition.Validator>();
        services.AddScoped<IValidator<UpdateToolDefinition.Request>, UpdateToolDefinition.Validator>();

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
