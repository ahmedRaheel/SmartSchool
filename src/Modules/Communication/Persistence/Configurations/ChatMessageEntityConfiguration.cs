using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatMessageEntityConfiguration : IEntityTypeConfiguration<ChatMessageEntity>
{
 public void Configure(EntityTypeBuilder<ChatMessageEntity> b){b.ToTable("chat_message","communication");b.HasKey(x=>x.Id);b.Property(x=>x.MessageType).HasMaxLength(30).IsRequired();b.Property(x=>x.Message).HasMaxLength(5000).IsRequired();b.Property(x=>x.RowVersion).IsConcurrencyToken();b.HasIndex(x=>new{x.TenantId,x.ConversationId,x.SentAt});}
}
