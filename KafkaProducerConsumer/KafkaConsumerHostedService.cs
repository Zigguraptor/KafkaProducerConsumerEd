using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaProducerConsumer;

public class KafkaConsumerHostedService : IHostedService
{
    private const string Topic = "MyRandom";
    private readonly ILogger<KafkaProducerHostedService> _logger;
    private readonly IConsumer<Null, string> _consumer;

    public KafkaConsumerHostedService(ILogger<KafkaProducerHostedService> logger)
    {
        var configuration = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "group1",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Null, string>(configuration).Build();
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(Topic);
        try
        {
            while (true)
            {
                var messageValue = _consumer.Consume(cancellationToken).Message.Value;
                if (messageValue == "End of work. OK gj")
                {
                    _logger.LogInformation($"End of work. OK gj");
                    break;
                }

                _logger.LogInformation($"Consumed message {messageValue}");
            }
        }
        catch (OperationCanceledException)
        {
            // Ctrl-C was pressed.
        }
        finally
        {
            _consumer.Close();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer?.Dispose();
        return Task.CompletedTask;
    }
}
