using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Organization.Models;
namespace SmartSchool.Modules.Organization.Persistence.Configurations;
public sealed class TenantContactEntityConfiguration : IEntityTypeConfiguration<TenantContactEntity>
{
	public void Configure(EntityTypeBuilder<TenantContactEntity> builder)
	{
		builder.ToTable("tenant_contact", "saas");

		builder.HasKey(entity => entity.TenantContactId);

		builder.Property(entity => entity.TenantContactId)
			.HasColumnName("tenant_contact_id");

		builder.Property(entity => entity.TenantId)
			.HasColumnName("tenant_id")
			.IsRequired();

		builder.Property(entity => entity.ContactName)
			.HasColumnName("contact_name")
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(entity => entity.Email)
			.HasColumnName("email")
			.HasMaxLength(256);

		builder.Property(entity => entity.Phone)
			.HasColumnName("phone")
			.HasMaxLength(50);

		builder.Property(entity => entity.AddressLine1)
			.HasColumnName("address_line1")
			.HasMaxLength(500);
		
		builder.Property(x => x.IsPrimary).HasColumnName("is_primary");
		builder.Property(x => x.IsActive).HasColumnName("is_active");
		builder.Property(x => x.CreatedAt).HasColumnName("created_at");
		builder.Ignore(x => x.UpdatedAt);
		builder.Ignore(x => x.RowVersion);
	}
}
