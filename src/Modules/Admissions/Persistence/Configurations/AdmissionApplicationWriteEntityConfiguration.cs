using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Admissions.Features;

namespace SmartSchool.Modules.Admissions.Persistence.Configurations;
public sealed class AdmissionApplicationWriteEntityConfiguration : IEntityTypeConfiguration<AdmissionApplicationWriteEntity>
{
    public void Configure(EntityTypeBuilder<AdmissionApplicationWriteEntity> builder)
    {
        builder.ToTable("student_application", "admission");
        builder.HasKey(x => x.ApplicationId);
        builder.Property(x => x.ApplicationId).HasColumnName("application_id"); builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.SchoolId).HasColumnName("school_id"); builder.Property(x => x.BranchId).HasColumnName("branch_id");
        builder.Property(x => x.AcademicYearId).HasColumnName("academic_year_id"); builder.Property(x => x.ClassId).HasColumnName("class_id"); builder.Property(x => x.SectionId).HasColumnName("section_id");
        builder.Property(x => x.FirstName).HasColumnName("first_name"); builder.Property(x => x.LastName).HasColumnName("last_name"); builder.Property(x => x.DateOfBirth).HasColumnName("date_of_birth"); builder.Property(x => x.Gender).HasColumnName("gender");
        builder.Property(x => x.Email).HasColumnName("email"); builder.Property(x => x.Phone).HasColumnName("phone"); builder.Property(x => x.Address).HasColumnName("address");
        builder.Property(x => x.GuardianName).HasColumnName("guardian_name"); builder.Property(x => x.GuardianCnic).HasColumnName("guardian_cnic"); builder.Property(x => x.GuardianEmail).HasColumnName("guardian_email"); builder.Property(x => x.GuardianPhone).HasColumnName("guardian_phone");
        builder.Property(x => x.Relationship).HasColumnName("relationship"); builder.Property(x => x.PreviousSchool).HasColumnName("previous_school"); builder.Property(x => x.PreviousMarks).HasColumnName("previous_marks"); builder.Property(x => x.Status).HasColumnName("status");
        builder.Property(x => x.IsActive).HasColumnName("is_active"); builder.Property(x => x.CreatedAt).HasColumnName("created_at"); builder.Property(x => x.UpdatedAt).HasColumnName("updated_at"); builder.Property(x => x.RowVersion).HasColumnName("row_version");
    }
}
