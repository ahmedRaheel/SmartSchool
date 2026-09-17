using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Reference.Models;

namespace SmartSchool.Modules.Reference.Persistence.Configurations;

public sealed class LookupTypeEntityConfiguration : IEntityTypeConfiguration<LookupTypeEntity>
{
    public void Configure(EntityTypeBuilder<LookupTypeEntity> builder)
    {
        builder.ToTable("lookup_type", "saas");
        builder.HasKey(x => x.LookupTypeId);
        builder.Property(x => x.LookupTypeId).HasColumnName("lookup_type_id").ValueGeneratedOnAdd();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(80).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(x => x.IsTenantScoped).HasColumnName("is_tenant_scoped").HasDefaultValue(false).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
