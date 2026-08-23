using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class NotificationPreferenceEntityConfiguration:IEntityTypeConfiguration<NotificationPreferenceEntity>{public void Configure(EntityTypeBuilder<NotificationPreferenceEntity> b){b.ToTable("notification_preference","communication");b.HasKey(x=>x.Id);b.Property(x=>x.NotificationType).HasConversion<string>().HasMaxLength(80);b.Property(x=>x.RowVersion).IsConcurrencyToken();b.HasIndex(x=>new{x.TenantId,x.UserId,x.NotificationType}).IsUnique();}}
		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.UserId).HasColumnName("UserId");
		builder.Property(entity => entity.NotificationType).HasColumnName("NotificationType");
		builder.Property(entity => entity.InAppEnabled).HasColumnName("InAppEnabled");
		builder.Property(entity => entity.PushEnabled).HasColumnName("PushEnabled");
		builder.Property(entity => entity.EmailEnabled).HasColumnName("EmailEnabled");
		builder.Property(entity => entity.SmsEnabled).HasColumnName("SmsEnabled");
		builder.Property(entity => entity.Id).HasColumnName("Id");
		builder.Property(entity => entity.TenantId).HasColumnName("TenantId");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

