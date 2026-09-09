using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Models;

public sealed class DocumentTypeEntity : Entity
{
    private DocumentTypeEntity()
    {
    }

    public Guid DocumentTypeId { get; private set; } = Guid.NewGuid();
    public Guid? CampusId { get; private set; }
    public DocumentOwnerType OwnerType { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public static DocumentTypeEntity Create(
        Guid tenantId,
        Guid? campusId,
        DocumentOwnerType ownerType,
        string code,
        string name,
        string? description)
    {
        return new DocumentTypeEntity
        {
            TenantId = tenantId,
            CampusId = campusId,
            OwnerType = ownerType,
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
