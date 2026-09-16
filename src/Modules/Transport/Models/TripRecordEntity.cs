using SmartSchool.SharedKernel;
namespace SmartSchool.Modules.Transport.Models;
public sealed class TripRecordEntity : Entity
{
    private TripRecordEntity() { }
    public Guid TripRecordId { get; private set; } = Guid.NewGuid();
    public Guid RouteId { get; private set; }
    public Guid StudentId { get; private set; }
    public DateOnly ServiceDate { get; private set; }
    public string Direction { get; private set; } = string.Empty;
    public string Status { get; private set; } = "WAITING";
    public Guid RecordedBy { get; private set; }
    public static TripRecordEntity Record(Guid tenantId, Guid routeId, Guid studentId, DateOnly date, string direction, string status, Guid userId)
    {
        var entity = new TripRecordEntity { TenantId = tenantId, RouteId = routeId, StudentId = studentId, ServiceDate = date, Direction = direction };
        entity.SetStatus(status, userId);
        return entity;
    }
    public void SetStatus(string status, Guid userId) { Status = status; RecordedBy = userId; MarkAsUpdated(); }
}
