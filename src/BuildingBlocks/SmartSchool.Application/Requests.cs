namespace SmartSchool.Application.Requests;

public sealed record PageRequest(int Page = 1, int PageSize = 25)
{
    public const int MaximumPageSize = 200;

    public int NormalizedPage =>
        Math.Max(1, Page);

    public int NormalizedPageSize =>
        Math.Clamp(PageSize, 1, MaximumPageSize);
}
