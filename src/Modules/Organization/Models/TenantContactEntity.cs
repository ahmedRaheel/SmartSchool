using SmartSchool.SharedKernel;
using SmartSchool.Modules.Organization.Enums;

namespace SmartSchool.Modules.Organization.Models;

public sealed class TenantContactEntity : Entity
{
    private TenantContactEntity()
    {
    }
    public Guid TenantContactId { get; private set; } = Guid.NewGuid();
    public ContactType ContactType { get; private set; } = ContactType.Primary;
    public string? ContactName { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? AddressLine1 { get; private set; }
    public bool IsPrimary { get; private set; } = true;
    public static TenantContactEntity CreatePrimary(Guid tenantId,
        string name,
        string email,
        string phone,
        string address)
        => new () {
            TenantId=tenantId,
            ContactName=name?.Trim(),
            Email=email?.Trim(),
            Phone=phone?.Trim(),
            AddressLine1=address?.Trim()
        };

    public void UpdatePrimary(string? name, string? email, string? phone, string? address)
    {
        ContactType = ContactType.Primary;
        IsPrimary = true;
        ContactName = Clean(name);
        Email = Clean(email);
        Phone = Clean(phone);
        AddressLine1 = Clean(address);
        MarkAsUpdated();
    }

    private static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
