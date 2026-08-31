using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface IHRDbContext
{
	DatabaseFacade Database { get; }

	DbSet<CandidateDocumentEntity> CandidateDocuments { get; }
	DbSet<CandidateEntity> Candidates { get; }
	DbSet<EmployeeDocumentEntity> EmployeeDocuments { get; }
	DbSet<EmployeeEducationEntity> EmployeeEducations { get; }
	DbSet<EmployeeEntity> Employees { get; }
	DbSet<EmployeeExperienceEntity> EmployeeExperiences { get; }
	DbSet<EmploymentHistoryEntity> EmploymentHistories { get; }
	DbSet<InterviewEntity> Interviews { get; }
	DbSet<JobEntity> Jobs { get; }
	DbSet<JobGradeEntity> JobGrades { get; }
	DbSet<LeaveRequestEntity> LeaveRequests { get; }
	DbSet<PayrollProfileEntity> PayrollProfiles { get; }
	DbSet<PositionEntity> Positions { get; }
	DbSet<ResumeEntity> Resumes { get; }
	DbSet<TeacherDirectoryReadEntity> TeacherDirectoryReads { get; }
	DbSet<TeacherDocumentEntity> TeacherDocuments { get; }
	DbSet<TeacherProfileEntity> TeacherProfiles { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class HRDbContext(IApplicationDbContext dbContext) : IHRDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<CandidateDocumentEntity> CandidateDocuments => dbContext.Set<CandidateDocumentEntity>();
	public DbSet<CandidateEntity> Candidates => dbContext.Set<CandidateEntity>();
	public DbSet<EmployeeDocumentEntity> EmployeeDocuments => dbContext.Set<EmployeeDocumentEntity>();
	public DbSet<EmployeeEducationEntity> EmployeeEducations => dbContext.Set<EmployeeEducationEntity>();
	public DbSet<EmployeeEntity> Employees => dbContext.Set<EmployeeEntity>();
	public DbSet<EmployeeExperienceEntity> EmployeeExperiences => dbContext.Set<EmployeeExperienceEntity>();
	public DbSet<EmploymentHistoryEntity> EmploymentHistories => dbContext.Set<EmploymentHistoryEntity>();
	public DbSet<InterviewEntity> Interviews => dbContext.Set<InterviewEntity>();
	public DbSet<JobEntity> Jobs => dbContext.Set<JobEntity>();
	public DbSet<JobGradeEntity> JobGrades => dbContext.Set<JobGradeEntity>();
	public DbSet<LeaveRequestEntity> LeaveRequests => dbContext.Set<LeaveRequestEntity>();
	public DbSet<PayrollProfileEntity> PayrollProfiles => dbContext.Set<PayrollProfileEntity>();
	public DbSet<PositionEntity> Positions => dbContext.Set<PositionEntity>();
	public DbSet<ResumeEntity> Resumes => dbContext.Set<ResumeEntity>();
	public DbSet<TeacherDirectoryReadEntity> TeacherDirectoryReads => dbContext.Set<TeacherDirectoryReadEntity>();
	public DbSet<TeacherDocumentEntity> TeacherDocuments => dbContext.Set<TeacherDocumentEntity>();
	public DbSet<TeacherProfileEntity> TeacherProfiles => dbContext.Set<TeacherProfileEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
