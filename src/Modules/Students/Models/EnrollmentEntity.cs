using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Models;

/// <summary>
/// Represents the EnrollmentEntity domain entity.
/// </summary>
public sealed class EnrollmentEntity : Entity
{
<<<<<<< HEAD
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid EnrollmentId { get; private set; } = Guid.NewGuid();
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid StudentEnrollmentId { get; private set; } = Guid.NewGuid();
=======
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid StudentEnrollmentId
	{
		get => Id;
		private set => Id = value;
	}
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

	private EnrollmentEntity()
	{
	}

	/// <summary>Gets the persisted student id value.</summary>
	public Guid StudentId { get; private set; }

	/// <summary>Gets the persisted academic year id value.</summary>
	public Guid AcademicYearId { get; private set; }

	/// <summary>Gets the persisted class section id value.</summary>
	public Guid ClassSectionId { get; private set; }

	/// <summary>Gets the persisted enrollment date value.</summary>
	public DateOnly EnrollmentDate { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new EnrollmentEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static EnrollmentEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new EnrollmentEntity
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
