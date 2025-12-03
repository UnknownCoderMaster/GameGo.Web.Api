using GameGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Contracts.Persistence;

public interface IApplicationDbContext
{
	DbSet<User> Users { get; }
	DbSet<Place> Places { get; }
	DbSet<PlaceType> PlaceTypes { get; }
	DbSet<PlaceImage> PlaceImages { get; }
	DbSet<PlaceFeature> PlaceFeatures { get; }
	DbSet<PlaceOwner> PlaceOwners { get; }
	DbSet<WorkingHours> WorkingHours { get; }
	DbSet<Service> Services { get; }
	DbSet<Booking> Bookings { get; }
	DbSet<BookingHistory> BookingHistories { get; }
	DbSet<Rating> Ratings { get; }
	DbSet<RatingHelpful> RatingHelpfuls { get; }
	DbSet<Game> Games { get; }
	DbSet<Genre> Genres { get; }
	DbSet<Device> Devices { get; }
	DbSet<GameGenre> GameGenres { get; }
	DbSet<GameDevice> GameDevices { get; }
	DbSet<PlaceGame> PlaceGames { get; }
	DbSet<Favourite> Favourites { get; }
	DbSet<Notification> Notifications { get; }
	DbSet<Verification> Verifications { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}