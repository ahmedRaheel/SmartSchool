using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
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
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class LearningDbContext(IApplicationDbContext dbContext) : ILearningDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<AssignmentEntity> Assignments => dbContext.Set<AssignmentEntity>();
	public DbSet<AssignmentSubmissionEntity> AssignmentSubmissions => dbContext.Set<AssignmentSubmissionEntity>();
	public DbSet<LearningResourceEntity> LearningResources => dbContext.Set<LearningResourceEntity>();
	public DbSet<LessonEntity> Lessons => dbContext.Set<LessonEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
