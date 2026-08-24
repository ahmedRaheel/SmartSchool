using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Models;

/// <summary>
/// Represents the JobEntity domain entity.
/// </summary>
public sealed class JobEntity : Entity
{
	private JobEntity()
	{
	}

	/// <summary>Gets the persisted department id value.</summary>
	public Guid? DepartmentId { get; private set; }

	/// <summary>Gets the persisted job family id value.</summary>
	public Guid? JobFamilyId { get; private set; }

	/// <summary>Gets the persisted title value.</summary>
	public string Title { get; private set; } = string.Empty;

	/// <summary>Gets the persisted description value.</summary>
	public string? Description { get; private set; }

	/// <summary>Gets the persisted responsibilities value.</summary>
	public string? Responsibilities { get; private set; }

	/// <summary>Gets the persisted minimum qualification value.</summary>
	public string? MinimumQualification { get; private set; }

	/// <summary>Gets the persisted minimum experience years value.</summary>
	public decimal? MinimumExperienceYears { get; private set; }

	/// <summary>Gets the persisted is teaching position value.</summary>
	public bool IsTeachingPosition { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new JobEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static JobEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new JobEntity
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
