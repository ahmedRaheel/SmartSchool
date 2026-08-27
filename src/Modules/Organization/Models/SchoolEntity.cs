using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>Represents a school owned by a SaaS tenant.</summary>
public sealed class SchoolEntity : Entity
{
    private SchoolEntity() { }

    public Guid SchoolId { get; private set; } = Guid.NewGuid();
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? RegistrationNumber { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Fax { get; private set; }
    public string? Website { get; private set; }
    public string? Address { get; private set; }
    public string? City { get; private set; }
    public string? Province { get; private set; }
    public string? Country { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? MetadataJson => null;

    public static SchoolEntity Create(
        Guid tenantId,
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
        string? logoUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new SchoolEntity
        {
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
            LogoUrl = Clean(logoUrl)
        };
    }

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
        string? logoUrl)
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
        MarkAsUpdated();
    }

    public void UpdateDetails(string code, string name)
    {
        UpdateDetails(code, name, RegistrationNumber, Email, Phone, Fax, Website, Address, City, Province, Country, LogoUrl);
    }

    private static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
