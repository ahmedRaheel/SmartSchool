using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
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
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class AIParentDbContext(IApplicationDbContext dbContext) : IAIParentDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<ParentConversationEntity> ParentConversations => dbContext.Set<ParentConversationEntity>();
	public DbSet<ParentMessageEntity> ParentMessages => dbContext.Set<ParentMessageEntity>();
	public DbSet<ParentToolExecutionEntity> ParentToolExecutions => dbContext.Set<ParentToolExecutionEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
