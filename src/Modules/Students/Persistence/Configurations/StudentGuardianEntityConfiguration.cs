using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="StudentGuardianEntity"/>.
/// </summary>
public sealed class StudentGuardianEntityConfiguration
	: IEntityTypeConfiguration<StudentGuardianEntity>
{
	public void Configure(EntityTypeBuilder<StudentGuardianEntity> builder)
	{
		builder.ToTable("student_guardian", schema: "student");
builder.HasKey(entity => entity.GuardianId);

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
		builder.Property(entity => entity.GuardianId).HasColumnName("id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.GuardianId).HasColumnName("guardian_id");
		builder.Property(entity => entity.Relationship).HasColumnName("relationship");
		builder.Property(entity => entity.IsPrimary).HasColumnName("is_primary");
		builder.Property(entity => entity.CanViewAcademics).HasColumnName("can_view_academics");
		builder.Property(entity => entity.CanViewFinance).HasColumnName("can_view_finance");
		builder.Property(entity => entity.CanPickup).HasColumnName("can_pickup");
	}
}
