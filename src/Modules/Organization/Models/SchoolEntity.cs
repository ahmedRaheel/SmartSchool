using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>
/// Represents a school owned by a tenant.
/// </summary>
public sealed class SchoolEntity : Entity
{
    private readonly List<CampusEntity> _campuses = [];

    private SchoolEntity()
    {
    }

    /// <summary>
    /// Gets the school identifier.
    /// </summary>
    public Guid SchoolId
    {
        get; private set;
    }

    /// <summary>
    /// Gets the business code.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the school name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    public string? RegistrationNumber
    {
        get; private set;
    }

    public string? Email
    {
        get; private set;
    }

    public string? Phone
    {
        get; private set;
    }

    public string? Fax
    {
        get; private set;
    }

    public string? Website
    {
        get; private set;
    }

    public string? Address
    {
        get; private set;
    }

    public string? City
    {
        get; private set;
    }

    public string? Province
    {
        get; private set;
    }

    public string? Country
    {
        get; private set;
    }

    public string? LogoUrl
    {
        get; private set;
    }

    public string? MetadataJson
    {
        get; private set;
    }

    /// <summary>
    /// Gets the campuses belonging to this school.
    /// </summary>
    public IReadOnlyCollection<CampusEntity> Campuses =>
        _campuses.AsReadOnly();

    /// <summary>
    /// Creates a school.
    /// </summary>
    public static SchoolEntity Create(
        Guid tenantId,
        Guid schoolId,
        string code,
        string name,
        string? registrationNumber = null,
        string? email = null,
        string? phone = null,
        string? fax = null,
        string? website = null,
        string? address = null,
        string? city = null,
        string? province = null,
        string? country = null,
        string? logoUrl = null,
        string? metadataJson = null)
    {
        if (schoolId == Guid.Empty)
        {
            throw new ArgumentException(
                "School identifier cannot be empty.",
                nameof(schoolId));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new SchoolEntity
        {
            SchoolId = schoolId,
            TenantId = tenantId,
            Code = code.Trim(),
            Name = name.Trim(),
            RegistrationNumber = Clean(registrationNumber),
            Email = Clean(email),
            Phone = Clean(phone),
            Fax = Clean(fax),
            Website = Clean(website),
            Address = Clean(address),
            City = Clean(city),
            Province = Clean(province),
            Country = Clean(country),
            LogoUrl = Clean(logoUrl),
            MetadataJson = Clean(metadataJson)
        };
    }

    /// <summary>
    /// Adds a campus to this school.
    /// </summary>
    public void AddCampus(CampusEntity campus)
    {
        ArgumentNullException.ThrowIfNull(campus);

        if (_campuses.Any(x => x.CampusId == campus.CampusId))
        {
            throw new InvalidOperationException(
                $"Campus '{campus.CampusId}' already belongs to the school.");
        }

        if (_campuses.Any(
                x => string.Equals(
                    x.Code,
                    campus.Code,
                    StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                $"Campus code '{campus.Code}' already exists in the school.");
        }

        _campuses.Add(campus);

        MarkAsUpdated();
    }

    /// <summary>
    /// Adds multiple campuses to this school.
    /// </summary>
    public void AddCampuses(IEnumerable<CampusEntity> campuses)
    {
        ArgumentNullException.ThrowIfNull(campuses);

        foreach (CampusEntity campus in campuses)
        {
            AddCampus(campus);
        }
    }

    /// <summary>
    /// Removes a campus from this school.
    /// </summary>
    public void RemoveCampus(Guid campusId)
    {
        CampusEntity? campus = _campuses.FirstOrDefault(
            x => x.CampusId == campusId);

        if (campus is null)
        {
            throw new InvalidOperationException(
                $"Campus '{campusId}' does not belong to the school.");
        }

        _campuses.Remove(campus);

        MarkAsUpdated();
    }

    /// <summary>
    /// Updates the school details.
    /// </summary>
    public void UpdateDetails(
        string code,
        string name,
        string? registrationNumber,
        string? email,
        string? phone,
        string? fax,
        string? website,
        string? address,
        string? city,
        string? province,
        string? country,
        string? logoUrl,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Code = code.Trim();
        Name = name.Trim();
        RegistrationNumber = Clean(registrationNumber);
        Email = Clean(email);
        Phone = Clean(phone);
        Fax = Clean(fax);
        Website = Clean(website);
        Address = Clean(address);
        City = Clean(city);
        Province = Clean(province);
        Country = Clean(country);
        LogoUrl = Clean(logoUrl);
        MetadataJson = Clean(metadataJson);

        MarkAsUpdated();
    }

    /// <summary>
    /// Updates the basic school details.
    /// </summary>
    public void UpdateDetails(
        string code,
        string name)
    {
        UpdateDetails(
            code,
            name,
            RegistrationNumber,
            Email,
            Phone,
            Fax,
            Website,
            Address,
            City,
            Province,
            Country,
            LogoUrl,
            MetadataJson);
    }

    private static string? Clean(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}
