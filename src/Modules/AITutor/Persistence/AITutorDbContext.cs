using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

public interface IAITutorDbContext
{
	DatabaseFacade Database { get; }

	DbSet<GeneratedQuizEntity> GeneratedQuizs { get; }
	DbSet<LearningRecommendationEntity> LearningRecommendations { get; }
	DbSet<QuizAttemptEntity> QuizAttempts { get; }
	DbSet<StudentTopicMasteryEntity> StudentTopicMasteries { get; }
	DbSet<TutorConversationEntity> TutorConversations { get; }
	DbSet<TutorMessageEntity> TutorMessages { get; }
	DbSet<TutorSessionEntity> TutorSessions { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class AITutorDbContext(IApplicationDbContext dbContext) : IAITutorDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<GeneratedQuizEntity> GeneratedQuizs => dbContext.Set<GeneratedQuizEntity>();
	public DbSet<LearningRecommendationEntity> LearningRecommendations => dbContext.Set<LearningRecommendationEntity>();
	public DbSet<QuizAttemptEntity> QuizAttempts => dbContext.Set<QuizAttemptEntity>();
	public DbSet<StudentTopicMasteryEntity> StudentTopicMasteries => dbContext.Set<StudentTopicMasteryEntity>();
	public DbSet<TutorConversationEntity> TutorConversations => dbContext.Set<TutorConversationEntity>();
	public DbSet<TutorMessageEntity> TutorMessages => dbContext.Set<TutorMessageEntity>();
	public DbSet<TutorSessionEntity> TutorSessions => dbContext.Set<TutorSessionEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
