using Confluent.Kafka;
using Engine.Application.Interfaces;
using Engine.Infrastructure.Events;
using Engine.Infrastructure.Events.Handlers;
using Engine.Infrastructure.Persistence;
using Engine.Presentation;
using Engine.Presentation.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services
    .AddKafkaConsumerService<string, string, InternalPixTransferCreditRequestedEventHandler>(
        InternalPixTransferCreditRequestedEventHandler.Topic);

// Unit of Work Services
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

builder.Services.AddServicesByConvention(typeof(EngineAssemblyReference).Assembly);

// JWT Authentication Configuration
IConfigurationSection jwtSettings = builder.Configuration.GetSection("JwtSettings");
string? secret = jwtSettings["Secret"];
string? issuer = jwtSettings["Issuer"];
string? audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? ""))
    };
});

// Register a Service for authorization
builder.Services.AddAuthorization();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
