using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence.Configurations;

/// <summary>Attachment mapping.</summary>
public sealed class ChatAttachmentConfiguration : IEntityTypeConfiguration<ChatAttachmentEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ChatAttachmentEntity> builder)
    {
        builder.ToTable("ChatAttachments", "Communication");
        builder.HasKey(x => x.ChatAttachmentId);
        builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.ContentType).HasMaxLength(150).IsRequired();
        builder.Property(x => x.StorageKey).HasMaxLength(500).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.MessageId });
    }
}
