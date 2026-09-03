using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Finance.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="EmployeeCompensationEntity"/>.
/// </summary>
public sealed class EmployeeCompensationEntityConfiguration
	: IEntityTypeConfiguration<EmployeeCompensationEntity>
{
	public void Configure(EntityTypeBuilder<EmployeeCompensationEntity> builder)
	{
		builder.ToTable("employee_compensation", schema: "hr");
		builder.HasKey(entity => entity.EmployeeCompensationId);

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
		builder.Property(entity => entity.EmployeeCompensationId).HasColumnName("employee_compensation_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.EmployeeId).HasColumnName("employee_id");
		builder.Property(entity => entity.JobGradeId).HasColumnName("job_grade_id");
		builder.Property(entity => entity.EffectiveFrom).HasColumnName("effective_from");
		builder.Property(entity => entity.EffectiveTo).HasColumnName("effective_to");
		builder.Property(entity => entity.BasicSalary).HasColumnName("basic_salary");
		builder.Property(entity => entity.GrossSalary).HasColumnName("gross_salary");
		builder.Property(entity => entity.CurrencyCode).HasColumnName("currency_code");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
