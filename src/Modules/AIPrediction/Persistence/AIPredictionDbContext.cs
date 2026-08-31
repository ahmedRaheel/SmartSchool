using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

public interface IAIPredictionDbContext
{
	DatabaseFacade Database { get; }

	DbSet<ClassPerformanceInsightEntity> ClassPerformanceInsights { get; }
	DbSet<MlExamPredictionEntity> MlExamPredictions { get; }
	DbSet<MlPredictionResultEntity> MlPredictionResults { get; }
	DbSet<PredictionEvaluationEntity> PredictionEvaluations { get; }
	DbSet<PredictionEvidenceEntity> PredictionEvidences { get; }
	DbSet<PredictionModelEntity> PredictionModels { get; }
	DbSet<StudentInterventionEntity> StudentInterventions { get; }
	DbSet<StudentPerformancePredictionEntity> StudentPerformancePredictions { get; }
	DbSet<TeachingRecommendationEntity> TeachingRecommendations { get; }
	DbSet<TopicPerformanceInsightEntity> TopicPerformanceInsights { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class AIPredictionDbContext(IApplicationDbContext dbContext) : IAIPredictionDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<ClassPerformanceInsightEntity> ClassPerformanceInsights => dbContext.Set<ClassPerformanceInsightEntity>();
	public DbSet<MlExamPredictionEntity> MlExamPredictions => dbContext.Set<MlExamPredictionEntity>();
	public DbSet<MlPredictionResultEntity> MlPredictionResults => dbContext.Set<MlPredictionResultEntity>();
	public DbSet<PredictionEvaluationEntity> PredictionEvaluations => dbContext.Set<PredictionEvaluationEntity>();
	public DbSet<PredictionEvidenceEntity> PredictionEvidences => dbContext.Set<PredictionEvidenceEntity>();
	public DbSet<PredictionModelEntity> PredictionModels => dbContext.Set<PredictionModelEntity>();
	public DbSet<StudentInterventionEntity> StudentInterventions => dbContext.Set<StudentInterventionEntity>();
	public DbSet<StudentPerformancePredictionEntity> StudentPerformancePredictions => dbContext.Set<StudentPerformancePredictionEntity>();
	public DbSet<TeachingRecommendationEntity> TeachingRecommendations => dbContext.Set<TeachingRecommendationEntity>();
	public DbSet<TopicPerformanceInsightEntity> TopicPerformanceInsights => dbContext.Set<TopicPerformanceInsightEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
