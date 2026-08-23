using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatAttachmentEntityConfiguration:IEntityTypeConfiguration<ChatAttachmentEntity>{public void Configure(EntityTypeBuilder<ChatAttachmentEntity> b){b.ToTable("chat_attachment","communication");b.HasKey(x=>x.Id);b.Property(x=>x.FileName).HasMaxLength(255).IsRequired();b.Property(x=>x.ContentType).HasMaxLength(150).IsRequired();b.Property(x=>x.StorageKey).HasMaxLength(500).IsRequired();b.Property(x=>x.RowVersion).IsConcurrencyToken();b.HasIndex(x=>new{x.TenantId,x.MessageId});}}
		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.MessageId).HasColumnName("MessageId");
		builder.Property(entity => entity.FileName).HasColumnName("FileName");
		builder.Property(entity => entity.ContentType).HasColumnName("ContentType");
		builder.Property(entity => entity.FileSizeBytes).HasColumnName("FileSizeBytes");
		builder.Property(entity => entity.StorageKey).HasColumnName("StorageKey");
		builder.Property(entity => entity.Id).HasColumnName("Id");
		builder.Property(entity => entity.TenantId).HasColumnName("TenantId");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

