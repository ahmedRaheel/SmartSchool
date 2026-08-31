using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Finance.Persistence;

public interface IFinanceDbContext
{
	DatabaseFacade Database { get; }

	DbSet<DiscountEntity> Discounts { get; }
	DbSet<EmployeeCompensationEntity> EmployeeCompensations { get; }
	DbSet<FeeStructureEntity> FeeStructures { get; }
	DbSet<FeeTypeEntity> FeeTypes { get; }
	DbSet<IncrementEntity> Increments { get; }
	DbSet<InvoiceEntity> Invoices { get; }
	DbSet<PaymentEntity> Payments { get; }
	DbSet<PayrollRunEntity> PayrollRuns { get; }
	DbSet<PayslipEntity> Payslips { get; }
	DbSet<SalaryStructureEntity> SalaryStructures { get; }
	DbSet<ScholarshipEntity> Scholarships { get; }
	DbSet<StudentFeeEntity> StudentFees { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class FinanceDbContext(IApplicationDbContext dbContext) : IFinanceDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<DiscountEntity> Discounts => dbContext.Set<DiscountEntity>();
	public DbSet<EmployeeCompensationEntity> EmployeeCompensations => dbContext.Set<EmployeeCompensationEntity>();
	public DbSet<FeeStructureEntity> FeeStructures => dbContext.Set<FeeStructureEntity>();
	public DbSet<FeeTypeEntity> FeeTypes => dbContext.Set<FeeTypeEntity>();
	public DbSet<IncrementEntity> Increments => dbContext.Set<IncrementEntity>();
	public DbSet<InvoiceEntity> Invoices => dbContext.Set<InvoiceEntity>();
	public DbSet<PaymentEntity> Payments => dbContext.Set<PaymentEntity>();
	public DbSet<PayrollRunEntity> PayrollRuns => dbContext.Set<PayrollRunEntity>();
	public DbSet<PayslipEntity> Payslips => dbContext.Set<PayslipEntity>();
	public DbSet<SalaryStructureEntity> SalaryStructures => dbContext.Set<SalaryStructureEntity>();
	public DbSet<ScholarshipEntity> Scholarships => dbContext.Set<ScholarshipEntity>();
	public DbSet<StudentFeeEntity> StudentFees => dbContext.Set<StudentFeeEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
