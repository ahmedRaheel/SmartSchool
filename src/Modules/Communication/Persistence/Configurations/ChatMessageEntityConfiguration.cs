using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatMessageEntityConfiguration : IEntityTypeConfiguration<ChatMessageEntity>
{
 public void Configure(EntityTypeBuilder<ChatMessageEntity> builder)
    {
        builder.ToTable("chat_message", schema: "communication");
        builder.HasKey(entity => entity.ChatMessageId);
        builder.Property(x => x.MessageType).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(5000).IsRequired();
        builder.Property(x => x.RowVersion).IsConcurrencyToken();
        builder.HasIndex(x => new { x.TenantId, x.ConversationId, x.SentAt });
        builder.Property(entity => entity.ConversationId).HasColumnName("conversation_id");
        builder.Property(entity => entity.SenderUserId).HasColumnName("sender_user_id");
        builder.Property(entity => entity.MessageType).HasColumnName("message_type");
        builder.Property(entity => entity.Message).HasColumnName("message");
        builder.Property(entity => entity.ReplyToMessageId).HasColumnName("reply_to_message_id");
        builder.Property(entity => entity.SentAt).HasColumnName("sent_at");
        builder.Property(entity => entity.EditedAt).HasColumnName("edited_at");
        builder.Property(entity => entity.IsDeleted).HasColumnName("is_deleted");
        builder.Property(entity => entity.ChatMessageId).HasColumnName("chat_message_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
        builder.HasOne<ConversationEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.ConversationId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
