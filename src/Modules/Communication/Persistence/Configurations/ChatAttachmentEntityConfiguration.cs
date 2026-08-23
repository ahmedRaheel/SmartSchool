using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class ChatAttachmentEntityConfiguration:IEntityTypeConfiguration<ChatAttachmentEntity>{public void Configure(EntityTypeBuilder<ChatAttachmentEntity> b){b.ToTable("chat_attachment","communication");b.HasKey(x=>x.Id);b.Property(x=>x.FileName).HasMaxLength(255).IsRequired();b.Property(x=>x.ContentType).HasMaxLength(150).IsRequired();b.Property(x=>x.StorageKey).HasMaxLength(500).IsRequired();b.Property(x=>x.RowVersion).IsConcurrencyToken();b.HasIndex(x=>new{x.TenantId,x.MessageId});}}
