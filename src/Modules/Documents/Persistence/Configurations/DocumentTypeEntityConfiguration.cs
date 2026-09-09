using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence.Configurations;

public sealed class DocumentTypeEntityConfiguration : IEntityTypeConfiguration<DocumentTypeEntity>
{
    public void Configure(EntityTypeBuilder<DocumentTypeEntity> builder)
    {
        builder.ToTable("document_type", "document");
        builder.HasKey(entity => entity.DocumentTypeId);
        builder.Property(entity => entity.DocumentTypeId).HasColumnName("document_type_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.CampusId).HasColumnName("campus_id");
        builder.Property(entity => entity.OwnerType).HasColumnName("owner_type").HasConversion<string>().HasMaxLength(40);
        builder.Property(entity => entity.Code).HasColumnName("code").HasMaxLength(80).IsRequired();
        builder.Property(entity => entity.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.Description).HasColumnName("description").HasMaxLength(500);
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsConcurrencyToken();
        builder.HasIndex(entity => new { entity.TenantId, entity.CampusId, entity.Code }).IsUnique();
    }
}
