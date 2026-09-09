using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence.Configurations;

public sealed class RequiredDocumentEntityConfiguration : IEntityTypeConfiguration<RequiredDocumentEntity>
{
    public void Configure(EntityTypeBuilder<RequiredDocumentEntity> builder)
    {
        builder.ToTable("required_document", "document");
        builder.HasKey(entity => entity.RequiredDocumentId);
        builder.Property(entity => entity.RequiredDocumentId).HasColumnName("required_document_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.CampusId).HasColumnName("campus_id");
        builder.Property(entity => entity.UserRole).HasColumnName("user_role").HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.IsMandatory).HasColumnName("is_mandatory");
        builder.Property(entity => entity.RequiredDocumentTypeId).HasColumnName("required_document_type_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsConcurrencyToken();
        builder.HasIndex(entity => new { entity.TenantId, entity.CampusId, entity.UserRole, entity.RequiredDocumentTypeId }).IsUnique();
    }
}
