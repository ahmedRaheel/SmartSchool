using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;

public sealed class ChatAttachmentEntityConfiguration : IEntityTypeConfiguration<ChatAttachmentEntity>
{
	public void Configure(EntityTypeBuilder<ChatAttachmentEntity> builder)
	{
		builder.ToTable("chat_attachment", "communication");
		builder.HasKey(entity => entity.ChatAttachmentId);
		builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
		builder.Property(x => x.ContentType).HasMaxLength(150).IsRequired();
		builder.Property(x => x.StorageKey).HasMaxLength(500).IsRequired();
		builder.Property(x => x.RowVersion).IsConcurrencyToken();
		builder.HasIndex(x => new { x.TenantId, x.MessageId });
		builder.Property(entity => entity.MessageId).HasColumnName("message_id");
		builder.Property(entity => entity.FileName).HasColumnName("file_name");
		builder.Property(entity => entity.ContentType).HasColumnName("content_type");
		builder.Property(entity => entity.FileSizeBytes).HasColumnName("file_size_bytes");
		builder.Property(entity => entity.StorageKey).HasColumnName("storage_key");
		builder.Property(entity => entity.ChatAttachmentId).HasColumnName("chat_attachment_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
        builder.HasOne<MessageEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.MessageId)
            .OnDelete(DeleteBehavior.Restrict);

	}
}	
