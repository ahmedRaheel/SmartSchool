using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence.Configurations;

public sealed class WorkflowDefinitionEntityConfiguration : IEntityTypeConfiguration<WorkflowDefinitionEntity>
{
    public void Configure(EntityTypeBuilder<WorkflowDefinitionEntity> builder)
    {
        builder.ToTable("workflowdefinition", "workflow");
        builder.HasKey(x => x.WorkflowDefinitionId);
        builder.Property(x => x.WorkflowDefinitionId).HasColumnName("workflow_definition_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description").HasMaxLength(2000);
        builder.Property(x => x.TriggerType).HasColumnName("trigger_type").HasMaxLength(50).IsRequired();
        builder.Property(x => x.EntityType).HasColumnName("entity_type").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(x => x.Version).HasColumnName("version").IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();
        builder.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.EntityType, x.Status });
    }
}
