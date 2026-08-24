using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;

public sealed class ChatAttachmentEntityConfiguration : IEntityTypeConfiguration<ChatAttachmentEntity>
{
	public void Configure(EntityTypeBuilder<ChatAttachmentEntity> builder)
	{
		builder.ToTable("chat_attachment", "communication");
		builder.Ignore(entity => entity.Id);
		builder.HasKey(x => x.Id);
		builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
		builder.Property(x => x.ContentType).HasMaxLength(150).IsRequired();
		builder.Property(x => x.StorageKey).HasMaxLength(500).IsRequired();
		builder.Property(x => x.RowVersion).IsConcurrencyToken();
		builder.HasIndex(x => new { x.TenantId, x.MessageId });
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
	}
}	
