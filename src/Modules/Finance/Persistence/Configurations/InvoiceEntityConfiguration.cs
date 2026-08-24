using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="InvoiceEntity"/>.
/// </summary>
public sealed class InvoiceEntityConfiguration
	: IEntityTypeConfiguration<InvoiceEntity>
{
	public void Configure(EntityTypeBuilder<InvoiceEntity> builder)
	{
		builder.ToTable("student_invoice", schema: "finance");

		builder.HasKey(entity => entity.Id);

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
		builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json");
		builder.Property(entity => entity.Id).HasColumnName("student_invoice_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.AcademicYearId).HasColumnName("academic_year_id");
		builder.Property(entity => entity.InvoiceNumber).HasColumnName("invoice_number");
		builder.Property(entity => entity.InvoiceDate).HasColumnName("invoice_date");
		builder.Property(entity => entity.DueDate).HasColumnName("due_date");
		builder.Property(entity => entity.Status).HasColumnName("status");
		builder.Property(entity => entity.TotalAmount).HasColumnName("total_amount");
		builder.Property(entity => entity.BalanceAmount).HasColumnName("balance_amount");
	}
}
