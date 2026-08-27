using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using SmartSchool.Application.Messaging;
using SmartSchool.Infrastructure.Options;
using SmartSchool.Modules.Communication.Features.Notification;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Api.Features;

public sealed record NotificationRequestedEvent(Guid TenantId, Guid RecipientUserId, string Type, string Title, string Message,
    Guid? RelatedEntityId = null, string? RelatedEntityType = null, string? ActionUrl = null, string Priority = "Normal");

/// <summary>Consumes Kafka notification requests and executes the same persisted + SignalR notification feature used by HTTP.</summary>
public sealed class KafkaCommunicationConsumer(IServiceScopeFactory scopeFactory, IOptionsMonitor<KafkaOptions> options,
    ILogger<KafkaCommunicationConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!options.CurrentValue.Enabled) return;
        var config = new ConsumerConfig { BootstrapServers=options.CurrentValue.BootstrapServers, GroupId="smartschool-api-communication", AutoOffsetReset=AutoOffsetReset.Earliest, EnableAutoCommit=false };
        using var consumer = new ConsumerBuilder<string,string>(config).Build();
        consumer.Subscribe(KafkaTopics.NotificationRequested);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var record=consumer.Consume(stoppingToken);
                var evt=JsonSerializer.Deserialize<NotificationRequestedEvent>(record.Message.Value);
                if (evt is null) { consumer.Commit(record); continue; }
                if (!Enum.TryParse<NotificationType>(evt.Type,true,out var type)) type=NotificationType.General;
                using var scope=scopeFactory.CreateScope();
                var mediator=scope.ServiceProvider.GetRequiredService<IMediator>();
                var request=new CreateNotification.Request(evt.TenantId,evt.RecipientUserId,type,evt.Title,evt.Message,evt.RelatedEntityId,evt.RelatedEntityType,evt.ActionUrl,evt.Priority);
                var result=await mediator.SendAsync<CreateNotification.Request,Result<CreateNotification.Response>>(request,stoppingToken);
                if (result.IsFailure) throw new InvalidOperationException(result.Error.Message);
                consumer.Commit(record);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }
            catch (Exception ex) { logger.LogError(ex,"Kafka notification consumption failed; message will be retried."); await Task.Delay(TimeSpan.FromSeconds(2),stoppingToken); }
        }
        consumer.Close();
    }
}
