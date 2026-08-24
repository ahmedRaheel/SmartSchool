using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="MessageEntity"/>.
/// </summary>
public sealed class MessageEntityConfiguration
	: IEntityTypeConfiguration<MessageEntity>
{
	public void Configure(EntityTypeBuilder<MessageEntity> builder)
	{
		builder.ToTable("message", schema: "communication");
builder.HasKey(entity => entity.MessageId);

		builder
			.Property(entity => entity.TenantId)
			.IsRequired();

		builder
			.Property(entity => entity.IsActive)
			.IsRequired();

		builder.HasIndex(entity => entity.TenantId);

		builder.Property(entity => entity.CreatedAt).IsRequired();
		builder.Property(entity => entity.UpdatedAt);
		builder.Property(entity => entity.RowVersion).IsRequired().IsConcurrencyToken();

		builder
			.Property(entity => entity.Code)
			.HasMaxLength(100)
			.IsRequired();

		builder
			.HasIndex(entity => new { entity.TenantId, entity.Code })
			.IsUnique();

		builder
			.Property(entity => entity.Name)
			.HasMaxLength(250)
			.IsRequired();


		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.Code).HasColumnName("code");
		builder.Property(entity => entity.Name).HasColumnName("name");
		builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
		builder.Property(entity => entity.MessageId).HasColumnName("message_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.ConversationId).HasColumnName("conversation_id");
		builder.Property(entity => entity.SenderUserId).HasColumnName("sender_user_id");
		builder.Property(entity => entity.ReplyToMessageId).HasColumnName("reply_to_message_id");
		builder.Property(entity => entity.MessageTypeCode).HasColumnName("message_type_code");
		builder.Property(entity => entity.Body).HasColumnName("body");
		builder.Property(entity => entity.SentAt).HasColumnName("sent_at");
		builder.Property(entity => entity.EditedAt).HasColumnName("edited_at");
		builder.Property(entity => entity.DeletedAt).HasColumnName("deleted_at");
	}
}
