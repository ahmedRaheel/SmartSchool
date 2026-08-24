using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="ExamEntity"/>.
/// </summary>
public sealed class ExamEntityConfiguration
	: IEntityTypeConfiguration<ExamEntity>
{
	public void Configure(EntityTypeBuilder<ExamEntity> builder)
	{
		builder.ToTable("exam", schema: "exam");
<<<<<<< HEAD
builder.HasKey(entity => entity.ExamId);
=======
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.ExamId);
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

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
		builder.Property(entity => entity.ExamId).HasColumnName("exam_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.CampusId).HasColumnName("campus_id");
		builder.Property(entity => entity.AcademicYearId).HasColumnName("academic_year_id");
		builder.Property(entity => entity.TermId).HasColumnName("term_id");
		builder.Property(entity => entity.AcademicSystemId).HasColumnName("academic_system_id");
		builder.Property(entity => entity.ExamTypeCode).HasColumnName("exam_type_code");
		builder.Property(entity => entity.StartDate).HasColumnName("start_date");
		builder.Property(entity => entity.EndDate).HasColumnName("end_date");
		builder.Property(entity => entity.ResultPublishDate).HasColumnName("result_publish_date");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
