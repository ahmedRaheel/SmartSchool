using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence.Configurations;

public sealed class ActivityEntityConfiguration : IEntityTypeConfiguration<ActivityEntity>
{
    public void Configure(EntityTypeBuilder<ActivityEntity> builder)
    {
        builder.ToTable("activity", "activity");
        builder.HasKey(entity => entity.ActivityId);

        builder.Property(entity => entity.ActivityId).HasColumnName("activity_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(entity => entity.CampusId).HasColumnName("campus_id");
        builder.Property(entity => entity.CoordinatorEmployeeId).HasColumnName("coordinator_employee_id");
        builder.Property(entity => entity.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.Name).HasColumnName("name").HasMaxLength(180).IsRequired();
        builder.Property(entity => entity.Category).HasColumnName("category").HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.ActivityDate).HasColumnName("activity_date").IsRequired();
        builder.Property(entity => entity.StartTime).HasColumnName("start_time");
        builder.Property(entity => entity.EndTime).HasColumnName("end_time");
        builder.Property(entity => entity.Venue).HasColumnName("venue").HasMaxLength(250);
        builder.Property(entity => entity.Description).HasColumnName("description");
        builder.Property(entity => entity.MaxParticipants).HasColumnName("max_participants");
        builder.Property(entity => entity.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(entity => entity.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();

        builder.HasIndex(entity => new { entity.TenantId, entity.Code }).IsUnique();
        builder.HasIndex(entity => new { entity.TenantId, entity.ActivityDate, entity.Status });
    }
}
