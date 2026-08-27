using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence.Configurations;

public sealed class CampusEntityConfiguration : IEntityTypeConfiguration<CampusEntity>
{
    public void Configure(EntityTypeBuilder<CampusEntity> builder)
    {
        builder.ToTable("campus", "org");
        builder.Ignore(x => x.MetadataJson);
        builder.HasKey(x => x.CampusId);
        builder.Property(x => x.CampusId).HasColumnName("campus_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(x => x.SchoolId).HasColumnName("school_id").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.BranchType).HasColumnName("branch_type").HasMaxLength(40).IsRequired();
        builder.Property(x => x.BranchGenderTypeId).HasColumnName("branch_gender_type_id").IsRequired();
        builder.Property(x => x.Address).HasColumnName("address");
        builder.Property(x => x.City).HasColumnName("city").HasMaxLength(120);
        builder.Property(x => x.Province).HasColumnName("province").HasMaxLength(120);
        builder.Property(x => x.Country).HasColumnName("country").HasMaxLength(120);
        builder.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(50);
        builder.Property(x => x.Fax).HasColumnName("fax").HasMaxLength(50);
        builder.Property(x => x.Mobile).HasColumnName("mobile").HasMaxLength(50);
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(200);
        builder.Property(x => x.LogoUrl).HasColumnName("logo_url").HasMaxLength(500);
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();
        builder.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.SchoolId });
    }
}
