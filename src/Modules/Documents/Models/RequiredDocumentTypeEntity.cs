using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Models;

public sealed class RequiredDocumentTypeEntity : Entity
{
    private RequiredDocumentTypeEntity()
    {
    }

    public Guid RequiredDocumentTypeId { get; private set; } = Guid.NewGuid();
    public Guid? CampusId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public static RequiredDocumentTypeEntity Create(
        Guid tenantId,
        Guid? campusId,
        string code,
        string name,
        string? description)
    {
        return new RequiredDocumentTypeEntity
        {
            TenantId = tenantId,
            CampusId = campusId,
            Code = code.Trim().ToUpperInvariant(),
            Name = name.Trim(),
            Description = description?.Trim()
        };
    }

    public void Update(string name, string? description)
    {
        Name = name.Trim();
        Description = description?.Trim();
        MarkAsUpdated();
    }
}
