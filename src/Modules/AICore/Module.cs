using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.Modules.AICore.Cag;
using SmartSchool.Modules.AICore.Rag;
using SmartSchool.Modules.AICore.Rag.Ollama;
using SmartSchool.Modules.AICore.Agents;
using ModelContextProtocol.Server;
using SmartSchool.Modules.AICore.Features;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application.AI;

using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;

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
        services.AddScoped<IAICoreDbContext>(serviceProvider =>
            serviceProvider.GetRequiredService<AICoreDbContext>());

        services.AddFeaturePersistence(typeof(Module).Assembly);
        services.AddScoped<AgentWorkflowServiceAiExecutionLogCommand>();
        
        services.AddScoped<SmartSchoolAgentToolsStudentExamResultQuery>();
        services.AddScoped<SmartSchoolAgentToolsStudentPerformancePredictionQuery>();
        services.AddScoped<SmartSchoolAgentToolsStudentQuery>();

        services.Configure<AiAssistantOptions>(configuration.GetSection(AiAssistantOptions.SectionName));
        services.AddOptions<OllamaRagOptions>()
            .Bind(configuration.GetSection(OllamaRagOptions.SectionName))
            .Validate(options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _), "AI:Ollama:BaseUrl must be an absolute URI.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.ChatModel), "AI:Ollama:ChatModel is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.EmbeddingModel), "AI:Ollama:EmbeddingModel is required.")
            .ValidateOnStart();
        services.AddScoped<IOllamaClient, OllamaClient>();
        services.AddScoped<IAiAssistantService, AiAssistantService>();
        services.AddScoped<IRagWorkflowStep, RagGuardrailStep>();
        services.AddScoped<IRagWorkflowStep, FastModelPromptStep>();
        services.AddScoped<LangChainRagWorkflow>();
        services.AddScoped<SmartSchoolAgentTools>();
        services.AddScoped<IAgentWorkflowService, AgentWorkflowService>();
        services
            .AddMcpServer()
            .WithHttpTransport()
            .WithTools<SmartSchoolAgentTools>();
        return services;
    }

    /// <summary>Maps AICore administrative and operational endpoints.</summary>
    public static IEndpointRouteBuilder MapAICoreEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateAiExecutionLog.MapEndpoint(endpoints);
        GetAiExecutionLogById.MapEndpoint(endpoints);
        GetAiExecutionLogByModelConfigurationId.MapEndpoint(endpoints);
        GetAiExecutionLogPage.MapEndpoint(endpoints);
        UpdateAiExecutionLog.MapEndpoint(endpoints);
        DeleteAiExecutionLog.MapEndpoint(endpoints);
        CreateKnowledgeChunk.MapEndpoint(endpoints);
        GetKnowledgeChunkById.MapEndpoint(endpoints);
        GetKnowledgeChunkByKnowledgeDocumentId.MapEndpoint(endpoints);
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
        GetKnowledgeDocumentByKnowledgeCollectionId.MapEndpoint(endpoints);
        GetKnowledgeDocumentByCampusId.MapEndpoint(endpoints);
        GetKnowledgeDocumentByAcademicSystemId.MapEndpoint(endpoints);
        GetKnowledgeDocumentPage.MapEndpoint(endpoints);
        UpdateKnowledgeDocument.MapEndpoint(endpoints);
        DeleteKnowledgeDocument.MapEndpoint(endpoints);
        UploadKnowledgePdf.MapEndpoint(endpoints);
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

       
        endpoints.MapAgentEndpoints();

        return endpoints;
    }
}
