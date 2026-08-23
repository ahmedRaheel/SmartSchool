using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatConversationEntityConfiguration : IEntityTypeConfiguration<ChatConversationEntity>
{
 public void Configure(EntityTypeBuilder<ChatConversationEntity> b){b.ToTable("chat_conversation","communication");b.HasKey(x=>x.Id);b.Property(x=>x.Title).HasMaxLength(250).IsRequired();b.Property(x=>x.ConversationType).HasMaxLength(50).IsRequired();b.Property(x=>x.RelatedEntityType).HasMaxLength(100);b.Property(x=>x.RowVersion).IsConcurrencyToken();b.HasIndex(x=>new{x.TenantId,x.CreatedByUserId});}
}
