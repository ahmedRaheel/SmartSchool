using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="StudentProfileEntity"/>.
/// </summary>
public sealed class StudentProfileEntityConfiguration
	: IEntityTypeConfiguration<StudentProfileEntity>
{
	public void Configure(EntityTypeBuilder<StudentProfileEntity> builder)
	{
		builder.ToTable("StudentProfile", schema: "student");

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

	}
}
