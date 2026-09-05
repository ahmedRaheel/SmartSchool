using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Reference.Models;

public sealed class LookupValueEntity : Entity
{
    public long LookupValueId { get; private set; }
    public long LookupTypeId { get; private set; }
    public Guid? LookupTenantId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public int SortOrder { get; private set; }
    public string? Metadata { get; private set; }

    private LookupValueEntity() { }

    public static LookupValueEntity Create(long lookupTypeId, Guid? tenantId, string code, string name, int sortOrder, string? metadata)
    {
        return new LookupValueEntity { LookupTypeId = lookupTypeId, LookupTenantId = tenantId, TenantId = tenantId ?? Guid.Empty, Code = code.Trim().ToUpperInvariant(), Name = name.Trim(), SortOrder = sortOrder, Metadata = metadata };
    }

    public void Update(string code, string name, int sortOrder, bool isActive, string? metadata)
    {
        Code = code.Trim().ToUpperInvariant(); Name = name.Trim(); SortOrder = sortOrder; Metadata = metadata;
        if (isActive) Activate(); else Deactivate();
        MarkAsUpdated();
    }
}
