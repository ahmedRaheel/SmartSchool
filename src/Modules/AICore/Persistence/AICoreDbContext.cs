using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

public interface IAICoreDbContext
{
	DatabaseFacade Database { get; }

	DbSet<AiExecutionLogEntity> AiExecutionLogs { get; }
	DbSet<KnowledgeChunkEntity> KnowledgeChunks { get; }
	DbSet<KnowledgeCollectionEntity> KnowledgeCollections { get; }
	DbSet<KnowledgeDocumentEntity> KnowledgeDocuments { get; }
	DbSet<ModelConfigurationEntity> ModelConfigurations { get; }
	DbSet<PromptTemplateEntity> PromptTemplates { get; }
	DbSet<ToolDefinitionEntity> ToolDefinitions { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class AICoreDbContext(IApplicationDbContext dbContext) : IAICoreDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<AiExecutionLogEntity> AiExecutionLogs => dbContext.Set<AiExecutionLogEntity>();
	public DbSet<KnowledgeChunkEntity> KnowledgeChunks => dbContext.Set<KnowledgeChunkEntity>();
	public DbSet<KnowledgeCollectionEntity> KnowledgeCollections => dbContext.Set<KnowledgeCollectionEntity>();
	public DbSet<KnowledgeDocumentEntity> KnowledgeDocuments => dbContext.Set<KnowledgeDocumentEntity>();
	public DbSet<ModelConfigurationEntity> ModelConfigurations => dbContext.Set<ModelConfigurationEntity>();
	public DbSet<PromptTemplateEntity> PromptTemplates => dbContext.Set<PromptTemplateEntity>();
	public DbSet<ToolDefinitionEntity> ToolDefinitions => dbContext.Set<ToolDefinitionEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
