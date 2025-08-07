using Confluent.Kafka;
using Engine.Application.Interfaces;
using Engine.Infrastructure.Events;
using Engine.Infrastructure.Events.Handlers;
using Engine.Infrastructure.Persistence;
using Engine.Presentation;
using Engine.Presentation.Extensions;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<KafkaConfig>(
    builder.Configuration.GetSection("KafkaConfig"));

// Register the KafkaProducer as a singleton
builder.Services.AddSingleton(sp =>
{
    KafkaConfig config = sp.GetRequiredService<IOptions<KafkaConfig>>().Value;
    ProducerConfig producerConfig = new() { BootstrapServers = config.BootstrapServers };
    return new ProducerBuilder<Null, string>(producerConfig).Build();
});

// Register event consumers
builder.Services
    .AddKafkaConsumerService<string, string, InternalPixTransferDebitRequestedEventHandler>(
        InternalPixTransferDebitRequestedEventHandler.Topic);

// Unit of Work Services
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

builder.Services.AddServicesByConvention(typeof(EngineAssemblyReference).Assembly);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
