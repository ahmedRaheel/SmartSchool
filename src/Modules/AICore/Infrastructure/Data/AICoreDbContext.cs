using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the AICore module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class AICoreDbContext(DbContextOptions<AICoreDbContext> options)
    : DbContext(options), IAICoreDbContext
{
    public DbSet<AiExecutionLogEntity> AiExecutionLogs => Set<AiExecutionLogEntity>();
    public DbSet<KnowledgeChunkEntity> KnowledgeChunks => Set<KnowledgeChunkEntity>();
    public DbSet<KnowledgeCollectionEntity> KnowledgeCollections => Set<KnowledgeCollectionEntity>();
    public DbSet<KnowledgeDocumentEntity> KnowledgeDocuments => Set<KnowledgeDocumentEntity>();
    public DbSet<ModelConfigurationEntity> ModelConfigurations => Set<ModelConfigurationEntity>();
    public DbSet<PromptTemplateEntity> PromptTemplates => Set<PromptTemplateEntity>();
    public DbSet<ToolDefinitionEntity> ToolDefinitions => Set<ToolDefinitionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AICoreDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.AICore.Persistence.Configurations", StringComparison.Ordinal));
    }
}
