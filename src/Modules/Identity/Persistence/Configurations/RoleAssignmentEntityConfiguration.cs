using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="RoleAssignmentEntity"/>.
/// </summary>
public sealed class RoleAssignmentEntityConfiguration
	: IEntityTypeConfiguration<RoleAssignmentEntity>
{
	public void Configure(EntityTypeBuilder<RoleAssignmentEntity> builder)
	{
		builder.ToTable("RoleAssignment");

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

	}
}
