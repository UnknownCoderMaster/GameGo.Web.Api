using GameGo.Api.Extensions;
using GameGo.Application;
using GameGo.Infrastructure;
using GameGo.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);


// Port configuration for Railway
//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
//builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Serilog Configuration
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("logs/gamego-.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "GameGo API V1");
		c.RoutePrefix = "swagger"; // Swagger UI at root
	});
//}

app.UseApiMiddleware();

app.UseHttpsRedirection();
app.UseStaticFiles(); // For serving uploaded files

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));


// Database initialization and seeding
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	try
	{
		var context = services.GetRequiredService<ApplicationDbContext>();
		var logger = services.GetRequiredService<ILogger<ApplicationDbContextInitializer>>();

		var initializer = new ApplicationDbContextInitializer(context, logger);
		await initializer.InitializeAsync();
		await initializer.SeedAsync();

		Log.Information("✅ Database initialized and seeded successfully");
	}
	catch (Exception ex)
	{
		Log.Error(ex, "❌ An error occurred during database initialization");
		// Development'da exception throw qilamiz
		if (app.Environment.IsDevelopment())
		{
			throw;
		}
	}
}


try
{
	Log.Information("Starting GameGo API");
	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}