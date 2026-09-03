using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatConversationEntityConfiguration : IEntityTypeConfiguration<ChatConversationEntity>
{
 public void Configure(EntityTypeBuilder<ChatConversationEntity> builder)
    {
        builder.ToTable("chat_conversation", schema: "communication");
        builder.HasKey(entity => entity.ChatConversationId);
        builder.Property(x=>x.Title).HasMaxLength(250).IsRequired();
        builder.Property(x=>x.ConversationType).HasMaxLength(50).IsRequired();
        builder.Property(x=>x.RelatedEntityType).HasMaxLength(100);
        builder.Property(x=>x.RowVersion).IsConcurrencyToken();
        builder	.HasIndex(x=>new{x.TenantId,x.CreatedByUserId});
        builder.Property(entity => entity.Title).HasColumnName("title");
        builder.Property(entity => entity.ConversationType).HasColumnName("conversation_type");
        builder.Property(entity => entity.CreatedByUserId).HasColumnName("created_by_user_id");
        builder.Property(entity => entity.RelatedEntityId).HasColumnName("related_entity_id");
        builder.Property(entity => entity.RelatedEntityType).HasColumnName("related_entity_type");
        builder.Property(entity => entity.IsClosed).HasColumnName("is_closed");
        builder.Property(entity => entity.ClosedAt).HasColumnName("closed_at");
        builder.Property(entity => entity.ChatConversationId).HasColumnName("chat_conversation_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version");
    }
}
