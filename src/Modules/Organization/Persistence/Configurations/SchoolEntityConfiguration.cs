using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence.Configurations;

public sealed class SchoolEntityConfiguration : IEntityTypeConfiguration<SchoolEntity>
{
    public void Configure(EntityTypeBuilder<SchoolEntity> builder)
    {
        builder.ToTable("school", "org");
        builder.Ignore(x => x.MetadataJson);
        builder.HasKey(x => x.SchoolId);
        builder.Property(x => x.SchoolId).HasColumnName("school_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.RegistrationNumber).HasColumnName("registration_number").HasMaxLength(100);
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(200);
        builder.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(50);
        builder.Property(x => x.Fax).HasColumnName("fax").HasMaxLength(50);
        builder.Property(x => x.Website).HasColumnName("website").HasMaxLength(300);
        builder.Property(x => x.Address).HasColumnName("address");
        builder.Property(x => x.City).HasColumnName("city").HasMaxLength(120);
        builder.Property(x => x.Province).HasColumnName("province").HasMaxLength(120);
        builder.Property(x => x.Country).HasColumnName("country").HasMaxLength(120);
        builder.Property(x => x.LogoUrl).HasColumnName("logo_url").HasMaxLength(500);
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();
        builder.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        builder.HasMany(x => x.Campuses)
        .WithOne()
        .HasForeignKey(campus => campus.SchoolId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
