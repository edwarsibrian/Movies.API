using HealthChecks.UI.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Movies.API.Configurations;
using Movies.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencies(builder.Configuration);

// Health Checks: Azure Blob Storage
var azureConn = builder.Configuration.GetSection("FileStorageSettings")["ConnectionString"];

builder.Services.AddHealthChecks()
    .AddAzureBlobStorage(
        connectionString: azureConn,
        containerName: builder.Configuration.GetSection("FileStorageSettings:Containers:Actors").Value, // container de prueba
        name: "Azure Blob Storage",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "storage", "azure" }
    );

// Health Checks UI
builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

var app = builder.Build();

// Map health endpoints
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui"; // Dashboard
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseOutputCache();

app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
