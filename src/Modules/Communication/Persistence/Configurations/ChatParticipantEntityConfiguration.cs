using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatParticipantEntityConfiguration : IEntityTypeConfiguration<ChatParticipantEntity>
{
 public void Configure(EntityTypeBuilder<ChatParticipantEntity> builder){
		builder.ToTable("chat_participant","communication");
		builder.HasKey(x=>x.Id);
		builder.Property(x=>x.Role).HasMaxLength(50).IsRequired();
		builder.Property(x=>x.RowVersion).IsConcurrencyToken();
		builder.HasIndex(x=>new{x.TenantId,x.ConversationId,x.UserId}).IsUnique();
	}
}
