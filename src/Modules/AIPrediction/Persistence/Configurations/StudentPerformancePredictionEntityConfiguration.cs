using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="StudentPerformancePredictionEntity"/>.
/// </summary>
public sealed class StudentPerformancePredictionEntityConfiguration
	: IEntityTypeConfiguration<StudentPerformancePredictionEntity>
{
	public void Configure(EntityTypeBuilder<StudentPerformancePredictionEntity> builder)
	{
		builder.ToTable("student_performance_prediction", schema: "ai");
		builder.HasKey(entity => entity.StudentPerformancePredictionId);

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
		builder.Property(entity => entity.StudentPerformancePredictionId).HasColumnName("student_performance_prediction_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.AcademicYearId).HasColumnName("academic_year_id");
		builder.Property(entity => entity.TermId).HasColumnName("term_id");
		builder.Property(entity => entity.CourseOfferingId).HasColumnName("course_offering_id");
		builder.Property(entity => entity.SubjectId).HasColumnName("subject_id");
		builder.Property(entity => entity.TargetExamId).HasColumnName("target_exam_id");
		builder.Property(entity => entity.TargetExamSubjectId).HasColumnName("target_exam_subject_id");
		builder.Property(entity => entity.TargetExamTypeCode).HasColumnName("target_exam_type_code");
		builder.Property(entity => entity.TargetDate).HasColumnName("target_date");
		builder.Property(entity => entity.PredictedMarks).HasColumnName("predicted_marks");
		builder.Property(entity => entity.PredictedPercentage).HasColumnName("predicted_percentage");
		builder.Property(entity => entity.PredictedGrade).HasColumnName("predicted_grade");
		builder.Property(entity => entity.LowerBoundPercentage).HasColumnName("lower_bound_percentage");
		builder.Property(entity => entity.UpperBoundPercentage).HasColumnName("upper_bound_percentage");
		builder.Property(entity => entity.ConfidenceScore).HasColumnName("confidence_score");
		builder.Property(entity => entity.PassProbability).HasColumnName("pass_probability");
		builder.Property(entity => entity.FailProbability).HasColumnName("fail_probability");
		builder.Property(entity => entity.TargetGrade).HasColumnName("target_grade");
		builder.Property(entity => entity.TargetGradeProbability).HasColumnName("target_grade_probability");
		builder.Property(entity => entity.Trend).HasColumnName("trend");
		builder.Property(entity => entity.RiskLevel).HasColumnName("risk_level");
		builder.Property(entity => entity.ExplanationSummary).HasColumnName("explanation_summary");
		builder.Property(entity => entity.Explanation).HasColumnName("explanation");
		builder.Property(entity => entity.PredictionModelId).HasColumnName("prediction_model_id");
		builder.Property(entity => entity.ModelVersion).HasColumnName("model_version");
		builder.Property(entity => entity.GeneratedAt).HasColumnName("generated_at");
		builder.Property(entity => entity.ExpiresAt).HasColumnName("expires_at");
	}
}
