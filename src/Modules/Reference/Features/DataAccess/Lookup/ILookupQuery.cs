namespace SmartSchool.Modules.Reference.Features.DataAccess.Lookup;

public interface ILookupQuery
{
    Task<IReadOnlyList<LookupTypeResponse>> GetTypesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<LookupValueResponse>> GetValuesAsync(string typeCode, CancellationToken cancellationToken);
    Task<IReadOnlyList<LookupGroupResponse>> GetAllAsync(CancellationToken cancellationToken);
}

public sealed record LookupTypeResponse(long Id, string Code, string Name);
public sealed record LookupValueResponse(long Id, string TypeCode, string Code, string Name, int SortOrder);
public sealed record LookupGroupResponse(string Code, string Name, IReadOnlyList<LookupValueResponse> Values);
