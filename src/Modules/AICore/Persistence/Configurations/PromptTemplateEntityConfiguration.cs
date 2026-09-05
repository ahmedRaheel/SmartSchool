using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="PromptTemplateEntity"/>.
/// </summary>
public sealed class PromptTemplateEntityConfiguration
    : IEntityTypeConfiguration<PromptTemplateEntity>
{
    public void Configure(EntityTypeBuilder<PromptTemplateEntity> builder)
    {
        builder.ToTable("prompt_template", schema: "ai_core");
        builder.HasKey(entity => entity.PromptTemplateId);

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
        builder.Property(entity => entity.PromptTemplateId).HasColumnName("prompt_template_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

        // Database columns synchronized from SmartSchoolComplete.sql.
        builder.Property(entity => entity.AssistantType).HasColumnName("assistant_type");
        builder.Property(entity => entity.PromptType).HasColumnName("prompt_type");
        builder.Property(entity => entity.PromptText).HasColumnName("prompt_text");
        builder.Property(entity => entity.Version).HasColumnName("version");
    }
}
