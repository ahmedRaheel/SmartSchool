using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="LearningRecommendationEntity"/>.
/// </summary>
public sealed class LearningRecommendationEntityConfiguration
	: IEntityTypeConfiguration<LearningRecommendationEntity>
{
	public void Configure(EntityTypeBuilder<LearningRecommendationEntity> builder)
	{
		builder.ToTable("learning_recommendation", schema: "ai_tutor");

		builder.HasKey(entity => entity.Id);

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
		builder.Property(entity => entity.Id).HasColumnName("learning_recommendation_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.SubjectId).HasColumnName("subject_id");
		builder.Property(entity => entity.Topic).HasColumnName("topic");
		builder.Property(entity => entity.RecommendationType).HasColumnName("recommendation_type");
		builder.Property(entity => entity.RecommendationText).HasColumnName("recommendation_text");
		builder.Property(entity => entity.Priority).HasColumnName("priority");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
