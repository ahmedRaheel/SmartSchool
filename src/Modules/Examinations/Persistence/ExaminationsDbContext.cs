using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the Examinations module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class ExaminationsDbContext(DbContextOptions<ExaminationsDbContext> options)
    : DbContext(options), IExaminationsDbContext
{
    public DbSet<ExamEntity> Exams => Set<ExamEntity>();
    public DbSet<ExamSubjectEntity> ExamSubjects => Set<ExamSubjectEntity>();
    public DbSet<GradeScaleEntity> GradeScales => Set<GradeScaleEntity>();
    public DbSet<StudentExamResultEntity> StudentExamResults => Set<StudentExamResultEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ExaminationsDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Examinations.Persistence.Configurations", StringComparison.Ordinal));
    }
}
