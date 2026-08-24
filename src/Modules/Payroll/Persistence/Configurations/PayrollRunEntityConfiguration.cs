using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="PayrollRunEntity"/>.
/// </summary>
public sealed class PayrollRunEntityConfiguration
	: IEntityTypeConfiguration<PayrollRunEntity>
{
	public void Configure(EntityTypeBuilder<PayrollRunEntity> builder)
	{
		builder.ToTable("payroll_run", schema: "payroll");
<<<<<<< HEAD
builder.HasKey(entity => entity.PayrollRunId);
=======
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.PayrollRunId);
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

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
		builder.Property(entity => entity.PayrollRunId).HasColumnName("payroll_run_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.PayrollPeriodId).HasColumnName("payroll_period_id");
		builder.Property(entity => entity.StatusCode).HasColumnName("status_code");
		builder.Property(entity => entity.ApprovedBy).HasColumnName("approved_by");
		builder.Property(entity => entity.ApprovedAt).HasColumnName("approved_at");
	}
}
