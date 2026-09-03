using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for
/// <see cref="NotificationEntity"/>.
/// </summary>
public sealed class NotificationEntityConfiguration
    : IEntityTypeConfiguration<NotificationEntity>
{
    /// <summary>
    /// Configures the database mapping for notifications.
    /// </summary>
    /// <param name="builder">
    /// Entity Framework configuration builder.
    /// </param>
    public void Configure(
        EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.ToTable("notification", schema: "communication");
        builder.HasKey(entity => entity.NotificationId);

        builder
            .Property(entity => entity.TenantId)
            .IsRequired();

        builder
            .Property(entity => entity.RecipientUserId)
            .IsRequired();

        builder
            .Property(entity => entity.Type)
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(entity => entity.Title)
            .HasMaxLength(250)
            .IsRequired();

        builder
            .Property(entity => entity.Message)
            .HasMaxLength(2000)
            .IsRequired();

        builder
            .Property(entity => entity.RelatedEntityId);

        builder
            .Property(entity => entity.RelatedEntityType)
            .HasMaxLength(100);

        builder
            .Property(entity => entity.ActionUrl)
            .HasMaxLength(500);

        builder
            .Property(entity => entity.Priority)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(entity => entity.IsRead)
            .IsRequired();

        builder
            .Property(entity => entity.ReadAt);

        builder
            .Property(entity => entity.OccurredAt)
            .IsRequired();

        builder
            .Property(entity => entity.IsActive)
            .IsRequired();

        builder
            .Property(entity => entity.RowVersion)
            .IsConcurrencyToken();

        builder.HasIndex(
            entity => entity.TenantId);

        builder.HasIndex(
            entity => new
            {
                entity.TenantId,
                entity.RecipientUserId
            });

        builder.HasIndex(
            entity => new
            {
                entity.TenantId,
                entity.RecipientUserId,
                entity.IsRead
            });

        builder.HasIndex(
            entity => new
            {
                entity.TenantId,
                entity.RecipientUserId,
                entity.OccurredAt
            });

        builder.HasIndex(
            entity => new
            {
                entity.TenantId,
                entity.Type
            });

        // Canonical database mapping generated from SmartSchoolComplete.sql.
        builder.Property(entity => entity.RecipientUserId).HasColumnName("recipient_user_id");
        builder.Property(entity => entity.Type)
            .HasConversion<string>()
            .HasColumnName("type")
            .HasMaxLength(80);
        builder.Property(entity => entity.Title).HasColumnName("title");
        builder.Property(entity => entity.Message).HasColumnName("message");
        builder.Property(entity => entity.RelatedEntityId).HasColumnName("related_entity_id");
        builder.Property(entity => entity.RelatedEntityType).HasColumnName("related_entity_type");
        builder.Property(entity => entity.ActionUrl).HasColumnName("action_url");
        builder.Property(entity => entity.Priority).HasColumnName("priority");
        builder.Property(entity => entity.IsRead).HasColumnName("is_read");
        builder.Property(entity => entity.ReadAt).HasColumnName("read_at");
        builder.Property(entity => entity.OccurredAt).HasColumnName("occurred_at");
        builder.Property(entity => entity.NotificationId).HasColumnName("notification_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

    }
}
