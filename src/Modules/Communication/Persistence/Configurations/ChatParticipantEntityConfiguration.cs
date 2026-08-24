using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatParticipantEntityConfiguration : IEntityTypeConfiguration<ChatParticipantEntity>
{
 public void Configure(EntityTypeBuilder<ChatParticipantEntity> builder)
	{
		builder.ToTable("chat_participant", schema: "communication");
<<<<<<< HEAD
builder.HasKey(entity => entity.ChatParticipantId);
=======
		builder.Ignore(entity => entity.Id);
		builder.HasKey(x => x.Id);
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0
		builder.Property(x => x.Role).HasMaxLength(50).IsRequired();
		builder.Property(x => x.RowVersion).IsConcurrencyToken();
		builder.HasIndex(x => new { x.TenantId, x.ConversationId, x.UserId }).IsUnique();
		builder.Property(entity => entity.ConversationId).HasColumnName("conversation_id");
		builder.Property(entity => entity.UserId).HasColumnName("UserId");
		builder.Property(entity => entity.Role).HasColumnName("Role");
		builder.Property(entity => entity.JoinedAt).HasColumnName("JoinedAt");
		builder.Property(entity => entity.LastReadAt).HasColumnName("LastReadAt");
		builder.Property(entity => entity.IsMuted).HasColumnName("IsMuted");
		builder.Property(entity => entity.ChatParticipantId).HasColumnName("chat_participant_id");
		builder.Property(entity => entity.TenantId).HasColumnName("TenantId");
		builder.Property(entity => entity.IsActive).HasColumnName("IsActive");
		builder.Property(entity => entity.CreatedAt).HasColumnName("CreatedAt");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("UpdatedAt");
		builder.Property(entity => entity.RowVersion).HasColumnName("RowVersion");
	}
}
	
