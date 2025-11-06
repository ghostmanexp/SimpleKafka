using System.Text.Json;
using Confluent.Kafka;
using ConsoleApp1.Consumer;
using ConsoleApp1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string topicName = "students";

var producerConfig = new ProducerConfig { BootstrapServers = "localhost:29092" };
builder.Services.AddSingleton<IProducer<Null, string>>(_ => new ProducerBuilder<Null, string>(producerConfig).Build());

builder.Services.AddHostedService<StudentConsumerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

app.MapGet("/students", async (IProducer<Null, string> producer) =>
{
    var stds = Students.Random();
    var message = new Message<Null, string> { Value = JsonSerializer.Serialize(stds) };
    await producer.ProduceAsync(topicName, message);
    return Results.Ok($"Produced student '{stds.Name}' with ID {stds.Id})");
});

app.Run();