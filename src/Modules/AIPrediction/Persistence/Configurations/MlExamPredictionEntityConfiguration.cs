using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

public sealed class MlExamPredictionEntityConfiguration : IEntityTypeConfiguration<MlExamPredictionEntity>
{
	public void Configure(EntityTypeBuilder<MlExamPredictionEntity> builder)
	{
		builder.ToTable("prediction", schema: "ai");
<<<<<<< HEAD
builder.HasKey(x => x.MlExamPredictionId);
		builder.Property(x => x.MlExamPredictionId).HasColumnName("ml_exam_prediction_id");
=======
		builder.Ignore(entity => entity.Id);
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).HasColumnName("ml_exam_prediction_id");
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0
		builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
		builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
		builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
		builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
		builder.Property(x => x.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();
		builder.Property(x => x.StudentId).HasColumnName("student_id").IsRequired();
		builder.Property(x => x.SubjectId).HasColumnName("subject_id").IsRequired();
		builder.Property(x => x.TargetExamId).HasColumnName("target_exam_id");
		builder.Property(x => x.TargetExamSubjectId).HasColumnName("target_exam_subject_id");
		builder.Property(x => x.TargetExamTypeCode).HasColumnName("target_exam_type_code").HasMaxLength(40).IsRequired();
		builder.Property(x => x.PredictedMarks).HasColumnName("predicted_marks").HasPrecision(8, 2);
		builder.Property(x => x.PredictedPercentage).HasColumnName("predicted_percentage").HasPrecision(7, 3);
		builder.Property(x => x.PredictedGrade).HasColumnName("predicted_grade").HasMaxLength(20);
		builder.Property(x => x.LowerBoundPercentage).HasColumnName("lower_bound_percentage").HasPrecision(7, 3);
		builder.Property(x => x.UpperBoundPercentage).HasColumnName("upper_bound_percentage").HasPrecision(7, 3);
		builder.Property(x => x.ConfidenceScore).HasColumnName("confidence_score").HasPrecision(7, 4);
		builder.Property(x => x.PassProbability).HasColumnName("pass_probability").HasPrecision(7, 4);
		builder.Property(x => x.Trend).HasColumnName("trend").HasMaxLength(30);
		builder.Property(x => x.RiskLevel).HasColumnName("risk_level").HasMaxLength(30);
		builder.Property(x => x.ModelVersion).HasColumnName("model_version").HasMaxLength(80);
		builder.Property(x => x.HistoricalResultCount).HasColumnName("historical_result_count");
		builder.Property(x => x.UsedMachineLearning).HasColumnName("used_machine_learning");
		builder.Property(x => x.GeneratedAt).HasColumnName("generated_at").IsRequired();
		builder.HasIndex(x => new { x.TenantId, x.StudentId, x.SubjectId, x.GeneratedAt });

		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.SubjectId).HasColumnName("subject_id");
		builder.Property(entity => entity.TargetExamId).HasColumnName("target_exam_id");
		builder.Property(entity => entity.TargetExamSubjectId).HasColumnName("target_exam_subject_id");
		builder.Property(entity => entity.TargetExamTypeCode).HasColumnName("target_exam_type_code");
		builder.Property(entity => entity.PredictedMarks).HasColumnName("predicted_marks");
		builder.Property(entity => entity.PredictedPercentage).HasColumnName("predicted_percentage");
		builder.Property(entity => entity.PredictedGrade).HasColumnName("predicted_grade");
		builder.Property(entity => entity.LowerBoundPercentage).HasColumnName("lower_bound_percentage");
		builder.Property(entity => entity.UpperBoundPercentage).HasColumnName("upper_bound_percentage");
		builder.Property(entity => entity.ConfidenceScore).HasColumnName("confidence_score");
		builder.Property(entity => entity.PassProbability).HasColumnName("pass_probability");
		builder.Property(entity => entity.Trend).HasColumnName("trend");
		builder.Property(entity => entity.RiskLevel).HasColumnName("risk_level");
		builder.Property(entity => entity.ModelVersion).HasColumnName("model_version");
		builder.Property(entity => entity.HistoricalResultCount).HasColumnName("historical_result_count");
		builder.Property(entity => entity.UsedMachineLearning).HasColumnName("used_machine_learning");
		builder.Property(entity => entity.GeneratedAt).HasColumnName("generated_at");
		builder.Property(entity => entity.PredictionId).HasColumnName("prediction_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.PredictionModelId).HasColumnName("prediction_model_id");
		builder.Property(entity => entity.PredictionType).HasColumnName("prediction_type");
		builder.Property(entity => entity.Score).HasColumnName("score");
		builder.Property(entity => entity.Explanation).HasColumnName("explanation");
		builder.Property(entity => entity.PredictedAt).HasColumnName("predicted_at");
	}
}
