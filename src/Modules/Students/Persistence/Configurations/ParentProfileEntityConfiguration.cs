using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="ParentProfileEntity"/>.
/// </summary>
public sealed class ParentProfileEntityConfiguration
	: IEntityTypeConfiguration<ParentProfileEntity>
{
	public void Configure(EntityTypeBuilder<ParentProfileEntity> builder)
	{
		builder.ToTable("ParentProfile");

		builder.HasKey(entity => entity.Id);

		builder
			.Property(entity => entity.TenantId)
			.IsRequired();

		builder
			.Property(entity => entity.IsActive)
			.IsRequired();

		builder
			.Property(entity => entity.RowVersion)
			.IsConcurrencyToken();

		builder.HasIndex(entity => entity.TenantId);

	}
}
