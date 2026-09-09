using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence.Configurations;

public sealed class RequiredDocumentTypeEntityConfiguration : IEntityTypeConfiguration<RequiredDocumentTypeEntity>
{
    public void Configure(EntityTypeBuilder<RequiredDocumentTypeEntity> builder)
    {
        builder.ToTable("required_document_type", "document");
        builder.HasKey(entity => entity.RequiredDocumentTypeId);
        builder.Property(entity => entity.RequiredDocumentTypeId).HasColumnName("required_document_type_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.CampusId).HasColumnName("campus_id");
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
