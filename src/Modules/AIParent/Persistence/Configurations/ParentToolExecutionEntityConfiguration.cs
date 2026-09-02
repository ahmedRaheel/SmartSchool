using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="ParentToolExecutionEntity"/>.
/// </summary>
public sealed class ParentToolExecutionEntityConfiguration
	: IEntityTypeConfiguration<ParentToolExecutionEntity>
{
	public void Configure(EntityTypeBuilder<ParentToolExecutionEntity> builder)
	{
		builder.ToTable("parent_tool_execution", schema: "ai_core");
		builder.HasKey(entity => entity.ParentToolExecutionId);

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
		builder.Property(entity => entity.ParentToolExecutionId).HasColumnName("parent_tool_execution_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.ParentConversationId).HasColumnName("parent_conversation_id");
		builder.Property(entity => entity.ToolDefinitionId).HasColumnName("tool_definition_id");
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.InputPayload).HasColumnName("input_payload");
		builder.Property(entity => entity.OutputPayload).HasColumnName("output_payload");
		builder.Property(entity => entity.Status).HasColumnName("status");
		builder.Property(entity => entity.ExecutedAt).HasColumnName("executed_at");

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
        builder.HasOne<ParentConversationEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.ParentConversationId)
            .OnDelete(DeleteBehavior.Restrict);

	}
}
