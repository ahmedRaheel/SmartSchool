using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>
/// Represents the tenant aggregate root.
/// </summary>
public sealed class TenantEntity : Entity
{
	private readonly List<SchoolEntity> _schools = [];
	private readonly List<TenantContactEntity> _contactDetails = [];

	private TenantEntity()
	{
	}

	///// <summary>
	///// Gets the tenant identifier.
	///// </summary>
	//public Guid TenantId
	//{
	//	get; private set;
	//}

	/// <summary>
	/// Gets the organization name associated with the tenant.
	/// </summary>
	public string OrganizationName { get; private set; } = string.Empty;
	/// <summary>
	/// Gets the persisted status code.
	/// </summary>
	public string StatusCode { get; private set; } = string.Empty;

	/// <summary>
	/// Gets the default language.
	/// </summary>
	public string DefaultLanguage { get; private set; } = string.Empty;

	/// <summary>
	/// Gets the tenant timezone.
	/// </summary>
	public string Timezone { get; private set; } = string.Empty;

	/// <summary>
	/// Gets the tenant currency code.
	/// </summary>
	public string CurrencyCode { get; private set; } = string.Empty;

	/// <summary>
	/// Gets the tenant business code.
	/// </summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>
	/// Gets the tenant display name.
	/// </summary>
	public string FirstName { get; private set; } = string.Empty;
	public string LastName { get; private set; } = string.Empty;

	/// <summary>
	/// Gets optional domain metadata serialized as JSON.
	/// </summary>
	public string? MetadataJson
	{
		get; private set;
	}

	/// <summary>
	/// Gets the schools belonging to this tenant.
	/// </summary>
	public IReadOnlyCollection<SchoolEntity> Schools => _schools.AsReadOnly();

	/// <summary>
	/// Gets the tenant contact details.
	/// </summary>
	public IReadOnlyCollection<TenantContactEntity> ContactDetails =>
		_contactDetails.AsReadOnly();

	/// <summary>
	/// Creates a tenant.
	/// </summary>
	public static TenantEntity Create(
		Guid tenantId,
		string code,
		string organizationName,
		string firstName,
		string lastName,
		string? metadataJson = null)
	{
		if (tenantId == Guid.Empty)
		{
			throw new ArgumentException(
				"Tenant identifier cannot be empty.",
				nameof(tenantId));
		}

		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(organizationName);		

		return new TenantEntity
		{
			TenantId = tenantId,
			Code = code.Trim(),
			OrganizationName = organizationName.Trim(),
			FirstName = firstName.Trim(),
			LastName = lastName.Trim(),
			MetadataJson = metadataJson
		};
	}

	/// <summary>
	/// Adds a school to the tenant.
	/// </summary>
	public void AddSchool(SchoolEntity school)
	{
		ArgumentNullException.ThrowIfNull(school);

		if (_schools.Any(x => x.SchoolId == school.SchoolId))
		{
			throw new InvalidOperationException(
				$"School '{school.SchoolId}' already belongs to the tenant.");
		}

		_schools.Add(school);

		MarkAsUpdated();
	}

	/// <summary>
	/// Adds multiple schools to the tenant.
	/// </summary>
	public void AddSchools(IEnumerable<SchoolEntity> schools)
	{
		ArgumentNullException.ThrowIfNull(schools);

		foreach (SchoolEntity school in schools)
		{
			AddSchool(school);
		}
	}

	/// <summary>
	/// Removes a school from the tenant.
	/// </summary>
	public void RemoveSchool(Guid schoolId)
	{
		SchoolEntity? school = _schools.FirstOrDefault(
			x => x.SchoolId == schoolId);

		if (school is null)
		{
			throw new InvalidOperationException(
				$"School '{schoolId}' does not belong to the tenant.");
		}

		_schools.Remove(school);

		MarkAsUpdated();
	}

	/// <summary>
	/// Adds contact details to the tenant.
	/// </summary>
	public void AddContactDetail(TenantContactEntity contactDetail)
	{
		ArgumentNullException.ThrowIfNull(contactDetail);

		if (_contactDetails.Any(
				x => x.TenantContactId == contactDetail.TenantContactId))
		{
			throw new InvalidOperationException(
				$"Contact detail '{contactDetail.TenantContactId}' already exists.");
		}

		_contactDetails.Add(contactDetail);

		MarkAsUpdated();
	}

	/// <summary>
	/// Removes contact details from the tenant.
	/// </summary>
	public void RemoveContactDetail(Guid tenantContactId)
	{
		TenantContactEntity? contactDetail = _contactDetails.FirstOrDefault(
			x => x.TenantContactId == tenantContactId);

		if (contactDetail is null)
		{
			throw new InvalidOperationException(
				$"Contact detail '{tenantContactId}' does not exist.");
		}

		_contactDetails.Remove(contactDetail);

		MarkAsUpdated();
	}

	/// <summary>
	/// Updates the tenant business details.
	/// </summary>
	public void UpdateDetails(
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		Code = code.Trim();
		OrganizationName = name.Trim();
		MetadataJson = metadataJson;

		MarkAsUpdated();
	}
}
