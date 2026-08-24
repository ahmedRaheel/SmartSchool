using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatConversationEntityConfiguration : IEntityTypeConfiguration<ChatConversationEntity>
{
 public void Configure(EntityTypeBuilder<ChatConversationEntity> builder)
	{
		builder.ToTable("chat_conversation", schema: "communication");
<<<<<<< HEAD
builder.HasKey(entity => entity.ChatConversationId);
=======
		builder.Ignore(entity => entity.Id);
		builder.HasKey(x=>x.Id);
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0
		builder.Property(x=>x.Title).HasMaxLength(250).IsRequired();
		builder.Property(x=>x.ConversationType).HasMaxLength(50).IsRequired();
		builder.Property(x=>x.RelatedEntityType).HasMaxLength(100);
		builder.Property(x=>x.RowVersion).IsConcurrencyToken();
		builder	.HasIndex(x=>new{x.TenantId,x.CreatedByUserId});
		builder.Property(entity => entity.Title).HasColumnName("Title");
		builder.Property(entity => entity.ConversationType).HasColumnName("ConversationType");
		builder.Property(entity => entity.CreatedByUserId).HasColumnName("CreatedByUserId");
		builder.Property(entity => entity.RelatedEntityId).HasColumnName("RelatedEntityId");
		builder.Property(entity => entity.RelatedEntityType).HasColumnName("RelatedEntityType");
		builder.Property(entity => entity.IsClosed).HasColumnName("IsClosed");
		builder.Property(entity => entity.ClosedAt).HasColumnName("ClosedAt");
		builder.Property(entity => entity.ChatConversationId).HasColumnName("Id");
		builder.Property(entity => entity.TenantId).HasColumnName("TenantId");
		builder.Property(entity => entity.IsActive).HasColumnName("IsActive");
		builder.Property(entity => entity.CreatedAt).HasColumnName("CreatedAt");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("UpdatedAt");
		builder.Property(entity => entity.RowVersion).HasColumnName("RowVersion");
	}
}
