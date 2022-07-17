using Microsoft.EntityFrameworkCore;
using MetricsApp.Data;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseMetricsWebTracking();
builder.Host.UseMetrics(options =>
{
    options.EndpointOptions = endpointsOptions =>
    {
        endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
        endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
        endpointsOptions.EnvironmentInfoEndpointEnabled = false;
    };
});

builder.Services.AddDbContext<CustomerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomerContext"), sqlOptions => sqlOptions.EnableRetryOnFailure()));

// Add services to the container.
//builder.Services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
//{
//    options.AllowSynchronousIO = true;
//});

builder.Services.AddControllers();
builder.Services.AddMetrics();
builder.Services.AddMetricsTrackingMiddleware();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMetricsAllMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
