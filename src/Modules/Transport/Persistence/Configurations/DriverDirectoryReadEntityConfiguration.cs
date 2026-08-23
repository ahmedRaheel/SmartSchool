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
		builder.ToTable("driverdirectoryread", schema: "public");
		builder.HasKey(readModel => readModel.Id);
		builder.Property(readModel => readModel.TenantId).IsRequired();
		builder.Property(readModel => readModel.DriverId).IsRequired();
		builder.HasIndex(readModel => new { readModel.TenantId, readModel.DriverId }).IsUnique();
		builder.Property(readModel => readModel.RowVersion).IsConcurrencyToken();

		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.DriverId).HasColumnName("driverid");
		builder.Property(entity => entity.EmployeeNumber).HasColumnName("employeenumber");
		builder.Property(entity => entity.DriverName).HasColumnName("drivername");
		builder.Property(entity => entity.MobileNumber).HasColumnName("mobilenumber");
		builder.Property(entity => entity.LicenseNumber).HasColumnName("licensenumber");
		builder.Property(entity => entity.LicenseExpiryDate).HasColumnName("licenseexpirydate");
		builder.Property(entity => entity.VehicleRegistrationNumber).HasColumnName("vehicleregistrationnumber");
		builder.Property(entity => entity.RouteName).HasColumnName("routename");
		builder.Property(entity => entity.DocumentCount).HasColumnName("documentcount");
		builder.Property(entity => entity.VerifiedDocumentCount).HasColumnName("verifieddocumentcount");
		builder.Property(entity => entity.Id).HasColumnName("id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenantid");
		builder.Property(entity => entity.IsActive).HasColumnName("isactive");
		builder.Property(entity => entity.CreatedAt).HasColumnName("createdat");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updatedat");
		builder.Property(entity => entity.RowVersion).HasColumnName("rowversion");
	}
}
