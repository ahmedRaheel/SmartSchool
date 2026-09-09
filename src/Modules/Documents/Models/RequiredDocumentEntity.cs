using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Models;

public sealed class RequiredDocumentEntity : Entity
{
    private RequiredDocumentEntity()
    {
    }

    public Guid RequiredDocumentId { get; private set; } = Guid.NewGuid();
    public Guid? CampusId { get; private set; }
    public string UserRole { get; private set; } = string.Empty;
    public bool IsMandatory { get; private set; }
    public Guid RequiredDocumentTypeId { get; private set; }

    public static RequiredDocumentEntity Create(
        Guid tenantId,
        Guid? campusId,
        string userRole,
        bool isMandatory,
        Guid requiredDocumentTypeId)
    {
        return new RequiredDocumentEntity
        {
            TenantId = tenantId,
            CampusId = campusId,
            UserRole = userRole.Trim().ToUpperInvariant(),
            IsMandatory = isMandatory,
            RequiredDocumentTypeId = requiredDocumentTypeId
        };
    }

    public void Update(bool isMandatory)
    {
        IsMandatory = isMandatory;
        MarkAsUpdated();
    }
}
