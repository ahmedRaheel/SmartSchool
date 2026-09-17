namespace SmartSchool.Modules.Reference.Models;

public sealed class CountryEntity
{
    public int CountryId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    private CountryEntity()
    {
    }
}
