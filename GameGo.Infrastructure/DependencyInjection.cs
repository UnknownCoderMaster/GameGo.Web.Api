using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using GameGo.Infrastructure.Identity;
using GameGo.Infrastructure.Persistence;
using GameGo.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GameGo.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		// Database
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseNpgsql(
				configuration.GetConnectionString("DefaultConnection"),
				b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

		//// Redis Cache
		//services.AddStackExchangeRedisCache(options =>
		//{
		//	options.Configuration = configuration.GetConnectionString("RedisConnection");
		//	options.InstanceName = "GameGo_";
		//});

		// Identity Services
		services.AddScoped<ITokenService, TokenService>();
		services.AddScoped<IIdentityService, IdentityService>();
		services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();

		// Infrastructure Services
		services.AddSingleton<IDateTime, DateTimeService>();
		services.AddScoped<ICurrentUserService, CurrentUserService>();
		//services.AddScoped<ICacheService, CacheService>();
		services.AddScoped<IFileService, FileService>();
		services.AddScoped<IEmailService, EmailService>();

		// HttpClient for SMS service
		//services.AddHttpClient<ISmsService, SmsService>();
		services.AddHttpClient<ISmsService, SmsService>(client =>
		{
			client.Timeout = TimeSpan.FromSeconds(30);
		});

		return services;
	}
}