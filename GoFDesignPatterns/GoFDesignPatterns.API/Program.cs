using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gang of Four & Modern Enterprise Design Patterns API",
        Version = "v1",
        Description = "Interactive API reference showcasing all 23 classic Gang of Four (GoF) design patterns " +
                      "and modern enterprise architectural patterns (CQRS, Specification, Outbox, Circuit Breaker, etc.) in .NET 8 / C# 12.",
        Contact = new OpenApiContact
        {
            Name = "Sharjeel Qasim",
            Url = new Uri("https://github.com/sharjeel-qasim/Gang-of-Four-Design-Patterns")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Design Patterns API v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Gang of Four & Modern Patterns Interactive Swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
