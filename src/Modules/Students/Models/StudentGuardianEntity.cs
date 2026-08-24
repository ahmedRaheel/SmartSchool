using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Models;

/// <summary>
/// Represents the StudentGuardianEntity domain entity.
/// </summary>
public sealed class StudentGuardianEntity : Entity
{
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid Id
	{
		get => Id;
		private set => Id = value;
	}

	private StudentGuardianEntity()
	{
	}

	/// <summary>Gets the persisted student id value.</summary>
	public Guid StudentId { get; private set; }

	/// <summary>Gets the persisted guardian id value.</summary>
	public Guid GuardianId { get; private set; }

	/// <summary>Gets the persisted relationship value.</summary>
	public string Relationship { get; private set; } = string.Empty;

	/// <summary>Gets the persisted is primary value.</summary>
	public bool IsPrimary { get; private set; }

	/// <summary>Gets the persisted can view academics value.</summary>
	public bool CanViewAcademics { get; private set; }

	/// <summary>Gets the persisted can view finance value.</summary>
	public bool CanViewFinance { get; private set; }

	/// <summary>Gets the persisted can pickup value.</summary>
	public bool CanPickup { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new StudentGuardianEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static StudentGuardianEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new StudentGuardianEntity
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
