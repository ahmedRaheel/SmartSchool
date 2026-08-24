using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatMessageEntityConfiguration : IEntityTypeConfiguration<ChatMessageEntity>
{
 public void Configure(EntityTypeBuilder<ChatMessageEntity> builder)
	{
		builder.ToTable("chat_message", schema: "communication");
<<<<<<< HEAD
builder.HasKey(entity => entity.ChatMessageId);
=======
		builder.Ignore(entity => entity.Id);
		builder.HasKey(x => x.Id);
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0
		builder.Property(x => x.MessageType).HasMaxLength(30).IsRequired();
		builder.Property(x => x.Message).HasMaxLength(5000).IsRequired();
		builder.Property(x => x.RowVersion).IsConcurrencyToken();
		builder.HasIndex(x => new { x.TenantId, x.ConversationId, x.SentAt });
		builder.Property(entity => entity.ConversationId).HasColumnName("conversation_id");
		builder.Property(entity => entity.SenderUserId).HasColumnName("SenderUserId");
		builder.Property(entity => entity.MessageType).HasColumnName("MessageType");
		builder.Property(entity => entity.Message).HasColumnName("Message");
		builder.Property(entity => entity.ReplyToMessageId).HasColumnName("ReplyToMessageId");
		builder.Property(entity => entity.SentAt).HasColumnName("SentAt");
		builder.Property(entity => entity.EditedAt).HasColumnName("EditedAt");
		builder.Property(entity => entity.IsDeleted).HasColumnName("IsDeleted");
		builder.Property(entity => entity.ChatMessageId).HasColumnName("chat_message_id");
		builder.Property(entity => entity.TenantId).HasColumnName("TenantId");
		builder.Property(entity => entity.IsActive).HasColumnName("IsActive");
		builder.Property(entity => entity.CreatedAt).HasColumnName("CreatedAt");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("UpdatedAt");
		builder.Property(entity => entity.RowVersion).HasColumnName("RowVersion");
	}
}
