using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>Represents a physical branch/campus belonging to a school.</summary>
public sealed class CampusEntity : Entity
{
    private CampusEntity() { }

    public Guid CampusId { get; private set; } = Guid.NewGuid();
    public Guid SchoolId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string BranchType { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? City { get; private set; }
    public string? Province { get; private set; }
    public string? Phone { get; private set; }
    public string? Fax { get; private set; }
    public string? Mobile { get; private set; }
    public string? Email { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? MetadataJson => null;

    public static CampusEntity Create(
        Guid tenantId,
        Guid schoolId,
        string code,
        string name,
        string branchType,
        string? address,
        string? city,
        string? province,
        string? phone,
        string? fax,
        string? mobile,
        string? email,
        string? logoUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(branchType);
        if (schoolId == Guid.Empty) throw new ArgumentException("School is required.", nameof(schoolId));

        return new CampusEntity
        {
            TenantId = tenantId,
            SchoolId = schoolId,
            Code = code.Trim(),
            Name = name.Trim(),
            BranchType = branchType.Trim(),
            Address = Clean(address),
            City = Clean(city),
            Province = Clean(province),
            Phone = Clean(phone),
            Fax = Clean(fax),
            Mobile = Clean(mobile),
            Email = Clean(email),
            LogoUrl = Clean(logoUrl)
        };
    }

    public void UpdateDetails(
        Guid schoolId,
        string code,
        string name,
        string branchType,
        string? address,
        string? city,
        string? province,
        string? phone,
        string? fax,
        string? mobile,
        string? email,
        string? logoUrl)
    {
        if (schoolId == Guid.Empty)
        {
            throw new ArgumentException("School is required.", nameof(schoolId));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(branchType);

        SchoolId = schoolId;
        Code = code.Trim();
        Name = name.Trim();
        BranchType = branchType.Trim();
        Address = Clean(address);
        City = Clean(city);
        Province = Clean(province);
        Phone = Clean(phone);
        Fax = Clean(fax);
        Mobile = Clean(mobile);
        Email = Clean(email);
        LogoUrl = Clean(logoUrl);
        MarkAsUpdated();
    }

    public void UpdateDetails(string code, string name, string? metadataJson = null)
    {
        UpdateDetails(SchoolId, code, name, BranchType, Address, City, Province, Phone, Fax, Mobile, Email, LogoUrl);
    }

    private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
