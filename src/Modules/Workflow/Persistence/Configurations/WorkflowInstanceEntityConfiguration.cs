using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence.Configurations;

public sealed class WorkflowInstanceEntityConfiguration : IEntityTypeConfiguration<WorkflowInstanceEntity>
{
    public void Configure(EntityTypeBuilder<WorkflowInstanceEntity> builder)
    {
        builder.ToTable("workflowinstance", "workflow");
        builder.HasKey(x => x.WorkflowInstanceId);
        builder.Property(x => x.WorkflowInstanceId).HasColumnName("workflow_instance_id");
        builder.Property(x => x.WorkflowDefinitionId).HasColumnName("workflow_definition_id").IsRequired();
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(x => x.EntityType).HasColumnName("entity_type").HasMaxLength(100).IsRequired();
        builder.Property(x => x.EntityId).HasColumnName("entity_id");
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(x => x.CurrentStepOrder).HasColumnName("current_step_order").IsRequired();
        builder.Property(x => x.StartedByUserId).HasColumnName("started_by_user_id").IsRequired();
        builder.Property(x => x.StartedAt).HasColumnName("started_at").IsRequired();
        builder.Property(x => x.CompletedAt).HasColumnName("completed_at");
        builder.Property(x => x.ContextJson).HasColumnName("context_json").HasColumnType("jsonb");
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();
        builder.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.Status, x.StartedAt });
        builder.HasOne<WorkflowDefinitionEntity>().WithMany().HasForeignKey(x => x.WorkflowDefinitionId).OnDelete(DeleteBehavior.Restrict);
    }
}
