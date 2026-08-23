using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;

namespace SmartSchool.Modules.Identity.Server;

public sealed record ImpersonationTicket(
    Guid ActorUserId,
    Guid TargetUserId,
    Guid? ActorTenantId,
    Guid? TargetTenantId,
    string ActorRole,
    string? Reason,
    DateTimeOffset ExpiresAtUtc);

public interface IImpersonationTicketService
{
    string Create(ImpersonationTicket ticket);
    bool TryRead(string protectedTicket, out ImpersonationTicket? ticket);
}

public sealed class ImpersonationTicketService(IDataProtectionProvider dataProtectionProvider)
    : IImpersonationTicketService
{
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector(
        "SmartSchool.Identity.Impersonation.v1");

    public string Create(ImpersonationTicket ticket) =>
        _protector.Protect(JsonSerializer.Serialize(ticket));

    public bool TryRead(string protectedTicket, out ImpersonationTicket? ticket)
    {
        ticket = null;
        if (string.IsNullOrWhiteSpace(protectedTicket)) return false;

        try
        {
            var json = _protector.Unprotect(protectedTicket);
            ticket = JsonSerializer.Deserialize<ImpersonationTicket>(json);
            return ticket is not null && ticket.ExpiresAtUtc > DateTimeOffset.UtcNow;
        }
        catch
        {
            return false;
        }
    }
}
