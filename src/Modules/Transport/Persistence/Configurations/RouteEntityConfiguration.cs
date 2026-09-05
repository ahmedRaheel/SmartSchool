using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="RouteEntity"/>.
/// </summary>
public sealed class RouteEntityConfiguration
    : IEntityTypeConfiguration<RouteEntity>
{
    public void Configure(EntityTypeBuilder<RouteEntity> builder)
    {
        builder.ToTable("route", schema: "transport");
        builder.HasKey(entity => entity.RouteId);

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


        // Canonical database mapping generated from SmartSchoolComplete.sql.
        builder.Property(entity => entity.Code).HasColumnName("code");
        builder.Property(entity => entity.Name).HasColumnName("name");
        builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
        builder.Property(entity => entity.RouteId).HasColumnName("route_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

        // Database columns synchronized from SmartSchoolComplete.sql.
        builder.Property(entity => entity.CampusId).HasColumnName("campus_id");
    }
}
