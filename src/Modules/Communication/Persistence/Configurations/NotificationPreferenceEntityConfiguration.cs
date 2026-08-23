using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;
namespace SmartSchool.Modules.Communication.Persistence.Configurations;
public sealed class NotificationPreferenceEntityConfiguration:IEntityTypeConfiguration<NotificationPreferenceEntity>{public void Configure(EntityTypeBuilder<NotificationPreferenceEntity> b){b.ToTable("notification_preference","communication");b.HasKey(x=>x.Id);b.Property(x=>x.NotificationType).HasConversion<string>().HasMaxLength(80);b.Property(x=>x.RowVersion).IsConcurrencyToken();b.HasIndex(x=>new{x.TenantId,x.UserId,x.NotificationType}).IsUnique();}}
