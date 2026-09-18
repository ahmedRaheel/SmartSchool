namespace SmartSchool.Modules.Reference.Models;

public sealed class BranchGenderTypeEntity
{
    public Guid BranchGenderTypeId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }

    private BranchGenderTypeEntity()
    {
    }
}
