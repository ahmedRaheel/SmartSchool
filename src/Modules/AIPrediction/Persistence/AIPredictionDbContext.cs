using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the AIPrediction module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class AIPredictionDbContext(DbContextOptions<AIPredictionDbContext> options)
    : DbContext(options), IAIPredictionDbContext
{
    public DbSet<ClassPerformanceInsightEntity> ClassPerformanceInsights => Set<ClassPerformanceInsightEntity>();
    public DbSet<MlExamPredictionEntity> MlExamPredictions => Set<MlExamPredictionEntity>();
    public DbSet<MlPredictionResultEntity> MlPredictionResults => Set<MlPredictionResultEntity>();
    public DbSet<PredictionEvaluationEntity> PredictionEvaluations => Set<PredictionEvaluationEntity>();
    public DbSet<PredictionEvidenceEntity> PredictionEvidences => Set<PredictionEvidenceEntity>();
    public DbSet<PredictionModelEntity> PredictionModels => Set<PredictionModelEntity>();
    public DbSet<StudentInterventionEntity> StudentInterventions => Set<StudentInterventionEntity>();
    public DbSet<StudentPerformancePredictionEntity> StudentPerformancePredictions => Set<StudentPerformancePredictionEntity>();
    public DbSet<TeachingRecommendationEntity> TeachingRecommendations => Set<TeachingRecommendationEntity>();
    public DbSet<TopicPerformanceInsightEntity> TopicPerformanceInsights => Set<TopicPerformanceInsightEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AIPredictionDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.AIPrediction.Persistence.Configurations", StringComparison.Ordinal));
    }
}
