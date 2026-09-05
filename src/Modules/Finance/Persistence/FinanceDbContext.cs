using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the Finance module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class FinanceDbContext(DbContextOptions<FinanceDbContext> options)
    : DbContext(options), IFinanceDbContext
{
    public DbSet<DiscountEntity> Discounts => Set<DiscountEntity>();
    public DbSet<EmployeeCompensationEntity> EmployeeCompensations => Set<EmployeeCompensationEntity>();
    public DbSet<FeeStructureEntity> FeeStructures => Set<FeeStructureEntity>();
    public DbSet<FeeTypeEntity> FeeTypes => Set<FeeTypeEntity>();
    public DbSet<IncrementEntity> Increments => Set<IncrementEntity>();
    public DbSet<InvoiceEntity> Invoices => Set<InvoiceEntity>();
    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();
    public DbSet<PayrollRunEntity> PayrollRuns => Set<PayrollRunEntity>();
    public DbSet<PayslipEntity> Payslips => Set<PayslipEntity>();
    public DbSet<SalaryStructureEntity> SalaryStructures => Set<SalaryStructureEntity>();
    public DbSet<ScholarshipEntity> Scholarships => Set<ScholarshipEntity>();
    public DbSet<StudentFeeEntity> StudentFees => Set<StudentFeeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FinanceDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Finance.Persistence.Configurations", StringComparison.Ordinal));
    }
}
