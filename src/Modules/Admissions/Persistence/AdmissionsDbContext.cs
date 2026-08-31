using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

public interface IAdmissionsDbContext
{
	DatabaseFacade Database { get; }

	DbSet<AdmissionDecisionEntity> AdmissionDecisions { get; }
	DbSet<ApplicantEntity> Applicants { get; }
	DbSet<ApplicationEntity> Applications { get; }
	DbSet<InquiryEntity> Inquiries { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class AdmissionsDbContext(IApplicationDbContext dbContext) : IAdmissionsDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<AdmissionDecisionEntity> AdmissionDecisions => dbContext.Set<AdmissionDecisionEntity>();
	public DbSet<ApplicantEntity> Applicants => dbContext.Set<ApplicantEntity>();
	public DbSet<ApplicationEntity> Applications => dbContext.Set<ApplicationEntity>();
	public DbSet<InquiryEntity> Inquiries => dbContext.Set<InquiryEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
