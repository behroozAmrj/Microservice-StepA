using Basket.API;
using Basket.API.GrpcService;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sw =>
{
    sw.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{
    string uri = builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl");
    opt.Address = new Uri(uri);
});

builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddHttpContextAccessor();


//builder.Services.AddMassTransit(busConfigurator =>
//{
//    busConfigurator.SetKebabCaseEndpointNameFormatter();
//    busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
//    {
//        busFactoryConfigurator.Host("rabbitmq", hostConfigurator => { });
//    });
//});


//builder.Services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
//{
//    cfg.Host(new Uri(RabbitMqSettings.Uri),
//    hst => 
//    {
//        hst.Username(RabbitMqSettings.UserName);
//        hst.Password(RabbitMqSettings.Password);
//    });
//}));
//
//builder.Services.AddSingleton<IBus>(prov => prov.GetRequiredService<IBusControl>());

//builder.Services.AddMassTransit(x =>
//{
//    x.UsingRabbitMq();
//});
//builder.Services.AddMassTransitHostedService();

//builder.Services.AddMassTransitHostedService();

//builder.Services.AddMassTransit(config => {
//    config.UsingRabbitMq((ctx, cfg) => {
//        cfg.Host("rabbitmq://user:mypass@localhost:5672");
//    });
//});

//builder.Services.AddMassTransitHostedService();

builder.Services.AddMassTransit(config =>
{
    // elided...

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username(RabbitMqSettings.UserName);
            h.Password(RabbitMqSettings.Password);
        });

    });
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
var app = builder.Build();

//=========================================================================
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
{
    opt.AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader();
}
);

app.UseAuthorization();

app.MapControllers();

app.Run();

