using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="ClassSectionEntity"/>.
/// </summary>
public sealed class ClassSectionEntityConfiguration
	: IEntityTypeConfiguration<ClassSectionEntity>
{
	public void Configure(EntityTypeBuilder<ClassSectionEntity> builder)
	{
		builder.ToTable("class_section", schema: "academic");
builder.HasKey(entity => entity.ClassSectionId);

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
		builder.Property(entity => entity.ClassSectionId).HasColumnName("class_section_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.CampusId).HasColumnName("campus_id");
		builder.Property(entity => entity.AcademicYearId).HasColumnName("academic_year_id");
		builder.Property(entity => entity.ProgramGradeId).HasColumnName("program_grade_id");
		builder.Property(entity => entity.SectionId).HasColumnName("section_id");
		builder.Property(entity => entity.ClassTeacherEmployeeId).HasColumnName("class_teacher_employee_id");
		builder.Property(entity => entity.RoomId).HasColumnName("room_id");
		builder.Property(entity => entity.Capacity).HasColumnName("capacity");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
