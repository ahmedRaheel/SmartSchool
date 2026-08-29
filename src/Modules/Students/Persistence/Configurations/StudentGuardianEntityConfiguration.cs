using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence.Configurations;

public sealed class StudentGuardianEntityConfiguration : IEntityTypeConfiguration<StudentGuardianEntity>
{
    public void Configure(EntityTypeBuilder<StudentGuardianEntity> builder)
    {
        builder.ToTable("student_guardian", "student");
        builder.HasKey(x => x.StudentGuardianId);
        builder.Property(x => x.StudentGuardianId).HasColumnName("student_guardian_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(x => x.StudentId).HasColumnName("student_id").IsRequired();
        builder.Property(x => x.GuardianId).HasColumnName("guardian_id").IsRequired();
        builder.Property(x => x.Relationship).HasColumnName("relationship").HasMaxLength(30).IsRequired();
        builder.Property(x => x.IsPrimary).HasColumnName("is_primary").IsRequired();
        builder.Property(x => x.CanViewAcademics).HasColumnName("can_view_academics").IsRequired();
        builder.Property(x => x.CanViewFinance).HasColumnName("can_view_finance").IsRequired();
        builder.Property(x => x.CanPickup).HasColumnName("can_pickup").IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Ignore(x => x.RowVersion);
        builder.Ignore(x => x.UpdatedAt);
        builder.Ignore(x => x.Code);
        builder.Ignore(x => x.Name);
        builder.Ignore(x => x.MetadataJson);
        builder.HasIndex(x => new { x.StudentId, x.GuardianId }).IsUnique();
    }
}
