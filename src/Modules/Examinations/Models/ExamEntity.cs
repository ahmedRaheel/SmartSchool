using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Models;

/// <summary>
/// Represents the ExamEntity domain entity.
/// </summary>
public sealed class ExamEntity : Entity
{
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid ExamId
	{
		get => Id;
		private set => Id = value;
	}

	private ExamEntity()
	{
	}

	/// <summary>Gets the persisted campus id value.</summary>
	public Guid CampusId { get; private set; }

	/// <summary>Gets the persisted academic year id value.</summary>
	public Guid AcademicYearId { get; private set; }

	/// <summary>Gets the persisted term id value.</summary>
	public Guid? TermId { get; private set; }

	/// <summary>Gets the persisted academic system id value.</summary>
	public Guid AcademicSystemId { get; private set; }

	/// <summary>Gets the persisted exam type code value.</summary>
	public string ExamTypeCode { get; private set; } = string.Empty;

	/// <summary>Gets the persisted start date value.</summary>
	public DateOnly? StartDate { get; private set; }

	/// <summary>Gets the persisted end date value.</summary>
	public DateOnly? EndDate { get; private set; }

	/// <summary>Gets the persisted result publish date value.</summary>
	public DateOnly? ResultPublishDate { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new ExamEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static ExamEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new ExamEntity
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
