using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence.Configurations;

public sealed class TeacherTeachingAssignmentEntityConfiguration
    : IEntityTypeConfiguration<TeacherTeachingAssignmentEntity>
{
    public void Configure(
        EntityTypeBuilder<TeacherTeachingAssignmentEntity> builder)
    {
        builder.ToTable("teacher_teaching_assignment", "hr");
        builder.HasKey(entity => entity.TeacherTeachingAssignmentId);

        builder.Property(entity => entity.TeacherTeachingAssignmentId)
            .HasColumnName("teacher_teaching_assignment_id");
        builder.Property(entity => entity.TenantId)
            .HasColumnName("tenant_id");
        builder.Property(entity => entity.SchoolId)
            .HasColumnName("school_id");
        builder.Property(entity => entity.CampusId)
            .HasColumnName("campus_id");
        builder.Property(entity => entity.EmployeeId)
            .HasColumnName("employee_id");
        builder.Property(entity => entity.ClassSectionId)
            .HasColumnName("class_section_id");
        builder.Property(entity => entity.SubjectId)
            .HasColumnName("subject_id");
        builder.Property(entity => entity.Code)
            .HasColumnName("code")
            .HasMaxLength(50);
        builder.Property(entity => entity.Name)
            .HasColumnName("name")
            .HasMaxLength(250);
        builder.Property(entity => entity.PeriodsPerWeek)
            .HasColumnName("periods_per_week");
        builder.Property(entity => entity.IsClassTeacher)
            .HasColumnName("is_class_teacher");
        builder.Property(entity => entity.EffectiveFrom)
            .HasColumnName("effective_from");
        builder.Property(entity => entity.EffectiveTo)
            .HasColumnName("effective_to");
        builder.Property(entity => entity.IsActive)
            .HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt)
            .HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt)
            .HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion)
            .HasColumnName("row_version")
            .IsConcurrencyToken();
    }
}
