using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatParticipantEntityConfiguration : IEntityTypeConfiguration<ChatParticipantEntity>
{
 public void Configure(EntityTypeBuilder<ChatParticipantEntity> builder)
	{
		builder.ToTable("chat_participant", schema: "communication");
builder.HasKey(entity => entity.ChatParticipantId);
		builder.Property(x => x.Role).HasMaxLength(50).IsRequired();
		builder.Property(x => x.RowVersion).IsConcurrencyToken();
		builder.HasIndex(x => new { x.TenantId, x.ConversationId, x.UserId }).IsUnique();
		builder.Property(entity => entity.ConversationId).HasColumnName("conversation_id");
		builder.Property(entity => entity.UserId).HasColumnName("user_id");
		builder.Property(entity => entity.Role).HasColumnName("role");
		builder.Property(entity => entity.JoinedAt).HasColumnName("joined_at");
		builder.Property(entity => entity.LastReadAt).HasColumnName("last_read_at");
		builder.Property(entity => entity.IsMuted).HasColumnName("is_muted");
		builder.Property(entity => entity.ChatParticipantId).HasColumnName("chat_participant_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");
	}
}
	
