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
        builder.ToTable("Reservation", schema: "library");
        builder.HasKey(entity => entity.ReservationId);

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


        // Explicit PostgreSQL mappings for synchronized table.
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version");
        builder.Property(entity => entity.Code).HasColumnName("code");
        builder.Property(entity => entity.Name).HasColumnName("name");
        builder.Property(entity => entity.ReservationId).HasColumnName("reservation_id");
    }
}
