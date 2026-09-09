using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence.Configurations;

public sealed class DocumentFileEntityConfiguration : IEntityTypeConfiguration<DocumentFileEntity>
{
    public void Configure(EntityTypeBuilder<DocumentFileEntity> builder)
    {
        builder.ToTable("document", "document");
        builder.HasKey(entity => entity.DocumentId);
        builder.Property(entity => entity.DocumentId).HasColumnName("document_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.CampusId).HasColumnName("campus_id");
        builder.Property(entity => entity.DocumentTypeId).HasColumnName("document_type_id");
        builder.Property(entity => entity.RequiredDocumentTypeId).HasColumnName("required_document_type_id");
        builder.Property(entity => entity.OwnerId).HasColumnName("owner_id");
        builder.Property(entity => entity.OwnerType).HasColumnName("owner_type").HasConversion<string>().HasMaxLength(40);
        builder.Property(entity => entity.DocumentNumber).HasColumnName("document_number").HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.OriginalFileName).HasColumnName("original_file_name").HasMaxLength(255).IsRequired();
        builder.Property(entity => entity.StoredFileName).HasColumnName("stored_file_name").HasMaxLength(255).IsRequired();
        builder.Property(entity => entity.Extension).HasColumnName("extension").HasMaxLength(20);
        builder.Property(entity => entity.MimeType).HasColumnName("mime_type").HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.SizeBytes).HasColumnName("size_bytes");
        builder.Property(entity => entity.Sha256).HasColumnName("sha256").HasMaxLength(64).IsRequired();
        builder.Property(entity => entity.BlobData).HasColumnName("blob_data");
        builder.Property(entity => entity.Title).HasColumnName("title").HasMaxLength(250);
        builder.Property(entity => entity.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(entity => entity.IsConfidential).HasColumnName("is_confidential");
        builder.Property(entity => entity.UploadedBy).HasColumnName("uploaded_by");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsConcurrencyToken();
        builder.HasIndex(entity => new { entity.TenantId, entity.DocumentNumber }).IsUnique();
        builder.HasIndex(entity => new { entity.TenantId, entity.CampusId, entity.OwnerType, entity.OwnerId });
    }
}
