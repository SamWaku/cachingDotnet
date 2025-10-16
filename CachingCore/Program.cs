using CachingCore.Common;
using FastEndpoints;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Scalar.AspNetCore;
using Serilog;
using StackExchange.Redis;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Application starting");
builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    var config = builder.Configuration.GetConnectionString("RedisCacheUrl");
    if (string.IsNullOrEmpty(config))
        throw new Exception("Connection not found");
    
    return ConnectionMultiplexer.Connect(config!);
});

builder.Services.AddScoped<RedisCacheService>();
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();
app.UseCors("DefaultPolicy");
app.UseAuthentication();   
app.UseAuthorization();
app.UseFastEndpoints();

app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();
app.UseSerilogRequestLogging();

//example api logging data
// app.MapGet("/serilog-example", (ILogger logger) =>
// {
//     logger.Information("Hello World!");
//     return Results.Ok();
// });
app.Run();