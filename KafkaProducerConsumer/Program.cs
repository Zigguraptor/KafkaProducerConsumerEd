using KafkaProducerConsumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, collection) =>
        {
            collection.AddHostedService<KafkaProducerHostedService>();
            collection.AddHostedService<KafkaConsumerHostedService>();
        });

CreateHostBuilder(args).Build().Run();
