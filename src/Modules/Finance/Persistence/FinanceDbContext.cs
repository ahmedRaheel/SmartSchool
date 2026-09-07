using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

public interface IFinanceDbContext
{
    DatabaseFacade Database { get; }

    DbSet<DiscountEntity> Discounts { get; }
    DbSet<FeeStructureEntity> FeeStructures { get; }
    DbSet<FeeTypeEntity> FeeTypes { get; }
    DbSet<InvoiceEntity> Invoices { get; }
    DbSet<PaymentEntity> Payments { get; }
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
    public DbSet<FeeStructureEntity> FeeStructures => Set<FeeStructureEntity>();
    public DbSet<FeeTypeEntity> FeeTypes => Set<FeeTypeEntity>();
    public DbSet<InvoiceEntity> Invoices => Set<InvoiceEntity>();
    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();
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
