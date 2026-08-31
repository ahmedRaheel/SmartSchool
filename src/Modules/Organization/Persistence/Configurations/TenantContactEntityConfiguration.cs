using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Organization.Models;
namespace SmartSchool.Modules.Organization.Persistence.Configurations;
public sealed class TenantContactEntityConfiguration : IEntityTypeConfiguration<TenantContactEntity>
{
	public void Configure(EntityTypeBuilder<TenantContactEntity> builder)
	{
		builder.ToTable("tenant_contact", "org");
		builder.HasKey(x => x.TenantContactId);
		builder.Property(x => x.TenantContactId).HasColumnName("tenant_contact_id");
		builder.Property(x => x.TenantId).HasColumnName("tenant_id");
		builder.Property(x => x.ContactType).HasColumnName("contact_type");
		builder.Property(x => x.ContactName).HasColumnName("contact_name");
		builder.Property(x => x.Email).HasColumnName("email");
		builder.Property(x => x.Phone).HasColumnName("phone");
		builder.Property(x => x.AddressLine1).HasColumnName("address_line1");
		builder.Property(x => x.IsPrimary).HasColumnName("is_primary");
		builder.Property(x => x.IsActive).HasColumnName("is_active");
		builder.Property(x => x.CreatedAt).HasColumnName("created_at");
		builder.Ignore(x => x.UpdatedAt);
		builder.Ignore(x => x.RowVersion);
	}
}
