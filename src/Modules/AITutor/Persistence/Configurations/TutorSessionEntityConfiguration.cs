using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="TutorSessionEntity"/>.
/// </summary>
public sealed class TutorSessionEntityConfiguration
	: IEntityTypeConfiguration<TutorSessionEntity>
{
	public void Configure(EntityTypeBuilder<TutorSessionEntity> builder)
	{
		builder.ToTable("tutor_session", schema: "ai_tutor");
		builder.HasKey(entity => entity.TutorSessionId);

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
		builder.Property(entity => entity.TutorSessionId).HasColumnName("tutor_session_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.TutorConversationId).HasColumnName("tutor_conversation_id");
		builder.Property(entity => entity.Topic).HasColumnName("topic");
		builder.Property(entity => entity.LearningObjective).HasColumnName("learning_objective");
		builder.Property(entity => entity.StartedAt).HasColumnName("started_at");
		builder.Property(entity => entity.EndedAt).HasColumnName("ended_at");
		builder.Property(entity => entity.SessionSummary).HasColumnName("session_summary");

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
        builder.HasOne<TutorConversationEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.TutorConversationId)
            .OnDelete(DeleteBehavior.Restrict);

	}
}
