using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence.Configurations;

public sealed class AdmissionPlacementEntityConfiguration : IEntityTypeConfiguration<AdmissionPlacementEntity>
{
    public void Configure(EntityTypeBuilder<AdmissionPlacementEntity> builder)
    {
        builder.ToTable("admission_placement", "student");
        builder.HasKey(x => x.AdmissionPlacementId);
        builder.Property(x => x.AdmissionPlacementId).HasColumnName("admission_placement_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.StudentId).HasColumnName("student_id");
        builder.Property(x => x.AcademicYearId).HasColumnName("academic_year_id");
        builder.Property(x => x.ClassSectionId).HasColumnName("class_section_id");
        builder.Property(x => x.RequestedAt).HasColumnName("requested_at");
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(20);
        builder.Property(x => x.ApprovedAt).HasColumnName("approved_at");
        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.CreatedAt);
        builder.Ignore(x => x.UpdatedAt);
        builder.Ignore(x => x.RowVersion);
    }
}
