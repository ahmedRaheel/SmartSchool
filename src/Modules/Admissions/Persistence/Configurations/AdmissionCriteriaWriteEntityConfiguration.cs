using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Admissions.Features;

namespace SmartSchool.Modules.Admissions.Persistence.Configurations;

public sealed class AdmissionCriteriaWriteEntityConfiguration
    : IEntityTypeConfiguration<AdmissionCriteriaWriteEntity>
{
    public void Configure(EntityTypeBuilder<AdmissionCriteriaWriteEntity> builder)
    {
        builder.ToTable("admission_criteria", "admission");
        builder.HasKey(criteria => criteria.AdmissionCriteriaId);

        builder.Property(criteria => criteria.AdmissionCriteriaId)
            .HasColumnName("admission_criteria_id");
        builder.Property(criteria => criteria.TenantId).HasColumnName("tenant_id");
        builder.Property(criteria => criteria.SchoolId).HasColumnName("school_id");
        builder.Property(criteria => criteria.BranchId).HasColumnName("branch_id");
        builder.Property(criteria => criteria.AcademicYearId).HasColumnName("academic_year_id");
        builder.Property(criteria => criteria.ClassId).HasColumnName("class_id");
        builder.Property(criteria => criteria.MinimumMarks).HasColumnName("minimum_marks");
        builder.Property(criteria => criteria.EntranceTestMinimum).HasColumnName("entrance_test_minimum");
        builder.Property(criteria => criteria.MinimumAge).HasColumnName("minimum_age");
        builder.Property(criteria => criteria.MaximumAge).HasColumnName("maximum_age");
        builder.Property(criteria => criteria.InterviewRequired).HasColumnName("interview_required");
        builder.Property(criteria => criteria.RequiredDocuments).HasColumnName("required_documents");
    }
}
