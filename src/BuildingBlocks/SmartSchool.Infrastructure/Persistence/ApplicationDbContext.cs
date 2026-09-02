using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Primary EF Core database context for SmartSchool.
/// The physical provider is selected through <see cref="PersistenceOptions"/>.
/// </summary>
public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor)
    : DbContext(options), IApplicationDbContext
{
    public new DbSet<TEntity> Set<TEntity>() where TEntity : Entity => base.Set<TEntity>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var pendingAuditEntries = CaptureAuditEntries();
        var affected = await base.SaveChangesAsync(cancellationToken);

        if (pendingAuditEntries.Count > 0 && Database.IsRelational())
        {
            await WriteAuditEntriesAsync(pendingAuditEntries, cancellationToken);
        }

        return affected;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var assembly in GetModuleAssemblies())
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }

        base.OnModelCreating(modelBuilder);
    }

    private List<PendingAuditEntry> CaptureAuditEntries()
    {
        ChangeTracker.DetectChanges();

        return ChangeTracker.Entries<Entity>()
            .Where(entry => entry.Entity.GetType().Name != "AuditLogEntity")
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Select(entry => new PendingAuditEntry(
                entry,
                entry.State.ToString(),
                entry.Entity.TenantId,
                SerializeValues(entry, original: true)))
            .ToList();
    }

    private async Task WriteAuditEntriesAsync(
        IReadOnlyCollection<PendingAuditEntry> entries,
        CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var userId = TryGetGuid(httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier))
            ?? TryGetGuid(httpContext?.User.FindFirstValue("sub"));
        var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString();
        var correlationId = httpContext?.TraceIdentifier;

        foreach (var pending in entries)
        {
            var entry = pending.Entry;
            var entityType = entry.Entity.GetType().Name.Replace("Entity", string.Empty, StringComparison.Ordinal);
            var entityId = GetPrimaryKey(entry);
            var newValues = pending.Action == nameof(EntityState.Deleted)
                ? null
                : SerializeValues(entry, original: false);
            var code = $"{entityType}.{pending.Action}";
            var name = $"{pending.Action} {entityType}";

            await Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO audit.audit_log
                    (tenant_id, user_id, action, entity_type, entity_id, old_values, new_values,
                     ip_address, correlation_id, occurred_at, code, name, metadata_json,
                     is_active, created_at, row_version)
                VALUES
                    ({pending.TenantId}, {userId}, {pending.Action}, {entityType}, {entityId},
                     CAST({pending.OldValues} AS jsonb), CAST({newValues} AS jsonb),
                     CAST({ipAddress} AS inet), {correlationId}, now(), {code}, {name},
                     CAST({newValues} AS jsonb), true, now(), decode('', 'hex'))
                """, cancellationToken);
        }
    }

    private static string? SerializeValues(EntityEntry entry, bool original)
    {
        if (original && entry.State == EntityState.Added)
        {
            return null;
        }

        var values = new Dictionary<string, object?>();
        foreach (var property in entry.Properties)
        {
            if (property.Metadata.IsShadowProperty()) continue;
            var value = original ? property.OriginalValue : property.CurrentValue;
            values[property.Metadata.Name] = value is byte[] bytes ? Convert.ToBase64String(bytes) : value;
        }

        return JsonSerializer.Serialize(values);
    }

    private static string? GetPrimaryKey(EntityEntry entry)
    {
        var key = entry.Metadata.FindPrimaryKey();
        if (key is null) return null;
        return string.Join("|", key.Properties.Select(property => entry.Property(property.Name).CurrentValue?.ToString()));
    }

    private static Guid? TryGetGuid(string? value) => Guid.TryParse(value, out var parsed) ? parsed : null;

    private static IEnumerable<Assembly> GetModuleAssemblies() =>
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => !assembly.IsDynamic
                && assembly.GetName().Name?.StartsWith("SmartSchool.Modules.", StringComparison.Ordinal) == true);

    private sealed record PendingAuditEntry(
        EntityEntry Entry,
        string Action,
        Guid TenantId,
        string? OldValues);
}
