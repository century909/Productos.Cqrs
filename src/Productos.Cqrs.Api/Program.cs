using System.Text.Json.Serialization;
using Productos.Cqrs.Api.Endpoints;
using Productos.Cqrs.Api.ExceptionHandling;
using Productos.Cqrs.Application;
using Productos.Cqrs.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Composition Root
builder.Services.AddApplication();

var connectionString = builder.Configuration.GetConnectionString("BootcampDb")
    ?? throw new InvalidOperationException(
        "Falta la cadena de conexion 'ConnectionStrings:BootcampDb' en appsettings.json.");

builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapProductosEndpoints();

app.Run();
