using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence.Configurations;

public sealed class WorkflowStepEntityConfiguration : IEntityTypeConfiguration<WorkflowStepEntity>
{
    public void Configure(EntityTypeBuilder<WorkflowStepEntity> builder)
    {
        builder.ToTable("workflowstep", "workflow");
        builder.HasKey(x => x.WorkflowStepId);
        builder.Property(x => x.WorkflowStepId).HasColumnName("workflow_step_id");
        builder.Property(x => x.WorkflowDefinitionId).HasColumnName("workflow_definition_id").IsRequired();
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(x => x.StepOrder).HasColumnName("step_order").IsRequired();
        builder.Property(x => x.StepType).HasColumnName("step_type").HasMaxLength(30).IsRequired();
        builder.Property(x => x.ApproverRole).HasColumnName("approver_role").HasMaxLength(100);
        builder.Property(x => x.ActionCode).HasColumnName("action_code").HasMaxLength(100);
        builder.Property(x => x.IsRequired).HasColumnName("is_required").IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();
        builder.HasIndex(x => new { x.TenantId, x.WorkflowDefinitionId, x.StepOrder }).IsUnique();
        builder.HasOne<WorkflowDefinitionEntity>().WithMany().HasForeignKey(x => x.WorkflowDefinitionId).OnDelete(DeleteBehavior.Cascade);
    }
}
