using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

public sealed class MlPredictionResultEntityConfiguration : IEntityTypeConfiguration<MlPredictionResultEntity>
{
	public void Configure(EntityTypeBuilder<MlPredictionResultEntity> builder)
	{
		builder.ToTable("prediction_result", schema: "ai");
		builder.HasKey(x=>x.Id);
		builder.Property(x=>x.Id).HasColumnName("ml_prediction_result_id");
		builder.Property(x=>x.TenantId).HasColumnName("tenant_id").IsRequired();
		builder.Property(x=>x.PredictionType).HasColumnName("prediction_type").HasMaxLength(80).IsRequired();
		builder.Property(x=>x.StudentId).HasColumnName("student_id");
		builder.Property(x=>x.SubjectId).HasColumnName("subject_id");
		builder.Property(x=>x.RelatedEntityId).HasColumnName("related_entity_id");
		builder.Property(x=>x.Score).HasColumnName("score").HasPrecision(8,4);
		builder.Property(x=>x.Probability).HasColumnName("probability").HasPrecision(8,6);
		builder.Property(x=>x.RiskLevel).HasColumnName("risk_level").HasMaxLength(30);
		builder.Property(x=>x.Outcome).HasColumnName("outcome").HasMaxLength(80);
		builder.Property(x=>x.ConfidenceScore).HasColumnName("confidence_score").HasPrecision(8,6);
		builder.Property(x=>x.ModelVersion).HasColumnName("model_version").HasMaxLength(80);
		builder.Property(x=>x.UsedMachineLearning).HasColumnName("used_machine_learning");
		builder.Property(x=>x.FactorsJson).HasColumnName("factors_json").HasColumnType("jsonb");
		builder.Property(x=>x.GeneratedAt).HasColumnName("generated_at");
		builder.Property(x=>x.IsActive).HasColumnName("is_active").IsRequired();
		builder.Property(x=>x.CreatedAt).HasColumnName("created_at").IsRequired();
		builder.Property(x=>x.UpdatedAt).HasColumnName("updated_at");
		builder.Property(x=>x.RowVersion).HasColumnName("row_version").IsConcurrencyToken();
		builder.HasIndex(x=>new{x.TenantId,x.PredictionType,x.StudentId});

		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.PredictionType).HasColumnName("prediction_type");
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.SubjectId).HasColumnName("subject_id");
		builder.Property(entity => entity.RelatedEntityId).HasColumnName("related_entity_id");
		builder.Property(entity => entity.Score).HasColumnName("score");
		builder.Property(entity => entity.Probability).HasColumnName("probability");
		builder.Property(entity => entity.RiskLevel).HasColumnName("risk_level");
		builder.Property(entity => entity.Outcome).HasColumnName("outcome");
		builder.Property(entity => entity.ConfidenceScore).HasColumnName("confidence_score");
		builder.Property(entity => entity.ModelVersion).HasColumnName("model_version");
		builder.Property(entity => entity.UsedMachineLearning).HasColumnName("used_machine_learning");
		builder.Property(entity => entity.FactorsJson).HasColumnName("factors_json");
		builder.Property(entity => entity.GeneratedAt).HasColumnName("generated_at");
		builder.Property(entity => entity.Id).HasColumnName("prediction_model_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");
	}
}
