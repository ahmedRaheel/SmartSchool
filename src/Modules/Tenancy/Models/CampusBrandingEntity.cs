using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Models;

/// <summary>
/// Represents the CampusBrandingEntity domain entity.
/// </summary>
public sealed class CampusBrandingEntity : Entity
{
	private CampusBrandingEntity()
	{
	}

	/// <summary>Gets the persisted logo value.</summary>
	public byte[]? Logo { get; private set; }

	/// <summary>Gets the persisted logo content type value.</summary>
	public string? LogoContentType { get; private set; }

	/// <summary>Gets the persisted logo file name value.</summary>
	public string? LogoFileName { get; private set; }

	/// <summary>Gets the persisted small logo value.</summary>
	public byte[]? SmallLogo { get; private set; }

	/// <summary>Gets the persisted small logo content type value.</summary>
	public string? SmallLogoContentType { get; private set; }

	/// <summary>Gets the persisted small logo file name value.</summary>
	public string? SmallLogoFileName { get; private set; }

	/// <summary>Gets the persisted favicon value.</summary>
	public byte[]? Favicon { get; private set; }

	/// <summary>Gets the persisted favicon content type value.</summary>
	public string? FaviconContentType { get; private set; }

	/// <summary>Gets the persisted favicon file name value.</summary>
	public string? FaviconFileName { get; private set; }

	/// <summary>Gets the persisted certificate logo value.</summary>
	public byte[]? CertificateLogo { get; private set; }

	/// <summary>Gets the persisted certificate logo content type value.</summary>
	public string? CertificateLogoContentType { get; private set; }

	/// <summary>Gets the persisted certificate logo file name value.</summary>
	public string? CertificateLogoFileName { get; private set; }

	/// <summary>Gets the persisted letterhead value.</summary>
	public byte[]? Letterhead { get; private set; }

	/// <summary>Gets the persisted letterhead content type value.</summary>
	public string? LetterheadContentType { get; private set; }

	/// <summary>Gets the persisted letterhead file name value.</summary>
	public string? LetterheadFileName { get; private set; }

	/// <summary>Gets the persisted watermark value.</summary>
	public byte[]? Watermark { get; private set; }

	/// <summary>Gets the persisted watermark content type value.</summary>
	public string? WatermarkContentType { get; private set; }

	/// <summary>Gets the persisted watermark file name value.</summary>
	public string? WatermarkFileName { get; private set; }

	/// <summary>Gets the persisted primary color value.</summary>
	public string? PrimaryColor { get; private set; }

	/// <summary>Gets the persisted secondary color value.</summary>
	public string? SecondaryColor { get; private set; }

	/// <summary>Gets the persisted accent color value.</summary>
	public string? AccentColor { get; private set; }

	/// <summary>Gets the persisted footer text value.</summary>
	public string? FooterText { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new CampusBrandingEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static CampusBrandingEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new CampusBrandingEntity
		{
			TenantId = tenantId,
			Code = code.Trim(),
			Name = name.Trim(),
			MetadataJson = metadataJson
		};
	}

	/// <summary>Updates the business details.</summary>
	/// <param name="code">The new business code.</param>
	/// <param name="name">The new display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	public void UpdateDetails(
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		Code = code.Trim();
		Name = name.Trim();
		MetadataJson = metadataJson;
		MarkAsUpdated();
	}
}
