using SmartSchool.Modules.AICore.Cag;
using SmartSchool.Modules.AICore.Features;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Persistence;

using SmartSchool.Modules.AICore.Features.AiExecutionLog;
using SmartSchool.Modules.AICore.Features.KnowledgeChunk;
using SmartSchool.Modules.AICore.Features.KnowledgeCollection;
using SmartSchool.Modules.AICore.Features.KnowledgeDocument;
using SmartSchool.Modules.AICore.Features.ModelConfiguration;
using SmartSchool.Modules.AICore.Features.PromptTemplate;
using SmartSchool.Modules.AICore.Features.ToolDefinition;
using SmartSchool.Application;

namespace SmartSchool.Modules.AICore;

public static class Module
{
	/// <summary>Registers AICore vertical slices and the shared CAG-first AI services.</summary>
	public static IServiceCollection AddAICoreModule(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

		services.Configure<AiAssistantOptions>(configuration.GetSection(AiAssistantOptions.SectionName));
		services.AddScoped<IOllamaClient, OllamaClient>();
		services.AddScoped<IAiAssistantService, AiAssistantService>();

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

		return services;
	}

	/// <summary>Maps AICore administrative and operational endpoints.</summary>
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

		OperationalAiCoreEndpoints.MapOperationalAiCoreEndpoints(endpoints);

		return endpoints;
	}
}
