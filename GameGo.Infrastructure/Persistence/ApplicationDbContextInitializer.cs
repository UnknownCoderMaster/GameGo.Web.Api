// ============================================
// ApplicationDbContextInitializer.cs
// Location: src/GameGo.Infrastructure/Persistence/
// ============================================
using System;
using System.Linq;
using System.Threading.Tasks;
using GameGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameGo.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
	private readonly ApplicationDbContext _context;
	private readonly ILogger<ApplicationDbContextInitializer> _logger;

	public ApplicationDbContextInitializer(
		ApplicationDbContext context,
		ILogger<ApplicationDbContextInitializer> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task InitializeAsync()
	{
		try
		{
			// Apply pending migrations
			if (_context.Database.GetPendingMigrations().Any())
			{
				await _context.Database.MigrateAsync();
				_logger.LogInformation("Database migrations applied successfully");
			}
			else
			{
				_logger.LogInformation("Database is up to date");
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while migrating the database");
			throw;
		}
	}

	public async Task SeedAsync()
	{
		try
		{
			await TrySeedAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while seeding the database");
			throw;
		}
	}

	private async Task TrySeedAsync()
	{
		// Seed Place Types
		if (!await _context.PlaceTypes.AnyAsync())
		{
			_logger.LogInformation("Seeding Place Types...");

			var placeTypes = new[]
			{
				PlaceType.Create("Kompyuter Klubi", "kompyuter-klubi", "🎮", "O'yin va internet kafelar"),
				PlaceType.Create("Futbol Maydoni", "futbol-maydoni", "⚽", "Yopiq va ochiq futbol maydonlari"),
				PlaceType.Create("Restoran", "restoran", "🍽️", "Ovqatlanish va yemak xizmatlari"),
				PlaceType.Create("Kinoteatr", "kinoteatr", "🎬", "Kino ko'rish zallari"),
				PlaceType.Create("Bouling", "bouling", "🎳", "Bouling zallari"),
				PlaceType.Create("Bilyard", "bilyard", "🎱", "Bilyard zallari"),
				PlaceType.Create("Karaoke", "karaoke", "🎤", "Karaoke xonalari"),
				PlaceType.Create("Sport Zali", "sport-zali", "💪", "Fitnes markazlari"),
				PlaceType.Create("Tennis Kort", "tennis-kort", "🎾", "Tennis maydonlari"),
				PlaceType.Create("Basseyn", "basseyn", "🏊", "Suzish havzalari")
			};

			_context.PlaceTypes.AddRange(placeTypes);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Place Types seeded: {Count}", placeTypes.Length);
		}

		// Seed Genres
		if (!await _context.Genres.AnyAsync())
		{
			_logger.LogInformation("Seeding Genres...");

			var genres = new[]
			{
				new Genre
				{
					Name = "Action",
					Slug = "action",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Genre
				{
					Name = "Adventure",
					Slug = "adventure",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Genre
				{
					Name = "RPG",
					Slug = "rpg",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Genre
				{
					Name = "Strategy",
					Slug = "strategy",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Genre
				{
					Name = "Sports",
					Slug = "sports",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Genre
				{
					Name = "Racing",
					Slug = "racing",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Genre
				{
					Name = "Fighting",
					Slug = "fighting",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Genre
				{
					Name = "Shooter",
					Slug = "shooter",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Genre
				{
					Name = "Puzzle",
					Slug = "puzzle",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Genre
				{
					Name = "Horror",
					Slug = "horror",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				}
			};

			_context.Genres.AddRange(genres);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Genres seeded: {Count}", genres.Length);
		}

		// Seed Devices
		if (!await _context.Devices.AnyAsync())
		{
			_logger.LogInformation("Seeding Devices...");

			var devices = new[]
			{
				new Device
				{
					Name = "PlayStation 5",
					Slug = "ps5",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Device
				{
					Name = "PlayStation 4",
					Slug = "ps4",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Device
				{
					Name = "Xbox Series X",
					Slug = "xbox-series-x",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Device
				{
					Name = "Xbox Series S",
					Slug = "xbox-series-s",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Device
				{
					Name = "Xbox One",
					Slug = "xbox-one",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Device
				{
					Name = "PC",
					Slug = "pc",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Device
				{
					Name = "Nintendo Switch",
					Slug = "switch",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				},
				new Device
				{
					Name = "VR Headset",
					Slug = "vr",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				}
			};

			_context.Devices.AddRange(devices);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Devices seeded: {Count}", devices.Length);
		}

		_logger.LogInformation("✅ Database seeding completed successfully!");
	}
}