using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="ClassPerformanceInsightEntity"/>.
/// </summary>
public sealed class ClassPerformanceInsightEntityConfiguration
	: IEntityTypeConfiguration<ClassPerformanceInsightEntity>
{
	public void Configure(EntityTypeBuilder<ClassPerformanceInsightEntity> builder)
	{
		builder.ToTable("class_performance_insight", schema: "ai");
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.ClassPerformanceInsightId);

		builder
			.Property(entity => entity.TenantId)
			.IsRequired();

		builder
			.Property(entity => entity.IsActive)
			.IsRequired();

		builder
			.Property(entity => entity.RowVersion)
			.IsConcurrencyToken();

		builder.HasIndex(entity => entity.TenantId);

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
		builder.Property(entity => entity.ClassPerformanceInsightId).HasColumnName("class_performance_insight_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.AcademicYearId).HasColumnName("academic_year_id");
		builder.Property(entity => entity.TermId).HasColumnName("term_id");
		builder.Property(entity => entity.ClassSectionId).HasColumnName("class_section_id");
		builder.Property(entity => entity.CourseOfferingId).HasColumnName("course_offering_id");
		builder.Property(entity => entity.TeacherEmployeeId).HasColumnName("teacher_employee_id");
		builder.Property(entity => entity.StudentsCount).HasColumnName("students_count");
		builder.Property(entity => entity.OnTrackCount).HasColumnName("on_track_count");
		builder.Property(entity => entity.NeedsAttentionCount).HasColumnName("needs_attention_count");
		builder.Property(entity => entity.HighRiskCount).HasColumnName("high_risk_count");
		builder.Property(entity => entity.PredictedClassAverage).HasColumnName("predicted_class_average");
		builder.Property(entity => entity.CurrentClassAverage).HasColumnName("current_class_average");
		builder.Property(entity => entity.Trend).HasColumnName("trend");
		builder.Property(entity => entity.Summary).HasColumnName("summary");
		builder.Property(entity => entity.GeneratedAt).HasColumnName("generated_at");
	}
}
