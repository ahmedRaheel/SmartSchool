using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="AssignmentEntity"/>.
/// </summary>
public sealed class AssignmentEntityConfiguration
	: IEntityTypeConfiguration<AssignmentEntity>
{
	public void Configure(EntityTypeBuilder<AssignmentEntity> builder)
	{
		builder.ToTable("academic_assignment", schema: "lms");
		builder.HasKey(entity => entity.AcademicAssignmentId);

		builder
			.Property(entity => entity.TenantId)
			.IsRequired();

		builder
			.Property(entity => entity.IsActive)
			.IsRequired();

		builder.HasIndex(entity => entity.TenantId);

		builder.Property(entity => entity.CreatedAt).IsRequired();
		builder.Property(entity => entity.UpdatedAt);
		builder.Property(entity => entity.RowVersion).IsRequired().IsConcurrencyToken();

		builder
			.Property(entity => entity.Code)
			.HasMaxLength(100)
			.IsRequired();

		builder
			.HasIndex(entity => new { entity.TenantId, entity.Code })
			.IsUnique();

		builder
			.Property(entity => entity.Name)
			.HasMaxLength(250)
			.IsRequired();


		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.Code).HasColumnName("code");
		builder.Property(entity => entity.Name).HasColumnName("name");
		builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
		builder.Property(entity => entity.AcademicAssignmentId).HasColumnName("academic_assignment_id");
		builder.Property(entity => entity.BranchId).HasColumnName("branch_id").IsRequired();
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.CourseOfferingId).HasColumnName("course_offering_id");
		builder.Property(entity => entity.ClassSectionId).HasColumnName("class_section_id");
		builder.Property(entity => entity.TeachingGroupId).HasColumnName("teaching_group_id");
		builder.Property(entity => entity.TeacherEmployeeId).HasColumnName("teacher_employee_id");
		builder.Property(entity => entity.AssignmentTypeCode).HasColumnName("assignment_type_code");
		builder.Property(entity => entity.Title).HasColumnName("title");
		builder.Property(entity => entity.Description).HasColumnName("description");
		builder.Property(entity => entity.Instructions).HasColumnName("instructions");
		builder.Property(entity => entity.AssignedAt).HasColumnName("assigned_at");
		builder.Property(entity => entity.DueAt).HasColumnName("due_at");
		builder.Property(entity => entity.TotalMarks).HasColumnName("total_marks");
		builder.Property(entity => entity.AllowLateSubmission).HasColumnName("allow_late_submission");
		builder.Property(entity => entity.MaxAttempts).HasColumnName("max_attempts");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
