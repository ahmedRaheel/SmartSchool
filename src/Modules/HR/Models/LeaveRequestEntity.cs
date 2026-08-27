using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Models;

/// <summary>
/// Represents the LeaveRequestEntity domain entity.
/// </summary>
public sealed class LeaveRequestEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid LeaveRequestId { get; private set; } = Guid.NewGuid();

	private LeaveRequestEntity()
	{
	}

	/// <summary>Gets the persisted employee id value.</summary>
	public Guid EmployeeId { get; private set; }

	/// <summary>Gets the persisted leave type value.</summary>
	public string LeaveType { get; private set; } = string.Empty;

	/// <summary>Gets the persisted from date value.</summary>
	public DateOnly FromDate { get; private set; }

	/// <summary>Gets the persisted to date value.</summary>
	public DateOnly ToDate { get; private set; }

	/// <summary>Gets the persisted reason value.</summary>
	public string Reason { get; private set; } = string.Empty;

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the persisted approved by value.</summary>
	public Guid? ApprovedBy { get; private set; }

	/// <summary>Gets the persisted decision at value.</summary>
	public DateTimeOffset? DecisionAt { get; private set; }

	/// <summary>Gets the persisted decision note value.</summary>
	public string? DecisionNote { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new LeaveRequestEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static LeaveRequestEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new LeaveRequestEntity
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
