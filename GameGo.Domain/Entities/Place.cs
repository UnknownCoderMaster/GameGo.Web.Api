using GameGo.Domain.Common;
using GameGo.Domain.ValueObjects;
using System.Collections.Generic;

namespace GameGo.Domain.Entities;

public class Place : AuditableEntity
{
	public long PlaceTypeId { get; private set; }
	public long OwnerId { get; private set; }
	public string Name { get; private set; } = null!;
	public string Slug { get; private set; } = null!;
	public string Description { get; private set; }
	public string Address { get; private set; } = null!;
	public decimal Latitude { get; private set; }
	public decimal Longitude { get; private set; }
	public string PhoneNumber { get; private set; } = null!;
	public string Email { get; private set; }
	public string Website { get; private set; }
	public string InstagramUsername { get; private set; }
	public string TelegramUsername { get; private set; }
	public decimal AverageRating { get; private set; }
	public int TotalRatings { get; private set; }
	public int TotalBookings { get; private set; }
	public bool IsActive { get; private set; }
	public bool IsVerified { get; private set; }

	// Navigation Properties
	public virtual PlaceType PlaceType { get; private set; } = null!;
	public virtual User Owner { get; private set; } = null!;
	public virtual ICollection<PlaceImage> PlaceImages { get; private set; } = new List<PlaceImage>();
	public virtual ICollection<PlaceFeature> PlaceFeatures { get; private set; } = new List<PlaceFeature>();
	public virtual ICollection<WorkingHours> WorkingHours { get; private set; } = new List<WorkingHours>();
	public virtual ICollection<Service> Services { get; private set; } = new List<Service>();
	public virtual ICollection<PlaceGame> PlaceGames { get; private set; } = new List<PlaceGame>();
	public virtual ICollection<Booking> Bookings { get; private set; } = new List<Booking>();
	public virtual ICollection<Rating> Ratings { get; private set; } = new List<Rating>();

	private Place() { }

	public static Place Create(
		long placeTypeId,
		long ownerId,
		string name,
		string slug,
		string address,
		decimal latitude,
		decimal longitude,
		string phoneNumber,
		string description = null)
	{
		var coordinate = new Coordinate(latitude, longitude);

		return new Place
		{
			PlaceTypeId = placeTypeId,
			OwnerId = ownerId,
			Name = name,
			Slug = slug.ToLower(),
			Description = description,
			Address = address,
			Latitude = latitude,
			Longitude = longitude,
			PhoneNumber = phoneNumber,
			IsActive = true,
			IsVerified = false,
			AverageRating = 0,
			TotalRatings = 0,
			TotalBookings = 0
		};
	}

	public void UpdateRating(decimal newAverageRating, int totalRatings)
	{
		AverageRating = newAverageRating;
		TotalRatings = totalRatings;
	}

	public void IncrementBookingCount() => TotalBookings++;

	public void Verify() => IsVerified = true;

	public void Deactivate() => IsActive = false;

	public void Activate() => IsActive = true;
}