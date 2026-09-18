using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence.Configurations;

public sealed class EmployeeProjectionEntityConfiguration : IEntityTypeConfiguration<EmployeeProjectionEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeProjectionEntity> builder)
    {
        builder.ToTable("employee_projection", "payroll");
        builder.HasKey(x => x.EmployeeId);
        builder.Property(x => x.EmployeeId).HasColumnName("employee_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(x => x.BranchId).HasColumnName("branch_id").IsRequired();
        builder.Property(x => x.EmployeeNumber).HasColumnName("employee_number").HasMaxLength(60);
        builder.Property(x => x.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasColumnName("last_name").HasMaxLength(100);
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.EmployeeId }).IsUnique();
    }
}
