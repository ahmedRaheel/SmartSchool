namespace SmartSchool.Modules.Reference.Models;

public sealed class CityEntity
{
    public int CityId { get; private set; }
    public int ProvinceId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    private CityEntity()
    {
    }
}
