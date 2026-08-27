using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="CourseSelectionEntity"/>.
/// </summary>
public sealed class CourseSelectionEntityConfiguration
	: IEntityTypeConfiguration<CourseSelectionEntity>
{
	public void Configure(EntityTypeBuilder<CourseSelectionEntity> builder)
	{
		builder.ToTable("student_course_enrollment", schema: "student");
		builder.HasKey(entity => entity.StudentCourseEnrollmentId);

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
		builder.Property(entity => entity.StudentCourseEnrollmentId).HasColumnName("student_course_enrollment_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentEnrollmentId).HasColumnName("student_enrollment_id");
		builder.Property(entity => entity.CourseOfferingId).HasColumnName("course_offering_id");
		builder.Property(entity => entity.EnrollmentTypeCode).HasColumnName("enrollment_type_code");
		builder.Property(entity => entity.SelectedAt).HasColumnName("selected_at");
		builder.Property(entity => entity.ApprovedBy).HasColumnName("approved_by");
		builder.Property(entity => entity.ApprovedAt).HasColumnName("approved_at");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
