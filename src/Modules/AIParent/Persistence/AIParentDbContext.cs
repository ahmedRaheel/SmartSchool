using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

public interface IAIParentDbContext
{
	DatabaseFacade Database { get; }

	DbSet<ParentConversationEntity> ParentConversations { get; }
	DbSet<ParentMessageEntity> ParentMessages { get; }
	DbSet<ParentToolExecutionEntity> ParentToolExecutions { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the AIParent module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class AIParentDbContext(DbContextOptions<AIParentDbContext> options)
	: DbContext(options), IAIParentDbContext
{
	public DbSet<ParentConversationEntity> ParentConversations => Set<ParentConversationEntity>();
	public DbSet<ParentMessageEntity> ParentMessages => Set<ParentMessageEntity>();
	public DbSet<ParentToolExecutionEntity> ParentToolExecutions => Set<ParentToolExecutionEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(AIParentDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.AIParent.Persistence.Configurations", StringComparison.Ordinal));
	}
}
