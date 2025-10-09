using CachingCore.Common;
using FastEndpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
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

var app = builder.Build();
app.UseCors("DefaultPolicy");
app.UseAuthentication();   
app.UseAuthorization();
app.UseFastEndpoints();

app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();
app.Run();