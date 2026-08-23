using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using SmartSchool.Infrastructure.Options;
using SmartSchool.Modules.AICore.Cag;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Api.Features;

public sealed record CagContextInvalidationEvent(Guid TenantId, string[] Collections);

/// <summary>
/// Invalidates versioned CAG contexts from domain events. Version invalidation is intentionally
/// used instead of Redis key scans so this remains safe with multiple API instances.
/// </summary>
public sealed class KafkaCagInvalidationConsumer(
    IServiceScopeFactory scopeFactory,
    IOptionsMonitor<KafkaOptions> options,
    ILogger<KafkaCagInvalidationConsumer> logger) : BackgroundService
{
    private static readonly IReadOnlyDictionary<string, string[]> DomainCollections =
        new Dictionary<string, string[]>(StringComparer.Ordinal)
        {
            [KafkaTopics.StudentEnrolled] = ["academic", "operations", "parent"],
            [KafkaTopics.ExamResultPublished] = ["academic", "parent", "learning"],
            [KafkaTopics.AttendanceRecorded] = ["academic", "parent", "operations"],
            [KafkaTopics.FeePaymentReceived] = ["fees", "parent", "operations"]
        };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!options.CurrentValue.Enabled) return;

        var config = new ConsumerConfig
        {
            BootstrapServers = options.CurrentValue.BootstrapServers,
            GroupId = "smartschool-api-cag-invalidation",
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe([.. DomainCollections.Keys, KafkaTopics.CagContextInvalidationRequested]);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var record = consumer.Consume(stoppingToken);
                using var document = JsonDocument.Parse(record.Message.Value);
                if (!TryGetTenantId(document.RootElement, out var tenantId))
                {
                    logger.LogWarning("CAG invalidation event {Topic} did not contain a tenant id.", record.Topic);
                    consumer.Commit(record);
                    continue;
                }

                var collections = record.Topic == KafkaTopics.CagContextInvalidationRequested
                    ? ReadCollections(document.RootElement)
                    : DomainCollections[record.Topic];

                using var scope = scopeFactory.CreateScope();
                var ai = scope.ServiceProvider.GetRequiredService<IAiAssistantService>();
                foreach (var collection in collections.Distinct(StringComparer.OrdinalIgnoreCase))
                    await ai.InvalidateKnowledgeAsync(tenantId, collection, stoppingToken);

                consumer.Commit(record);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Kafka CAG invalidation failed; event will be retried.");
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }

        consumer.Close();
    }

    private static bool TryGetTenantId(JsonElement root, out Guid tenantId)
    {
        tenantId = Guid.Empty;
        foreach (var propertyName in new[] { "tenantId", "TenantId", "tenant_id" })
        {
            if (root.TryGetProperty(propertyName, out var value) && Guid.TryParse(value.GetString(), out tenantId))
                return true;
        }
        return false;
    }

    private static string[] ReadCollections(JsonElement root)
    {
        if (!root.TryGetProperty("collections", out var value) && !root.TryGetProperty("Collections", out value))
            return [];
        return value.ValueKind == JsonValueKind.Array
            ? value.EnumerateArray().Select(item => item.GetString()).Where(item => !string.IsNullOrWhiteSpace(item)).Select(item => item!).ToArray()
            : [];
    }
}
