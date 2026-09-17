namespace SmartSchool.Modules.Reference.Models;

public sealed class LookupTypeEntity
{
    public long LookupTypeId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public bool IsTenantScoped { get; private set; }

    private LookupTypeEntity()
    {
    }
}
