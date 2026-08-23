namespace SmartSchool.Application.Messaging;

public interface IIntegrationEventPublisher
{
    Task PublishAsync<T>(string topic, T value, CancellationToken cancellationToken);
}
