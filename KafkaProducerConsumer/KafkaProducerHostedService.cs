using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaProducerConsumer;

public class KafkaProducerHostedService : IHostedService
{
    private const string Topic = "MyRandom";
    private readonly ILogger<KafkaProducerHostedService> _logger;
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerHostedService(ILogger<KafkaProducerHostedService> logger)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            AllowAutoCreateTopics = true
        };
        _logger = logger;
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        string? messageValue;
        for (var i = 0; i < 100; i++)
        {
            messageValue = $"My test message \"{Guid.NewGuid()}\"";
            _logger.LogInformation($"Produced message \"{messageValue}\"");
            await _producer.ProduceAsync(Topic, new Message<Null, string>
            {
                Value = messageValue
            }, cancellationToken);
        }

        messageValue = "End of work. OK gj";
        _logger.LogInformation($"Produced message \"{messageValue}\"");
        await _producer.ProduceAsync(Topic, new Message<Null, string>
        {
            Value = messageValue
        }, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _producer?.Dispose();
        return Task.CompletedTask;
    }
}
