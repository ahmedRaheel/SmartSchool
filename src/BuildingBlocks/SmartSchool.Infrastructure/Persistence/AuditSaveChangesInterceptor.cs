using System.Collections.Concurrent;
using System.Data;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SmartSchool.SharedKernel;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Cross-cutting EF Core audit interceptor applied to every SmartSchool DbContext.
/// This keeps audit behavior out of individual vertical slices while ensuring
/// module-owned DbContexts (HR, Students, Admissions, etc.) are audited too.
/// </summary>
public sealed class AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    : SaveChangesInterceptor
{
    private readonly ConcurrentDictionary<Guid, IReadOnlyCollection<PendingAuditEntry>> _pending = new();

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        Capture(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Capture(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if (eventData.Context is not null)
            WriteAsync(eventData.Context, CancellationToken.None).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            await WriteAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        Remove(eventData.Context);
        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        Remove(eventData.Context);
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    private void Capture(DbContext? context)
    {
        if (context is null || !context.Database.IsRelational()) return;

        // AuditDbContext writes audit records themselves and must never recursively audit audit_log.
        if (context.GetType().Name == "AuditDbContext") return;

        context.ChangeTracker.DetectChanges();
        var entries = context.ChangeTracker.Entries<Entity>()
            .Where(x => x.Entity.GetType().Name != "AuditLogEntity")
            .Where(x => x.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Select(x => new PendingAuditEntry(
                x,
                x.State.ToString(),
                x.Entity.TenantId,
                SerializeValues(x, original: true)))
            .ToArray();

        if (entries.Length > 0) _pending[context.ContextId.InstanceId] = entries;
    }

    private async Task WriteAsync(DbContext context, CancellationToken cancellationToken)
    {
        if (!_pending.TryRemove(context.ContextId.InstanceId, out var entries) || entries.Count == 0)
            return;

        var httpContext = httpContextAccessor.HttpContext;
        var userId = TryGetGuid(httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier))
            ?? TryGetGuid(httpContext?.User.FindFirstValue("sub"));
        var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString();
        var correlationId = httpContext?.TraceIdentifier;

        var connection = context.Database.GetDbConnection();
        var shouldClose = connection.State != ConnectionState.Open;
        if (shouldClose) await connection.OpenAsync(cancellationToken);

        try
        {
            foreach (var pending in entries)
            {
                var entry = pending.Entry;
                var entityType = entry.Entity.GetType().Name.Replace("Entity", string.Empty, StringComparison.Ordinal);
                var entityId = GetPrimaryKey(entry);
                var newValues = pending.Action == nameof(EntityState.Deleted) ? null : SerializeValues(entry, original: false);

                await using var command = connection.CreateCommand();
                command.CommandText = """
                    INSERT INTO audit.audit_log
                        (tenant_id, user_id, action, entity_type, entity_id, old_values, new_values,
                         ip_address, correlation_id, occurred_at, code, name, metadata_json,
                         is_active, created_at, row_version)
                    VALUES
                        (@tenantId, @userId, @action, @entityType, @entityId,
                         CAST(@oldValues AS jsonb), CAST(@newValues AS jsonb),
                         CAST(@ipAddress AS inet), @correlationId, CURRENT_TIMESTAMP, @code, @name,
                         CAST(@metadataJson AS jsonb), TRUE, CURRENT_TIMESTAMP, decode('', 'hex'))
                    """;

                Add(command, "@tenantId", pending.TenantId);
                Add(command, "@userId", userId);
                Add(command, "@action", pending.Action);
                Add(command, "@entityType", entityType);
                Add(command, "@entityId", entityId);
                Add(command, "@oldValues", pending.OldValues);
                Add(command, "@newValues", newValues);
                Add(command, "@ipAddress", ipAddress);
                Add(command, "@correlationId", correlationId);
                Add(command, "@code", $"{entityType}.{pending.Action}");
                Add(command, "@name", $"{pending.Action} {entityType}");
                Add(command, "@metadataJson", newValues);
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
        finally
        {
            if (shouldClose) await connection.CloseAsync();
        }
    }

    private static void Add(System.Data.Common.DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }

    private static string? SerializeValues(EntityEntry entry, bool original)
    {
        if (original && entry.State == EntityState.Added) return null;
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
        return key is null
            ? null
            : string.Join("|", key.Properties.Select(x => entry.Property(x.Name).CurrentValue?.ToString()));
    }

    private static Guid? TryGetGuid(string? value) => Guid.TryParse(value, out var parsed) ? parsed : null;

    private void Remove(DbContext? context)
    {
        if (context is not null) _pending.TryRemove(context.ContextId.InstanceId, out _);
    }

    private sealed record PendingAuditEntry(EntityEntry Entry, string Action, Guid TenantId, string? OldValues);
}
