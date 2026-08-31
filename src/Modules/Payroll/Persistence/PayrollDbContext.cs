using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

public interface IPayrollDbContext
{
	DatabaseFacade Database { get; }

	DbSet<EmployeeCompensationEntity> EmployeeCompensations { get; }
	DbSet<IncrementEntity> Increments { get; }
	DbSet<PayrollRunEntity> PayrollRuns { get; }
	DbSet<PayslipEntity> Payslips { get; }
	DbSet<SalaryStructureEntity> SalaryStructures { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class PayrollDbContext(IApplicationDbContext dbContext) : IPayrollDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<EmployeeCompensationEntity> EmployeeCompensations => dbContext.Set<EmployeeCompensationEntity>();
	public DbSet<IncrementEntity> Increments => dbContext.Set<IncrementEntity>();
	public DbSet<PayrollRunEntity> PayrollRuns => dbContext.Set<PayrollRunEntity>();
	public DbSet<PayslipEntity> Payslips => dbContext.Set<PayslipEntity>();
	public DbSet<SalaryStructureEntity> SalaryStructures => dbContext.Set<SalaryStructureEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
