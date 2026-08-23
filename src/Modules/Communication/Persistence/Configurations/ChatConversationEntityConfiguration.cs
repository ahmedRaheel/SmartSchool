using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatConversationEntityConfiguration : IEntityTypeConfiguration<ChatConversationEntity>
{
 public void Configure(EntityTypeBuilder<ChatConversationEntity> builder)
	{
		builder.ToTable("chat_conversation","communication");
		builder.HasKey(x=>x.Id);
		builder.Property(x=>x.Title).HasMaxLength(250).IsRequired();
		builder.Property(x=>x.ConversationType).HasMaxLength(50).IsRequired();
		builder.Property(x=>x.RelatedEntityType).HasMaxLength(100);
		builder.Property(x=>x.RowVersion).IsConcurrencyToken();
		builder.HasIndex(x=>new{x.TenantId,x.CreatedByUserId});
	}
}
