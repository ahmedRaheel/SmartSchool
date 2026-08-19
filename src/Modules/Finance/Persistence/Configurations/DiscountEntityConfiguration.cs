using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="DiscountEntity"/>.
/// </summary>
public sealed class DiscountEntityConfiguration
	: IEntityTypeConfiguration<DiscountEntity>
{
	public void Configure(EntityTypeBuilder<DiscountEntity> builder)
	{
		builder.ToTable("Discount", schema: "finance");

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

	}
}
