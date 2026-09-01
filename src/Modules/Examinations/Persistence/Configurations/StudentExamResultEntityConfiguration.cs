using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="StudentExamResultEntity"/>.
/// </summary>
public sealed class StudentExamResultEntityConfiguration
	: IEntityTypeConfiguration<StudentExamResultEntity>
{
	public void Configure(EntityTypeBuilder<StudentExamResultEntity> builder)
	{
		builder.ToTable("student_exam_result", schema: "exam");
		builder.HasKey(entity => entity.StudentExamResultId);

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
		builder.Property(entity => entity.StudentExamResultId).HasColumnName("student_exam_result_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.ExamSubjectId).HasColumnName("exam_subject_id");
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.MarksObtained).HasColumnName("marks_obtained");
		builder.Property(entity => entity.Percentage).HasColumnName("percentage");
		builder.Property(entity => entity.Grade).HasColumnName("grade");
		builder.Property(entity => entity.IsAbsent).HasColumnName("is_absent");
		builder.Property(entity => entity.Remarks).HasColumnName("remarks");
		builder.Property(entity => entity.EnteredBy).HasColumnName("entered_by");
		builder.Property(entity => entity.VerifiedBy).HasColumnName("verified_by");

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
        builder.HasOne<ExamSubjectEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.ExamSubjectId)
            .OnDelete(DeleteBehavior.Restrict);

	}
}
