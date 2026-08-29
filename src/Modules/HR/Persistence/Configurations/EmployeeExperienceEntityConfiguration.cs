using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;
namespace SmartSchool.Modules.HR.Persistence.Configurations;

public sealed class EmployeeExperienceEntityConfiguration : IEntityTypeConfiguration<EmployeeExperienceEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeExperienceEntity> builder)
    {
        builder.ToTable("employee_experience", "hr");
        builder.HasKey(x => x.EmployeeExperienceId);
        builder.Property(x => x.EmployeeExperienceId).HasColumnName("employee_experience_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.EmployeeId).HasColumnName("employee_id");
        builder.Property(x => x.Employer).HasColumnName("employer");
        builder.Property(x => x.JobTitle).HasColumnName("job_title");
        builder.Property(x => x.StartDate).HasColumnName("start_date");
        builder.Property(x => x.EndDate).HasColumnName("end_date");
        builder.Property(x => x.Responsibilities).HasColumnName("responsibilities");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.UpdatedAt);
        builder.Ignore(x => x.RowVersion);
    }
}
