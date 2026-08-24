using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="PredictionEvaluationEntity"/>.
/// </summary>
public sealed class PredictionEvaluationEntityConfiguration
	: IEntityTypeConfiguration<PredictionEvaluationEntity>
{
	public void Configure(EntityTypeBuilder<PredictionEvaluationEntity> builder)
	{
		builder.ToTable("prediction_evaluation", schema: "ai");
builder.HasKey(entity => entity.PredictionEvaluationId);

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
		builder.Property(entity => entity.PredictionEvaluationId).HasColumnName("prediction_evaluation_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentPerformancePredictionId).HasColumnName("student_performance_prediction_id");
		builder.Property(entity => entity.StudentExamResultId).HasColumnName("student_exam_result_id");
		builder.Property(entity => entity.PredictedPercentage).HasColumnName("predicted_percentage");
		builder.Property(entity => entity.ActualPercentage).HasColumnName("actual_percentage");
		builder.Property(entity => entity.AbsoluteError).HasColumnName("absolute_error");
		builder.Property(entity => entity.PredictedGrade).HasColumnName("predicted_grade");
		builder.Property(entity => entity.ActualGrade).HasColumnName("actual_grade");
		builder.Property(entity => entity.GradeCorrect).HasColumnName("grade_correct");
		builder.Property(entity => entity.EvaluatedAt).HasColumnName("evaluated_at");
	}
}
