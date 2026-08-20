using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

public sealed class MlPredictionResultEntityConfiguration : IEntityTypeConfiguration<MlPredictionResultEntity>
{
	public void Configure(EntityTypeBuilder<MlPredictionResultEntity> builder)
	{
		builder.ToTable("ml_prediction_result", "ai");
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
	}
}
