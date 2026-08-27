using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence.Configurations;

/// <summary>Notification mapping.</summary>
public sealed class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.ToTable("Notifications", "Communication");
        builder.HasKey(x => x.NotificationId);
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(80);
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.RelatedEntityType).HasMaxLength(100);
        builder.Property(x => x.ActionUrl).HasMaxLength(500);
        builder.Property(x => x.Priority).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.RecipientUserId, x.IsRead, x.OccurredAt });
    }
}
