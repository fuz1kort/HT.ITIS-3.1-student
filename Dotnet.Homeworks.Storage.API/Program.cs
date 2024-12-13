using Dotnet.Homeworks.Storage.API.Configuration;
using Dotnet.Homeworks.Storage.API.Endpoints;
using Dotnet.Homeworks.Storage.API.Services;
using Dotnet.Homeworks.Storage.API.ServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

var minioConfig = builder.Configuration.GetSection("MinioConfig");

builder.Services.Configure<MinioConfig>(minioConfig);
builder.Services.AddMinioClient(minioConfig.Get<MinioConfig>());
builder.Services.AddSingleton<IStorageFactory, StorageFactory>();
builder.Services.AddHostedService<PendingObjectsProcessor>();

var app = builder.Build();

app.MapProductsEndpoints();

app.Run();