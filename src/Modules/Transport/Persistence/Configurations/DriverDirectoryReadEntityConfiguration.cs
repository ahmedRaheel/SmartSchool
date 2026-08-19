using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence.Configurations;

/// <summary>
/// Configures the DriverDirectoryRead materialized read table.
/// </summary>
public sealed class DriverDirectoryReadEntityConfiguration
	: IEntityTypeConfiguration<DriverDirectoryReadEntity>
{
	public void Configure(EntityTypeBuilder<DriverDirectoryReadEntity> builder)
	{
		builder.ToTable("DriverDirectoryRead", schema: "transport");
		builder.HasKey(readModel => readModel.Id);
		builder.Property(readModel => readModel.TenantId).IsRequired();
		builder.Property(readModel => readModel.DriverId).IsRequired();
		builder.HasIndex(readModel => new { readModel.TenantId, readModel.DriverId }).IsUnique();
		builder.Property(readModel => readModel.RowVersion).IsConcurrencyToken();
	}
}
