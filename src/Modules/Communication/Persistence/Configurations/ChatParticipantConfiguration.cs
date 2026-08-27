using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence.Configurations;

/// <summary>Participant mapping.</summary>
public sealed class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipantEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ChatParticipantEntity> builder)
    {
        builder.ToTable("ChatParticipants", "Communication");
        builder.HasKey(x => x.ChatParticipantId);
        builder.Property(x => x.Role).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.ConversationId, x.UserId }).IsUnique();
    }
}
