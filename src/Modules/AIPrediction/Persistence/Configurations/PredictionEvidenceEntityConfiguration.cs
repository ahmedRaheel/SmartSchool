using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="PredictionEvidenceEntity"/>.
/// </summary>
public sealed class PredictionEvidenceEntityConfiguration
	: IEntityTypeConfiguration<PredictionEvidenceEntity>
{
	public void Configure(EntityTypeBuilder<PredictionEvidenceEntity> builder)
	{
		builder.ToTable("prediction_evidence", schema: "ai");
		builder.HasKey(entity => entity.PredictionEvidenceId);

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
		builder.Property(entity => entity.PredictionEvidenceId).HasColumnName("prediction_evidence_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentPerformancePredictionId).HasColumnName("student_performance_prediction_id");
		builder.Property(entity => entity.EvidenceType).HasColumnName("evidence_type");
		builder.Property(entity => entity.SourceEntityType).HasColumnName("source_entity_type");
		builder.Property(entity => entity.SourceEntityId).HasColumnName("source_entity_id");
		builder.Property(entity => entity.NumericValue).HasColumnName("numeric_value");
		builder.Property(entity => entity.TextValue).HasColumnName("text_value");
		builder.Property(entity => entity.NormalizedValue).HasColumnName("normalized_value");
		builder.Property(entity => entity.Weight).HasColumnName("weight");
		builder.Property(entity => entity.OccurredAt).HasColumnName("occurred_at");
		builder.Property(entity => entity.Explanation).HasColumnName("explanation");
	}
}
