using System.Text.Json;
using Confluent.Kafka;
using ConsoleApp1.Models;

namespace ConsoleApp1.Consumer;

public class StudentConsumerService(ILogger<StudentConsumerService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:29092",
            GroupId = "reader",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        using var consumer = new ConsumerBuilder<Null, string>(config).Build();
        consumer.Subscribe("students");
        logger.LogInformation("Subscribed to students.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumed = consumer.Consume(stoppingToken);
            var stds = JsonSerializer.Deserialize<Student>(consumed.Message.Value)!;
            logger.LogInformation($"Consumed student '{stds.Name}' with ID {stds.Id})");
        }
    
        consumer.Close();
    }
}