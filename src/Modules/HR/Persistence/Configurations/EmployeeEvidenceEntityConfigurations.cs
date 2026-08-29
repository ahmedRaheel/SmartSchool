using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;
namespace SmartSchool.Modules.HR.Persistence.Configurations;
public sealed class EmployeeEducationEntityConfiguration : IEntityTypeConfiguration<EmployeeEducationEntity>
{
	public void Configure(EntityTypeBuilder<EmployeeEducationEntity> builder)
	{
		builder.ToTable("employee_education", "hr");
		builder.HasKey(x => x.EmployeeEducationId);
		builder.Property(x => x.EmployeeEducationId).HasColumnName("employee_education_id");
		builder.Property(x => x.TenantId).HasColumnName("tenant_id");
		builder.Property(x => x.EmployeeId).HasColumnName("employee_id");
		builder.Property(x => x.Qualification).HasColumnName("qualification");
		builder.Property(x => x.Institute).HasColumnName("institute");
		builder.Property(x => x.FieldOfStudy).HasColumnName("field_of_study");
		builder.Property(x => x.StartDate).HasColumnName("start_date");
		builder.Property(x => x.EndDate).HasColumnName("end_date");
		builder.Property(x => x.Grade).HasColumnName("grade");
		builder.Property(x => x.IsHighest).HasColumnName("is_highest");
		builder.Property(x => x.CreatedAt).HasColumnName("created_at");
		builder.Ignore(x => x.IsActive);
		builder.Ignore(x => x.UpdatedAt);
		builder.Ignore(x => x.RowVersion);
	}
}
