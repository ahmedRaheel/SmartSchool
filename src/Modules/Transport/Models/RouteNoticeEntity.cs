using SmartSchool.SharedKernel;
namespace SmartSchool.Modules.Transport.Models;
public sealed class RouteNoticeEntity : Entity
{
    private RouteNoticeEntity() { }
    public Guid RouteNoticeId { get; private set; } = Guid.NewGuid();
    public Guid RouteId { get; private set; }
    public DateOnly ServiceDate { get; private set; }
    public int DelayMinutes { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public Guid CreatedBy { get; private set; }
    public static RouteNoticeEntity Create(Guid tenantId, Guid routeId, DateOnly date, int delay, string message, Guid userId) =>
        new() { TenantId = tenantId, RouteId = routeId, ServiceDate = date, DelayMinutes = delay, Message = message.Trim(), CreatedBy = userId };
}
