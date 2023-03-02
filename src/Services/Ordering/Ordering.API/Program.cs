using EventBus.Message.Commons;
using MassTransit;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sw =>
{
    sw.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
});

builder.Services.AddMassTransit(config =>
{
    // elided...
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("user");
            h.Password("mypass");
        });

        cfg.ReceiveEndpoint(EventBusContracts.BasketCheckoutQueue
                            , c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(context);
        });
    });
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<BasketCheckoutConsumer>();

IConfiguration config = builder.Configuration;

builder.Services.AppApplicationServices();
builder.Services.AddInfrastructureServices(config);


var app = builder.Build();
app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();
})
       ;
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
