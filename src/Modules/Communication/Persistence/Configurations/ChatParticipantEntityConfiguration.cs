using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatParticipantEntityConfiguration : IEntityTypeConfiguration<ChatParticipantEntity>
{
 public void Configure(EntityTypeBuilder<ChatParticipantEntity> b){b.ToTable("chat_participant","communication");b.HasKey(x=>x.Id);b.Property(x=>x.Role).HasMaxLength(50).IsRequired();b.Property(x=>x.RowVersion).IsConcurrencyToken();b.HasIndex(x=>new{x.TenantId,x.ConversationId,x.UserId}).IsUnique();}
}
