using CachingCore.Common;
using FastEndpoints;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Caching";
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration.GetValue<string>("RedisCacheUrl");
// });

builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    var config = builder.Configuration.GetConnectionString("RedisCacheUrl");
    if (!string.IsNullOrEmpty(config))
        throw new Exception("Connection not found");
    
    return ConnectionMultiplexer.Connect(config!);
});



var app = builder.Build();
app.UseCors("DefaultPolicy");
app.UseAuthentication();   
app.UseAuthorization();
app.UseFastEndpoints();

app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();
app.Run();