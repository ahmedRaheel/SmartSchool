using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence.Configurations;

public sealed class JobGradeProjectionEntityConfiguration : IEntityTypeConfiguration<JobGradeProjectionEntity>
{
    public void Configure(EntityTypeBuilder<JobGradeProjectionEntity> builder)
    {
        builder.ToTable("job_grade_projection", "payroll");
        builder.HasKey(x => x.JobGradeId);
        builder.Property(x => x.JobGradeId).HasColumnName("job_grade_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.JobGradeId }).IsUnique();
    }
}
