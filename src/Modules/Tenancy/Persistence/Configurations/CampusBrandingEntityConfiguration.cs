using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="CampusBrandingEntity"/>.
/// </summary>
public sealed class CampusBrandingEntityConfiguration
	: IEntityTypeConfiguration<CampusBrandingEntity>
{
	public void Configure(EntityTypeBuilder<CampusBrandingEntity> builder)
	{
		builder.ToTable("school_branding", schema: "saas");

		builder.HasKey(entity => entity.Id);

		builder
			.Property(entity => entity.TenantId)
			.IsRequired();

		builder
			.Property(entity => entity.IsActive)
			.IsRequired();

		builder
			.Property(entity => entity.RowVersion)
			.IsConcurrencyToken();

		builder.HasIndex(entity => entity.TenantId);

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
		builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json");
		builder.Property(entity => entity.Id).HasColumnName("id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.Logo).HasColumnName("logo");
		builder.Property(entity => entity.LogoContentType).HasColumnName("logo_content_type");
		builder.Property(entity => entity.LogoFileName).HasColumnName("logo_file_name");
		builder.Property(entity => entity.SmallLogo).HasColumnName("small_logo");
		builder.Property(entity => entity.SmallLogoContentType).HasColumnName("small_logo_content_type");
		builder.Property(entity => entity.SmallLogoFileName).HasColumnName("small_logo_file_name");
		builder.Property(entity => entity.Favicon).HasColumnName("favicon");
		builder.Property(entity => entity.FaviconContentType).HasColumnName("favicon_content_type");
		builder.Property(entity => entity.FaviconFileName).HasColumnName("favicon_file_name");
		builder.Property(entity => entity.CertificateLogo).HasColumnName("certificate_logo");
		builder.Property(entity => entity.CertificateLogoContentType).HasColumnName("certificate_logo_content_type");
		builder.Property(entity => entity.CertificateLogoFileName).HasColumnName("certificate_logo_file_name");
		builder.Property(entity => entity.Letterhead).HasColumnName("letterhead");
		builder.Property(entity => entity.LetterheadContentType).HasColumnName("letterhead_content_type");
		builder.Property(entity => entity.LetterheadFileName).HasColumnName("letterhead_file_name");
		builder.Property(entity => entity.Watermark).HasColumnName("watermark");
		builder.Property(entity => entity.WatermarkContentType).HasColumnName("watermark_content_type");
		builder.Property(entity => entity.WatermarkFileName).HasColumnName("watermark_file_name");
		builder.Property(entity => entity.PrimaryColor).HasColumnName("primary_color");
		builder.Property(entity => entity.SecondaryColor).HasColumnName("secondary_color");
		builder.Property(entity => entity.AccentColor).HasColumnName("accent_color");
		builder.Property(entity => entity.FooterText).HasColumnName("footer_text");
	}
}
