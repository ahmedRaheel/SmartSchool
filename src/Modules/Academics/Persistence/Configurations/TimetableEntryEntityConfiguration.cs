using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="TimetableEntryEntity"/>.
/// </summary>
public sealed class TimetableEntryEntityConfiguration
	: IEntityTypeConfiguration<TimetableEntryEntity>
{
	public void Configure(EntityTypeBuilder<TimetableEntryEntity> builder)
	{
		builder.ToTable("timetable_entry", schema: "academic");
		builder.HasKey(entity => entity.TimetableEntryId);

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
		builder.Property(entity => entity.TimetableEntryId).HasColumnName("timetable_entry_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.TimetableId).HasColumnName("timetable_id");
		builder.Property(entity => entity.DayOfWeek).HasColumnName("day_of_week");
		builder.Property(entity => entity.TimetablePeriodId).HasColumnName("timetable_period_id");
		builder.Property(entity => entity.ClassSectionId).HasColumnName("class_section_id");
		builder.Property(entity => entity.TeachingGroupId).HasColumnName("teaching_group_id");
		builder.Property(entity => entity.CourseOfferingId).HasColumnName("course_offering_id");
		builder.Property(entity => entity.TeacherCourseAssignmentId).HasColumnName("teacher_course_assignment_id");
		builder.Property(entity => entity.RoomId).HasColumnName("room_id");
		builder.Property(entity => entity.EntryType).HasColumnName("entry_type");
	}
}
