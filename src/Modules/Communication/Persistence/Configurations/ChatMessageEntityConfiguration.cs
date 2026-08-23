using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatMessageEntityConfiguration : IEntityTypeConfiguration<ChatMessageEntity>
{
 public void Configure(EntityTypeBuilder<ChatMessageEntity> builder){
		builder.ToTable("chat_message","communication");
		builder.HasKey(x=>x.Id);
		builder.Property(x=>x.MessageType).HasMaxLength(30).IsRequired();
		builder.Property(x=>x.Message).HasMaxLength(5000).IsRequired();
		builder.Property(x=>x.RowVersion).IsConcurrencyToken();
		builder.HasIndex(x=>new{x.TenantId,x.ConversationId,x.SentAt});
	}
}
