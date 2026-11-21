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
builder.Services.AddInfrastructure(builder.Configuration["Persistence:DatabaseName"] ?? "ProductosDb");

var app = builder.Build();

app.Services.InicializarBaseDeDatos();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapProductosEndpoints();

app.Run();
