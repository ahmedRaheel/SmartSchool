using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="DepartmentEntity"/>.
/// </summary>
public sealed class DepartmentEntityConfiguration
	: IEntityTypeConfiguration<DepartmentEntity>
{
	public void Configure(EntityTypeBuilder<DepartmentEntity> builder)
	{
		builder.ToTable("department", schema: "org");
		builder.HasKey(entity => entity.DepartmentId);

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
		builder.Property(entity => entity.Telephone).HasColumnName("telephone").HasMaxLength(50);
		builder.Property(entity => entity.Email).HasColumnName("email").HasMaxLength(250);
		builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
		builder.Property(entity => entity.DepartmentId).HasColumnName("department_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.CampusId).HasColumnName("campus_id").IsRequired();
		builder.Property(entity => entity.HeadOfDepartmentEmployeeId).HasColumnName("head_of_department_employee_id");

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
}
}
