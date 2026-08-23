using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="StudentPerformancePredictionEntity"/>.
/// </summary>
public sealed class StudentPerformancePredictionEntityConfiguration
	: IEntityTypeConfiguration<StudentPerformancePredictionEntity>
{
	public void Configure(EntityTypeBuilder<StudentPerformancePredictionEntity> builder)
	{
		builder.ToTable("StudentPerformancePrediction", SmartSchool.Modules.AIPrediction.ModuleConstants.Schema);

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
