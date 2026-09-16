using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence.Configurations;

public sealed class ApprovalEntityConfiguration : IEntityTypeConfiguration<ApprovalEntity>
{
    public void Configure(EntityTypeBuilder<ApprovalEntity> builder)
    {
        builder.ToTable("approval", "workflow");
        builder.HasKey(x => x.ApprovalId);
        builder.Property(x => x.ApprovalId).HasColumnName("approval_id");
        builder.Property(x => x.WorkflowInstanceId).HasColumnName("workflow_instance_id").IsRequired();
        builder.Property(x => x.WorkflowStepId).HasColumnName("workflow_step_id").IsRequired();
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(x => x.AssignedRole).HasColumnName("assigned_role").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(x => x.RequestedAt).HasColumnName("requested_at").IsRequired();
        builder.Property(x => x.DecisionAt).HasColumnName("decision_at");
        builder.Property(x => x.DecidedByUserId).HasColumnName("decided_by_user_id");
        builder.Property(x => x.Comments).HasColumnName("comments").HasMaxLength(2000);
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();
        builder.HasIndex(x => new { x.TenantId, x.Status, x.AssignedRole });
        builder.HasIndex(x => new { x.TenantId, x.WorkflowInstanceId, x.WorkflowStepId }).IsUnique();
        builder.HasOne<WorkflowInstanceEntity>().WithMany().HasForeignKey(x => x.WorkflowInstanceId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<WorkflowStepEntity>().WithMany().HasForeignKey(x => x.WorkflowStepId).OnDelete(DeleteBehavior.Restrict);
    }
}
