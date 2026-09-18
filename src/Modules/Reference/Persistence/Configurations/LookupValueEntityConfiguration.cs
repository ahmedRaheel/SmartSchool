using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Reference.Models;

namespace SmartSchool.Modules.Reference.Persistence.Configurations;

public sealed class LookupValueEntityConfiguration : IEntityTypeConfiguration<LookupValueEntity>
{
    public void Configure(EntityTypeBuilder<LookupValueEntity> builder)
    {
        builder.ToTable("lookup_value", "saas");
        builder.HasKey(x => x.LookupValueId);
        builder.Property(x => x.LookupValueId).HasColumnName("lookup_value_id").ValueGeneratedOnAdd();
        builder.Property(x => x.LookupTypeId).HasColumnName("lookup_type_id").IsRequired();
        builder.Property(x => x.LookupTenantId).HasColumnName("tenant_id");
        builder.Ignore(x => x.TenantId);
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(x => x.SortOrder).HasColumnName("sort_order").IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(x => x.Metadata).HasColumnName("metadata").HasColumnType("jsonb");
        builder.Ignore(x => x.CreatedAt); builder.Ignore(x => x.UpdatedAt); builder.Ignore(x => x.RowVersion);
        builder.HasIndex(x => new { x.LookupTypeId, x.LookupTenantId, x.Code }).IsUnique();
    }
}
