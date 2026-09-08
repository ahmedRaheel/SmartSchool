using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence.Configurations;

public sealed class SectionEntityConfiguration : IEntityTypeConfiguration<SectionEntity>
{
    public void Configure(EntityTypeBuilder<SectionEntity> builder)
    {
        builder.ToTable("section", "academic");
        builder.HasKey(entity => entity.SectionId);
        builder.Property(entity => entity.SectionId).HasColumnName("section_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(entity => entity.Code).HasColumnName("code").HasMaxLength(30).IsRequired();
        builder.Property(entity => entity.Name).HasColumnName("name").HasMaxLength(80).IsRequired();
        builder.Property(entity => entity.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();
        builder.HasIndex(entity => new { entity.TenantId, entity.Code }).IsUnique();
    }
}
