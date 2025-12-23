using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GameGo.Api.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
	{
		// Controllers
		services.AddControllers();

		// CORS
		services.AddCors(options =>
		{
			options.AddPolicy("AllowAll", builder =>
			{
				var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

				if (allowedOrigins != null && allowedOrigins.Length > 0)
				{
					builder.WithOrigins(allowedOrigins)
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials();
				}
				else
				{
					// Agar config'da yo'q bo'lsa, barchasiga ruxsat
					builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader();
				}
			});
		});

		// JWT Authentication
		var jwtSettings = configuration.GetSection("Jwt");
		var key = jwtSettings["Key"];

		if (!string.IsNullOrEmpty(key))
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = jwtSettings["Issuer"],
						ValidAudience = jwtSettings["Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
						ClockSkew = TimeSpan.Zero
					};
				});

			services.AddAuthorization();
		}

		// HttpContextAccessor
		services.AddHttpContextAccessor();

		// Swagger
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "GameGo API",
				Version = "v1",
				Description = "GameGo - O'yin-kulgi joylari va tadbirlar uchun bron qilish tizimi API'si",
				Contact = new OpenApiContact
				{
					Name = "GameGo Team",
					Email = "support@gamego.uz"
				}
			});

			// XML Comments uchun konfiguratsiya
			var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
			if (System.IO.File.Exists(xmlPath))
			{
				c.IncludeXmlComments(xmlPath);
			}

			// JWT Authentication in Swagger
			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});

			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
			});
		});

		return services;
	}
}