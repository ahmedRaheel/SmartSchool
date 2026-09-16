using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence.Configurations;

public sealed class StudentActivityEntityConfiguration : IEntityTypeConfiguration<StudentActivityEntity>
{
    public void Configure(EntityTypeBuilder<StudentActivityEntity> builder)
    {
        builder.ToTable("student_activity", "activity");
        builder.HasKey(entity => entity.StudentActivityId);

        builder.Property(entity => entity.StudentActivityId).HasColumnName("student_activity_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(entity => entity.ActivityId).HasColumnName("activity_id").IsRequired();
        builder.Property(entity => entity.StudentId).HasColumnName("student_id").IsRequired();
        builder.Property(entity => entity.RoleName).HasColumnName("role_name").HasMaxLength(100);
        builder.Property(entity => entity.JoinedAt).HasColumnName("joined_at").IsRequired();
        builder.Property(entity => entity.LeftAt).HasColumnName("left_at");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();

        builder.HasIndex(entity => new { entity.TenantId, entity.ActivityId, entity.StudentId })
            .IsUnique()
            .HasFilter("is_active = TRUE");

        builder.HasOne<ActivityEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.ActivityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
