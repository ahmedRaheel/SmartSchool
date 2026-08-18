using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="ReservationEntity"/>.
/// </summary>
public sealed class ReservationEntityConfiguration
	: IEntityTypeConfiguration<ReservationEntity>
{
	public void Configure(EntityTypeBuilder<ReservationEntity> builder)
	{
		builder.ToTable("Reservation");

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
