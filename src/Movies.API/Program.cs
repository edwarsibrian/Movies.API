using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Movies.API.Configurations;
using Movies.API.Middlewares;
using Serilog;

//Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

var builder = WebApplication.CreateBuilder(args);

//Configure Serilog
builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

// Services
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApi(builder.Configuration);

// Health Checks UI
builder.Services.AddHealthChecksUI(options =>
{
    options.AddHealthCheckEndpoint("Movies.API", "/health");
})
    .AddInMemoryStorage();

var app = builder.Build();

// Logging request
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"]);
        diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress);
    };
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseOutputCache();

app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseAuthorization();

// Endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui"; // Dashboard
});

app.MapControllers();

app.Run();
