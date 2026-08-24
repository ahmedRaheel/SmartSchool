using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="TeacherAssignmentEntity"/>.
/// </summary>
public sealed class TeacherAssignmentEntityConfiguration
	: IEntityTypeConfiguration<TeacherAssignmentEntity>
{
	public void Configure(EntityTypeBuilder<TeacherAssignmentEntity> builder)
	{
		builder.ToTable("teacher_course_assignment", schema: "academic");
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.TeacherCourseAssignmentId);

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
		builder.Property(entity => entity.TeacherCourseAssignmentId).HasColumnName("teacher_course_assignment_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.CourseOfferingId).HasColumnName("course_offering_id");
		builder.Property(entity => entity.EmployeeId).HasColumnName("employee_id");
		builder.Property(entity => entity.ClassSectionId).HasColumnName("class_section_id");
		builder.Property(entity => entity.TeachingGroupId).HasColumnName("teaching_group_id");
		builder.Property(entity => entity.AssignmentRole).HasColumnName("assignment_role");
		builder.Property(entity => entity.PeriodsPerWeek).HasColumnName("periods_per_week");
		builder.Property(entity => entity.EffectiveFrom).HasColumnName("effective_from");
		builder.Property(entity => entity.EffectiveTo).HasColumnName("effective_to");
		builder.Property(entity => entity.IsPrimary).HasColumnName("is_primary");
	}
}
