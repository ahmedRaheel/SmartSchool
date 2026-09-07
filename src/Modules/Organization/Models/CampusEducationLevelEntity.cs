using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

public sealed class CampusEducationLevelEntity : Entity
{
    private CampusEducationLevelEntity() { }

    public Guid CampusId { get; private set; }
    public Guid EducationLevelId { get; private set; }

    public static CampusEducationLevelEntity Create(Guid tenantId, Guid campusId, Guid educationLevelId) => new()
    {
        TenantId = tenantId,
        CampusId = campusId,
        EducationLevelId = educationLevelId
    };
}
