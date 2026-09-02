using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence;

public interface IExaminationsDbContext
{
	DatabaseFacade Database { get; }

	DbSet<ExamEntity> Exams { get; }
	DbSet<ExamSubjectEntity> ExamSubjects { get; }
	DbSet<GradeScaleEntity> GradeScales { get; }
	DbSet<StudentExamResultEntity> StudentExamResults { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class ExaminationsDbContext(IApplicationDbContext dbContext) : IExaminationsDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<ExamEntity> Exams => dbContext.Set<ExamEntity>();
	public DbSet<ExamSubjectEntity> ExamSubjects => dbContext.Set<ExamSubjectEntity>();
	public DbSet<GradeScaleEntity> GradeScales => dbContext.Set<GradeScaleEntity>();
	public DbSet<StudentExamResultEntity> StudentExamResults => dbContext.Set<StudentExamResultEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
