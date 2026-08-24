using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Models;

/// <summary>
/// Represents the InvoiceEntity domain entity.
/// </summary>
public sealed class InvoiceEntity : Entity
{
	private InvoiceEntity()
	{
	}

	/// <summary>Gets the persisted student id value.</summary>
	public Guid StudentId { get; private set; }

	/// <summary>Gets the persisted academic year id value.</summary>
	public Guid? AcademicYearId { get; private set; }

	/// <summary>Gets the persisted invoice number value.</summary>
	public string InvoiceNumber { get; private set; } = string.Empty;

	/// <summary>Gets the persisted invoice date value.</summary>
	public DateOnly InvoiceDate { get; private set; }

	/// <summary>Gets the persisted due date value.</summary>
	public DateOnly? DueDate { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the persisted total amount value.</summary>
	public decimal TotalAmount { get; private set; }

	/// <summary>Gets the persisted balance amount value.</summary>
	public decimal BalanceAmount { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new InvoiceEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static InvoiceEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new InvoiceEntity
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
