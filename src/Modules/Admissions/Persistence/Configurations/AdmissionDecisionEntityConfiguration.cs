using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="AdmissionDecisionEntity"/>.
/// </summary>
public sealed class AdmissionDecisionEntityConfiguration
	: IEntityTypeConfiguration<AdmissionDecisionEntity>
{
	public void Configure(EntityTypeBuilder<AdmissionDecisionEntity> builder)
	{
		builder.ToTable("AdmissionDecision");

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
