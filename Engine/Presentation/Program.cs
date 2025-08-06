using Confluent.Kafka;
using Engine.Application.Interfaces;
using Engine.Infrastructure.Events;
using Engine.Infrastructure.Persistence;
using Engine.Presentation;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<KafkaConfig>(
    builder.Configuration.GetSection("KafkaConfig"));

builder.Services.AddSingleton(sp =>
{
    KafkaConfig config = sp.GetRequiredService<IOptions<KafkaConfig>>().Value;
    ProducerConfig producerConfig = new() { BootstrapServers = config.BootstrapServers };
    return new ProducerBuilder<Null, string>(producerConfig).Build();
});

builder.Services.AddSingleton(sp =>
{
    KafkaConfig config = sp.GetRequiredService<IOptions<KafkaConfig>>().Value;
    ConsumerConfig consumerConfig = new()
    {
        BootstrapServers = config.BootstrapServers,
        GroupId = config.ConsumerGroupId,
        AutoOffsetReset = AutoOffsetReset.Earliest
    };
    return new ConsumerBuilder<string, string>(consumerConfig).Build();
});


// Unit of Work Services
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

builder.Services.AddServicesByConvention(typeof(EngineAssemblyReference).Assembly);

// Repositories Services
//builder.Services.AddScoped<IAuthCredentialRepository, AuthCredentialRepository>();
//builder.Services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
//builder.Services.AddScoped<IPaymentAccountRepository, PaymentAccountRepository>();
//builder.Services.AddScoped<IPaymentAccountUserRepository, PaymentAccountUserRepository>();
//builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
//builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();

// Application Services
//builder.Services.AddScoped<IAuthCredentialService, AuthCredentialService>();
//builder.Services.AddScoped<IOutboxMessageService, OutboxMessageService>();
//builder.Services.AddScoped<IPaymentAccountService, PaymentAccountService>();
//builder.Services.AddScoped<IPaymentAccountUserService, PaymentAccountUserService>();
//builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
//builder.Services.AddScoped<ITransactionService, TransactionService>();
//builder.Services.AddScoped<IUserService, UserService>();

// Password Services
//builder.Services.AddScoped<IPasswordService<User>, PasswordService<User>>();

// UseCases Services
//builder.Services.AddScoped<SignUpUseCase>();
//builder.Services.AddScoped<SignInUseCase>();

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
