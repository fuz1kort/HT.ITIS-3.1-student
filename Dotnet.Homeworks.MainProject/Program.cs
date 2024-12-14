using System.Diagnostics.Metrics;
using Dotnet.Homeworks.Infrastructure.Services;
using Dotnet.Homeworks.MainProject.Configuration;
using Dotnet.Homeworks.MainProject.ServicesExtensions.DataAccess;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Infrastructure;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Mapper;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;
using Dotnet.Homeworks.MainProject.ServicesExtensions.MediatR;
using Dotnet.Homeworks.MainProject.ServicesExtensions.MongoDb;
using Dotnet.Homeworks.MainProject.ServicesExtensions.OpenTelemetry;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();

builder.Services.AddOpenTelemetry(builder.Configuration
                                      .GetSection(nameof(OpenTelemetryConfig))
                                      .Get<OpenTelemetryConfig>()
                                  ?? throw new ApplicationException("Not supported openTelemetrySettings settings"));

builder.Services.AddScoped<ICommunicationService, CommunicationService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection(nameof(RabbitMqConfig)));
builder.Services.AddMasstransitRabbitMq(builder.Configuration
                                            .GetSection(nameof(RabbitMqConfig))
                                            .Get<RabbitMqConfig>()
                                        ?? throw new ApplicationException("Not supported rabbitMq settings"));

builder.Services.AddTransient<ResultFactory>();

builder.Services.AddMediatR();
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddMongoClient(builder.Configuration
                                    .GetSection(nameof(MongoDbConfig))
                                    .Get<MongoDbConfig>()
                                ?? throw new ApplicationException("Not supported mongoDb settings"));
builder.Services.AddMappers(Dotnet.Homeworks.Features.Helpers.AssemblyReference.Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", () => "Hello World!");

var meter = new Meter("Dotnet.Homeworks.Metrics");
var score = meter.CreateCounter<int>("score");

app.MapPost("/complete-sale", ([FromQuery] int count) =>
{
    score.Add(count);
});

app.MapControllers();


app.Run();