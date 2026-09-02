using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Models;

public sealed class FeeTypeEntity : Entity
{
    public Guid FeeTypeId { get; private set; } = Guid.NewGuid();
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Frequency { get; private set; } = "Monthly";
    public string? Description { get; private set; }
    public string? MetadataJson { get; private set; }

    private FeeTypeEntity() { }

    public static FeeTypeEntity Create(Guid tenantId, string code, string name, string frequency, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(frequency);
        return new FeeTypeEntity { TenantId=tenantId, Code=code.Trim(), Name=name.Trim(), Frequency=frequency.Trim(), Description=description?.Trim() };
    }

    public void UpdateDetails(string name, string frequency, bool isActive, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(frequency);
        Name=name.Trim(); Frequency=frequency.Trim(); Description=description?.Trim();
        if (isActive) Activate(); else Deactivate();
        MarkAsUpdated();
    }
}
