using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Common;
using GameGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	private readonly IMediator _mediator;
	private readonly IDateTime _dateTime;
	private readonly ICurrentUserService _currentUserService;

	public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IMediator mediator,
		IDateTime dateTime,
		ICurrentUserService currentUserService) : base(options)
	{
		_mediator = mediator;
		_dateTime = dateTime;
		_currentUserService = currentUserService;
	}

	public DbSet<User> Users => Set<User>();
	public DbSet<Place> Places => Set<Place>();
	public DbSet<PlaceType> PlaceTypes => Set<PlaceType>();
	public DbSet<PlaceImage> PlaceImages => Set<PlaceImage>();
	public DbSet<PlaceFeature> PlaceFeatures => Set<PlaceFeature>();
	public DbSet<PlaceOwner> PlaceOwners => Set<PlaceOwner>();
	public DbSet<WorkingHours> WorkingHours => Set<WorkingHours>();
	public DbSet<Service> Services => Set<Service>();
	public DbSet<Booking> Bookings => Set<Booking>();
	public DbSet<BookingHistory> BookingHistories => Set<BookingHistory>();
	public DbSet<Rating> Ratings => Set<Rating>();
	public DbSet<RatingHelpful> RatingHelpfuls => Set<RatingHelpful>();
	public DbSet<Game> Games => Set<Game>();
	public DbSet<Genre> Genres => Set<Genre>();
	public DbSet<Device> Devices => Set<Device>();
	public DbSet<GameGenre> GameGenres => Set<GameGenre>();
	public DbSet<GameDevice> GameDevices => Set<GameDevice>();
	public DbSet<PlaceGame> PlaceGames => Set<PlaceGame>();
	public DbSet<Favourite> Favourites => Set<Favourite>();
	public DbSet<Notification> Notifications => Set<Notification>();
	public DbSet<Verification> Verifications => Set<Verification>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(builder);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		// Update audit fields
		foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedAt = _dateTime.UtcNow;
					entry.Entity.UpdatedAt = _dateTime.UtcNow;
					break;

				case EntityState.Modified:
					entry.Entity.UpdatedAt = _dateTime.UtcNow;
					break;
			}
		}

		var result = await base.SaveChangesAsync(cancellationToken);

		// Dispatch domain events
		await DispatchDomainEvents();

		return result;
	}

	private async Task DispatchDomainEvents()
	{
		var domainEntities = ChangeTracker
			.Entries<BaseEntity>()
			.Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
			.ToList();

		var domainEvents = domainEntities
			.SelectMany(x => x.Entity.DomainEvents)
			.ToList();

		domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

		foreach (var domainEvent in domainEvents)
		{
			await _mediator.Publish(domainEvent);
		}
	}
}