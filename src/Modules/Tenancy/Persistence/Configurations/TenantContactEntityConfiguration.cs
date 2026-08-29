using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Tenancy.Models;
namespace SmartSchool.Modules.Tenancy.Persistence.Configurations;
public sealed class TenantContactEntityConfiguration : IEntityTypeConfiguration<TenantContactEntity>
{
    public void Configure(EntityTypeBuilder<TenantContactEntity> b)
    {
        b.ToTable("tenant_contact","saas"); b.HasKey(x=>x.TenantContactId); b.Property(x=>x.TenantContactId).HasColumnName("tenant_contact_id"); b.Property(x=>x.TenantId).HasColumnName("tenant_id"); b.Property(x=>x.ContactType).HasColumnName("contact_type"); b.Property(x=>x.ContactName).HasColumnName("contact_name"); b.Property(x=>x.Email).HasColumnName("email"); b.Property(x=>x.Phone).HasColumnName("phone"); b.Property(x=>x.AddressLine1).HasColumnName("address_line1"); b.Property(x=>x.IsPrimary).HasColumnName("is_primary"); b.Property(x=>x.IsActive).HasColumnName("is_active"); b.Property(x=>x.CreatedAt).HasColumnName("created_at"); b.Ignore(x=>x.UpdatedAt); b.Ignore(x=>x.RowVersion);
    }
}
