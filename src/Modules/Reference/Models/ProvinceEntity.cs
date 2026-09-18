namespace SmartSchool.Modules.Reference.Models;

public sealed class ProvinceEntity
{
    public int ProvinceId { get; private set; }
    public int CountryId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    private ProvinceEntity()
    {
    }
}
