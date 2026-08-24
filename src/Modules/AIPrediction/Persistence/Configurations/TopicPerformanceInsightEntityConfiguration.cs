using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="TopicPerformanceInsightEntity"/>.
/// </summary>
public sealed class TopicPerformanceInsightEntityConfiguration
	: IEntityTypeConfiguration<TopicPerformanceInsightEntity>
{
	public void Configure(EntityTypeBuilder<TopicPerformanceInsightEntity> builder)
	{
		builder.ToTable("topic_performance_insight", schema: "ai");

		builder.HasKey(entity => entity.Id);

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
		builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json");
		builder.Property(entity => entity.Id).HasColumnName("topic_performance_insight_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.ClassPerformanceInsightId).HasColumnName("class_performance_insight_id");
		builder.Property(entity => entity.SubjectId).HasColumnName("subject_id");
		builder.Property(entity => entity.Topic).HasColumnName("topic");
		builder.Property(entity => entity.AverageMasteryScore).HasColumnName("average_mastery_score");
		builder.Property(entity => entity.StudentsStrugglingCount).HasColumnName("students_struggling_count");
		builder.Property(entity => entity.StudentsMasteredCount).HasColumnName("students_mastered_count");
		builder.Property(entity => entity.RiskLevel).HasColumnName("risk_level");
		builder.Property(entity => entity.RecommendedFocus).HasColumnName("recommended_focus");
	}
}
