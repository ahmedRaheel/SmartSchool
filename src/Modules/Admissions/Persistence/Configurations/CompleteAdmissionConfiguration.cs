using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Admissions.Features;

namespace SmartSchool.Modules.Admissions.Persistence.Configurations;

public sealed class CompleteAdmissionStudentConfiguration
    : IEntityTypeConfiguration<CompleteAdmissionStudent>
{
    public void Configure(EntityTypeBuilder<CompleteAdmissionStudent> builder)
    {
        builder.ToTable("student", "student");
        builder.HasKey(x => x.StudentId);
        builder.Property(x => x.StudentId).HasColumnName("student_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.SchoolId).HasColumnName("school_id");
        builder.Property(x => x.BranchId).HasColumnName("branch_id");
        builder.Property(x => x.StudentNumber).HasColumnName("student_number");
        builder.Property(x => x.FirstName).HasColumnName("first_name");
        builder.Property(x => x.LastName).HasColumnName("last_name");
        builder.Property(x => x.DateOfBirth).HasColumnName("date_of_birth");
        builder.Property(x => x.Gender).HasColumnName("gender");
        builder.Property(x => x.AdmissionDate).HasColumnName("admission_date");
        builder.Property(x => x.Status).HasColumnName("status");
    }
}

public sealed class CompleteAdmissionGuardianConfiguration
    : IEntityTypeConfiguration<CompleteAdmissionGuardian>
{
    public void Configure(EntityTypeBuilder<CompleteAdmissionGuardian> builder)
    {
        builder.ToTable("guardian", "student");
        builder.HasKey(x => x.GuardianId);
        builder.Property(x => x.GuardianId).HasColumnName("guardian_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.FullName).HasColumnName("full_name");
        builder.Property(x => x.CnicNumber).HasColumnName("cnic_number");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.Phone).HasColumnName("phone");
    }
}

public sealed class CompleteAdmissionStudentGuardianConfiguration
    : IEntityTypeConfiguration<CompleteAdmissionStudentGuardian>
{
    public void Configure(EntityTypeBuilder<CompleteAdmissionStudentGuardian> builder)
    {
        builder.ToTable("student_guardian", "student");
        builder.HasKey(x => x.StudentGuardianId);
        builder.Property(x => x.StudentGuardianId).HasColumnName("student_guardian_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.StudentId).HasColumnName("student_id");
        builder.Property(x => x.GuardianId).HasColumnName("guardian_id");
        builder.Property(x => x.Relationship).HasColumnName("relationship");
        builder.Property(x => x.IsPrimary).HasColumnName("is_primary");
        builder.Property(x => x.CanViewAcademics).HasColumnName("can_view_academics");
        builder.Property(x => x.CanViewFinance).HasColumnName("can_view_finance");
        builder.Property(x => x.CanPickup).HasColumnName("can_pickup");
    }
}

public sealed class CompleteAdmissionEnrollmentConfiguration
    : IEntityTypeConfiguration<CompleteAdmissionEnrollment>
{
    public void Configure(EntityTypeBuilder<CompleteAdmissionEnrollment> builder)
    {
        builder.ToTable("student_enrollment", "student");
        builder.HasKey(x => x.StudentEnrollmentId);
        builder.Property(x => x.StudentEnrollmentId).HasColumnName("student_enrollment_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.StudentId).HasColumnName("student_id");
        builder.Property(x => x.EnrollmentNumber).HasColumnName("enrollment_number");
        builder.Property(x => x.AcademicYearId).HasColumnName("academic_year_id");
        builder.Property(x => x.ClassClassSectionId).HasColumnName("class_class_section_id");
        builder.Property(x => x.EnrollmentDate).HasColumnName("enrollment_date");
        builder.Property(x => x.Status).HasColumnName("status");
    }
}

public sealed class CompleteAdmissionApplicationConfiguration
    : IEntityTypeConfiguration<CompleteAdmissionApplication>
{
    public void Configure(EntityTypeBuilder<CompleteAdmissionApplication> builder)
    {
        builder.ToTable("student_application", "admission");
        builder.HasKey(x => x.ApplicationId);
        builder.Property(x => x.ApplicationId).HasColumnName("application_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.Status).HasColumnName("status");
        builder.Property(x => x.StudentId).HasColumnName("student_id");
        builder.Property(x => x.DecisionNotes).HasColumnName("decision_notes");
        builder.Property(x => x.DecidedAt).HasColumnName("decided_at");
    }
}
