using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the Payroll module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class PayrollDbContext(DbContextOptions<PayrollDbContext> options)
	: DbContext(options), IPayrollDbContext
{
	public DbSet<EmployeeCompensationEntity> EmployeeCompensations => Set<EmployeeCompensationEntity>();
	public DbSet<IncrementEntity> Increments => Set<IncrementEntity>();
	public DbSet<PayrollRunEntity> PayrollRuns => Set<PayrollRunEntity>();
	public DbSet<PayslipEntity> Payslips => Set<PayslipEntity>();
	public DbSet<SalaryStructureEntity> SalaryStructures => Set<SalaryStructureEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(PayrollDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.Payroll.Persistence.Configurations", StringComparison.Ordinal));
	}
}
