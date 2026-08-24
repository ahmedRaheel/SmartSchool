using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence.Configurations;

/// <summary>Notification preference mapping.</summary>
public sealed class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreferenceEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<NotificationPreferenceEntity> builder)
    {
        builder.ToTable("NotificationPreferences", "Communication");
        builder.HasKey(x => x.NotificationPreferenceId);
        builder.Property(x => x.NotificationType).HasConversion<string>().HasMaxLength(80);
        builder.HasIndex(x => new { x.TenantId, x.UserId, x.NotificationType }).IsUnique();
    }
}
