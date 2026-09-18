using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="RoomEntity"/>.
/// </summary>
public sealed class RoomEntityConfiguration : IEntityTypeConfiguration<RoomEntity>
{
    public void Configure(EntityTypeBuilder<RoomEntity> builder)
    {
        builder.ToTable("room", "org");
        builder.HasKey(entity => entity.RoomId);

        builder.Property(entity => entity.RoomId).HasColumnName("room_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(entity => entity.CampusId).HasColumnName("campus_id").IsRequired();
        builder.Property(entity => entity.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.Name).HasColumnName("name").HasMaxLength(120).IsRequired();
        builder.Property(entity => entity.Capacity).HasColumnName("capacity");
        builder.Property(entity => entity.RoomType).HasColumnName("room_type").HasMaxLength(40);
        builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();

        builder.HasIndex(entity => entity.TenantId);
        builder.HasIndex(entity => new { entity.CampusId, entity.Code }).IsUnique();

        builder.HasOne<CampusEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.CampusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
