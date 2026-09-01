using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="AiExecutionLogEntity"/>.
/// </summary>
public sealed class AiExecutionLogEntityConfiguration
	: IEntityTypeConfiguration<AiExecutionLogEntity>
{
	public void Configure(EntityTypeBuilder<AiExecutionLogEntity> builder)
	{
		builder.ToTable("ai_execution_log", schema: "ai_core");
		builder.HasKey(entity => entity.AiExecutionLogId);

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
		builder.Property(entity => entity.AiExecutionLogId).HasColumnName("ai_execution_log_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.AssistantType).HasColumnName("assistant_type");
		builder.Property(entity => entity.ConversationReferenceId).HasColumnName("conversation_reference_id");
		builder.Property(entity => entity.UserId).HasColumnName("user_id");
		builder.Property(entity => entity.ModelConfigurationId).HasColumnName("model_configuration_id");
		builder.Property(entity => entity.PromptTokens).HasColumnName("prompt_tokens");
		builder.Property(entity => entity.CompletionTokens).HasColumnName("completion_tokens");
		builder.Property(entity => entity.TotalTokens).HasColumnName("total_tokens");
		builder.Property(entity => entity.EstimatedCost).HasColumnName("estimated_cost");
		builder.Property(entity => entity.LatencyMs).HasColumnName("latency_ms");
		builder.Property(entity => entity.Status).HasColumnName("status");
		builder.Property(entity => entity.CorrelationId).HasColumnName("correlation_id");

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
        builder.HasOne<ModelConfigurationEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.ModelConfigurationId)
            .OnDelete(DeleteBehavior.Restrict);

	}
}
