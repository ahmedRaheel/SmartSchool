using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="PayrollProfileEntity"/>.
/// </summary>
public sealed class PayrollProfileEntityConfiguration
    : IEntityTypeConfiguration<PayrollProfileEntity>
{
    public void Configure(EntityTypeBuilder<PayrollProfileEntity> builder)
    {
        builder.ToTable("PayrollProfile", SmartSchool.Modules.HR.ModuleConstants.Schema);

        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version");
        builder.HasKey(entity => entity.PayrollProfileId);

        builder
            .Property(entity => entity.TenantId)
            .IsRequired();

        builder
            .Property(entity => entity.IsActive)
            .IsRequired();

        builder
            .Property(entity => entity.RowVersion)
            .IsConcurrencyToken();

        builder.HasIndex(entity => entity.TenantId);

    }
}
