using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

public interface ILearningDbContext
{
	DatabaseFacade Database { get; }

	DbSet<AssignmentEntity> Assignments { get; }
	DbSet<AssignmentSubmissionEntity> AssignmentSubmissions { get; }
	DbSet<LearningResourceEntity> LearningResources { get; }
	DbSet<LessonEntity> Lessons { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the Learning module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class LearningDbContext(DbContextOptions<LearningDbContext> options)
	: DbContext(options), ILearningDbContext
{
	public DbSet<AssignmentEntity> Assignments => Set<AssignmentEntity>();
	public DbSet<AssignmentSubmissionEntity> AssignmentSubmissions => Set<AssignmentSubmissionEntity>();
	public DbSet<LearningResourceEntity> LearningResources => Set<LearningResourceEntity>();
	public DbSet<LessonEntity> Lessons => Set<LessonEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(LearningDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.Learning.Persistence.Configurations", StringComparison.Ordinal));
	}
}
