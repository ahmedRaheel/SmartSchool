using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="DriverEntity"/>.
/// </summary>
public sealed class DriverEntityConfiguration
    : IEntityTypeConfiguration<DriverEntity>
{
    public void Configure(EntityTypeBuilder<DriverEntity> builder)
    {
        builder.ToTable("driver", schema: "transport");
        builder.HasKey(entity => entity.DriverId);

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


        // Canonical database mapping generated from SmartSchoolComplete.sql.
        builder.Property(entity => entity.EmployeeNumber).HasColumnName("employee_number");
        builder.Property(entity => entity.FirstName).HasColumnName("first_name");
        builder.Property(entity => entity.LastName).HasColumnName("last_name");
        builder.Property(entity => entity.Cnic).HasColumnName("cnic");
        builder.Property(entity => entity.DateOfBirth).HasColumnName("date_of_birth");
        builder.Property(entity => entity.MobileNumber).HasColumnName("mobile_number");
        builder.Property(entity => entity.DrivingLicenseNumber).HasColumnName("driving_license_number");
        builder.Property(entity => entity.DrivingLicenseCategory).HasColumnName("driving_license_category");
        builder.Property(entity => entity.LicenseExpiryDate).HasColumnName("license_expiry_date");
        builder.Property(entity => entity.JoiningDate).HasColumnName("joining_date");
        builder.Property(entity => entity.EmploymentStatusCode).HasColumnName("employment_status_code");
        builder.Property(entity => entity.EmergencyContactName).HasColumnName("emergency_contact_name");
        builder.Property(entity => entity.EmergencyContactPhone).HasColumnName("emergency_contact_phone");
        builder.Property(entity => entity.AssignedVehicleId).HasColumnName("assigned_vehicle_id");
        builder.Property(entity => entity.DriverId).HasColumnName("driver_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

        // Database columns synchronized from SmartSchoolComplete.sql.
        builder.Property(entity => entity.EmployeeId).HasColumnName("employee_id");
        builder.Property(entity => entity.DriverNumber).HasColumnName("driver_number");
        builder.Property(entity => entity.FullName).HasColumnName("full_name");
        builder.Property(entity => entity.CnicNumber).HasColumnName("cnic_number");
        builder.Property(entity => entity.Phone).HasColumnName("phone");
        builder.Property(entity => entity.AlternatePhone).HasColumnName("alternate_phone");
        builder.Property(entity => entity.DrivingLicenseIssuedOn).HasColumnName("driving_license_issued_on");
        builder.Property(entity => entity.DrivingLicenseExpiresOn).HasColumnName("driving_license_expires_on");
        builder.Property(entity => entity.Picture).HasColumnName("picture");
        builder.Property(entity => entity.PictureContentType).HasColumnName("picture_content_type");
        builder.Property(entity => entity.PictureFileName).HasColumnName("picture_file_name");
        builder.Property(entity => entity.Address).HasColumnName("address");
        builder.Property(entity => entity.HireDate).HasColumnName("hire_date");
        builder.Property(entity => entity.Status).HasColumnName("status");
    }
}
