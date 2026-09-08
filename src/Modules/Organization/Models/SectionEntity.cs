using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>Represents a reusable section label created with a class section.</summary>
public sealed class SectionEntity : Entity
{
    public Guid SectionId { get; private set; } = Guid.NewGuid();
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    private SectionEntity()
    {
    }

    public static SectionEntity Create(Guid tenantId, string code, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new SectionEntity
        {
            TenantId = tenantId,
            Code = code.Trim(),
            Name = name.Trim()
        };
    }
}
