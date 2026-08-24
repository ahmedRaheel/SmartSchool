using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Models;

/// <summary>
/// Represents the CandidateEntity domain entity.
/// </summary>
public sealed class CandidateEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid CandidateId { get; private set; } = Guid.NewGuid();

	private CandidateEntity()
	{
	}

	/// <summary>Gets the persisted first name value.</summary>
	public string FirstName { get; private set; } = string.Empty;

	/// <summary>Gets the persisted last name value.</summary>
	public string? LastName { get; private set; }

	/// <summary>Gets the persisted email value.</summary>
	public string? Email { get; private set; }

	/// <summary>Gets the persisted phone value.</summary>
	public string? Phone { get; private set; }

	/// <summary>Gets the persisted current job title value.</summary>
	public string? CurrentJobTitle { get; private set; }

	/// <summary>Gets the persisted current employer value.</summary>
	public string? CurrentEmployer { get; private set; }

	/// <summary>Gets the persisted total experience years value.</summary>
	public decimal? TotalExperienceYears { get; private set; }

	/// <summary>Gets the persisted highest qualification value.</summary>
	public string? HighestQualification { get; private set; }

	/// <summary>Gets the persisted expected salary value.</summary>
	public decimal? ExpectedSalary { get; private set; }

	/// <summary>Gets the persisted notice period days value.</summary>
	public int? NoticePeriodDays { get; private set; }

	/// <summary>Gets the persisted status code value.</summary>
	public string StatusCode { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new CandidateEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static CandidateEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new CandidateEntity
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
