using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;
namespace SmartSchool.Modules.HR.Persistence.Configurations;
public sealed class EmployeeEducationEntityConfiguration : IEntityTypeConfiguration<EmployeeEducationEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEducationEntity> b) { b.ToTable("employee_education","hr"); b.HasKey(x=>x.EmployeeEducationId); b.Property(x=>x.EmployeeEducationId).HasColumnName("employee_education_id"); b.Property(x=>x.TenantId).HasColumnName("tenant_id"); b.Property(x=>x.EmployeeId).HasColumnName("employee_id"); b.Property(x=>x.Qualification).HasColumnName("qualification"); b.Property(x=>x.Institute).HasColumnName("institute"); b.Property(x=>x.FieldOfStudy).HasColumnName("field_of_study"); b.Property(x=>x.StartDate).HasColumnName("start_date"); b.Property(x=>x.EndDate).HasColumnName("end_date"); b.Property(x=>x.Grade).HasColumnName("grade"); b.Property(x=>x.IsHighest).HasColumnName("is_highest"); b.Property(x=>x.CreatedAt).HasColumnName("created_at"); b.Ignore(x=>x.IsActive); b.Ignore(x=>x.UpdatedAt); b.Ignore(x=>x.RowVersion); }
}
public sealed class EmployeeExperienceEntityConfiguration : IEntityTypeConfiguration<EmployeeExperienceEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeExperienceEntity> b) { b.ToTable("employee_experience","hr"); b.HasKey(x=>x.EmployeeExperienceId); b.Property(x=>x.EmployeeExperienceId).HasColumnName("employee_experience_id"); b.Property(x=>x.TenantId).HasColumnName("tenant_id"); b.Property(x=>x.EmployeeId).HasColumnName("employee_id"); b.Property(x=>x.Employer).HasColumnName("employer"); b.Property(x=>x.JobTitle).HasColumnName("job_title"); b.Property(x=>x.StartDate).HasColumnName("start_date"); b.Property(x=>x.EndDate).HasColumnName("end_date"); b.Property(x=>x.Responsibilities).HasColumnName("responsibilities"); b.Property(x=>x.CreatedAt).HasColumnName("created_at"); b.Ignore(x=>x.IsActive); b.Ignore(x=>x.UpdatedAt); b.Ignore(x=>x.RowVersion); }
}
