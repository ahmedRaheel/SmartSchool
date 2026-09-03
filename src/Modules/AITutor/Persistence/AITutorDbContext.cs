using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the AITutor module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class AITutorDbContext(DbContextOptions<AITutorDbContext> options)
	: DbContext(options), IAITutorDbContext
{
	public DbSet<GeneratedQuizEntity> GeneratedQuizs => Set<GeneratedQuizEntity>();
	public DbSet<LearningRecommendationEntity> LearningRecommendations => Set<LearningRecommendationEntity>();
	public DbSet<QuizAttemptEntity> QuizAttempts => Set<QuizAttemptEntity>();
	public DbSet<StudentTopicMasteryEntity> StudentTopicMasteries => Set<StudentTopicMasteryEntity>();
	public DbSet<TutorConversationEntity> TutorConversations => Set<TutorConversationEntity>();
	public DbSet<TutorMessageEntity> TutorMessages => Set<TutorMessageEntity>();
	public DbSet<TutorSessionEntity> TutorSessions => Set<TutorSessionEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(AITutorDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.AITutor.Persistence.Configurations", StringComparison.Ordinal));
	}
}
