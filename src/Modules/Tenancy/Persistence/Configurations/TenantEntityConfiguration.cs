using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="TenantEntity"/>.
/// </summary>
public sealed class TenantEntityConfiguration
	: IEntityTypeConfiguration<TenantEntity>
{
	public void Configure(EntityTypeBuilder<TenantEntity> builder)
	{
		builder.ToTable("Tenant", SmartSchool.Modules.Tenancy.ModuleConstants.Schema);

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
