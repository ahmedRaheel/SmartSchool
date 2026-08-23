using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatParticipantEntityConfiguration : IEntityTypeConfiguration<ChatParticipantEntity>
{
 public void Configure(EntityTypeBuilder<ChatParticipantEntity> b){b.ToTable("chat_participant","communication");b.HasKey(x=>x.Id);b.Property(x=>x.Role).HasMaxLength(50).IsRequired();b.Property(x=>x.RowVersion).IsConcurrencyToken();b.HasIndex(x=>new{x.TenantId,x.ConversationId,x.UserId}).IsUnique();}
}
		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.ConversationId).HasColumnName("ConversationId");
		builder.Property(entity => entity.UserId).HasColumnName("UserId");
		builder.Property(entity => entity.Role).HasColumnName("Role");
		builder.Property(entity => entity.JoinedAt).HasColumnName("JoinedAt");
		builder.Property(entity => entity.LastReadAt).HasColumnName("LastReadAt");
		builder.Property(entity => entity.IsMuted).HasColumnName("IsMuted");
		builder.Property(entity => entity.Id).HasColumnName("Id");
		builder.Property(entity => entity.TenantId).HasColumnName("TenantId");
		builder.Property(entity => entity.IsActive).HasColumnName("IsActive");
		builder.Property(entity => entity.CreatedAt).HasColumnName("CreatedAt");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("UpdatedAt");
		builder.Property(entity => entity.RowVersion).HasColumnName("RowVersion");

