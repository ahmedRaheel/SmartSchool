using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence.Configurations;

/// <summary>Defines persistence rules for <see cref="GuardianEntity"/>.</summary>
public sealed class GuardianEntityConfiguration : IEntityTypeConfiguration<GuardianEntity>
{
	public void Configure(EntityTypeBuilder<GuardianEntity> builder)
	{
		builder.ToTable("guardian", schema: "student");
		builder.Ignore(entity => entity.Id);
		builder.HasKey(entity => entity.GuardianId);

		builder.Property(entity => entity.GuardianId).HasColumnName("guardian_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id").IsRequired();
		builder.Property(entity => entity.UserId).HasColumnName("user_id");
		builder.Property(entity => entity.FullName).HasColumnName("full_name").HasMaxLength(200).IsRequired();
		builder.Property(entity => entity.CnicNumber).HasColumnName("cnic_number").HasMaxLength(20);
		builder.Property(entity => entity.Email).HasColumnName("email").HasMaxLength(250);
		builder.Property(entity => entity.Phone).HasColumnName("phone").HasMaxLength(50);

		builder.HasIndex(entity => new { entity.TenantId, entity.CnicNumber }).IsUnique();

		builder.Property(entity => entity.IsActive).HasColumnName("is_active").IsRequired();
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").IsRequired();
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();

		// Canonical database mapping generated from SmartSchoolComplete.sql.
	}
}
