using Dotnet.Homeworks.Infrastructure.Services;
using Dotnet.Homeworks.MainProject.Configuration;
using Dotnet.Homeworks.MainProject.ServicesExtensions.DataAccess;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Infrastructure;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();

var rabbitMqConfig = builder.Configuration
                         .GetSection(nameof(RabbitMqConfig))
                         .Get<RabbitMqConfig>()
                     ?? throw new ApplicationException("Not supported rabbitMq settings");

builder.Services.AddScoped<ICommunicationService, CommunicationService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection(nameof(RabbitMqConfig)));
builder.Services.AddMasstransitRabbitMq(rabbitMqConfig);
builder.Services.AddTransient<ResultFactory>();

// builder.Services.AddMediatR();
builder.Services.AddInfrastructure();
builder.Services.AddDataAccess(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.Run();