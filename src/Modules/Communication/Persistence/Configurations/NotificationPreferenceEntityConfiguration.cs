using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence.Configurations;

/// <summary>
/// Configures persistence for <see cref="NotificationPreferenceEntity"/>.
/// </summary>
public sealed class NotificationPreferenceEntityConfiguration
    : IEntityTypeConfiguration<NotificationPreferenceEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<NotificationPreferenceEntity> builder)
    {
        builder.ToTable("notification_preference", "communication");

        builder.HasKey(entity => entity.NotificationPreferenceId);

        builder.Property(entity => entity.NotificationPreferenceId)
            .HasColumnName("notification_preference_id");

        builder.Property(entity => entity.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(entity => entity.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(entity => entity.NotificationType)
            .HasColumnName("notification_type")
            .HasConversion<string>()
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(entity => entity.InAppEnabled)
            .HasColumnName("in_app_enabled")
            .IsRequired();

        builder.Property(entity => entity.PushEnabled)
            .HasColumnName("push_enabled")
            .IsRequired();

        builder.Property(entity => entity.EmailEnabled)
            .HasColumnName("email_enabled")
            .IsRequired();

        builder.Property(entity => entity.SmsEnabled)
            .HasColumnName("sms_enabled")
            .IsRequired();

        builder.Property(entity => entity.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(entity => entity.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(entity => entity.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(entity => entity.RowVersion)
            .HasColumnName("row_version")
            .IsConcurrencyToken();

        builder.HasIndex(entity => new
        {
            entity.TenantId,
            entity.UserId,
            entity.NotificationType
        }).IsUnique();
    }
}
