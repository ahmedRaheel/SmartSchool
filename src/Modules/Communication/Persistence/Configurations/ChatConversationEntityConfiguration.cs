using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatConversationEntityConfiguration : IEntityTypeConfiguration<ChatConversationEntity>
{
 public void Configure(EntityTypeBuilder<ChatConversationEntity> b){b.ToTable("chat_conversation","communication");b.HasKey(x=>x.Id);b.Property(x=>x.Title).HasMaxLength(250).IsRequired();b.Property(x=>x.ConversationType).HasMaxLength(50).IsRequired();b.Property(x=>x.RelatedEntityType).HasMaxLength(100);b.Property(x=>x.RowVersion).IsConcurrencyToken();b.HasIndex(x=>new{x.TenantId,x.CreatedByUserId});}
}
		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.Title).HasColumnName("Title");
		builder.Property(entity => entity.ConversationType).HasColumnName("ConversationType");
		builder.Property(entity => entity.CreatedByUserId).HasColumnName("CreatedByUserId");
		builder.Property(entity => entity.RelatedEntityId).HasColumnName("RelatedEntityId");
		builder.Property(entity => entity.RelatedEntityType).HasColumnName("RelatedEntityType");
		builder.Property(entity => entity.IsClosed).HasColumnName("IsClosed");
		builder.Property(entity => entity.ClosedAt).HasColumnName("ClosedAt");
		builder.Property(entity => entity.Id).HasColumnName("Id");
		builder.Property(entity => entity.TenantId).HasColumnName("TenantId");
		builder.Property(entity => entity.IsActive).HasColumnName("IsActive");
		builder.Property(entity => entity.CreatedAt).HasColumnName("CreatedAt");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("UpdatedAt");
		builder.Property(entity => entity.RowVersion).HasColumnName("RowVersion");

