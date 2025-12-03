using GameGo.Domain.Common;
using GameGo.Domain.Events;
using System;
using System.Collections.Generic;

namespace GameGo.Domain.Entities;

public class Rating : AuditableEntity
{
	public long UserId { get; private set; }
	public long PlaceId { get; private set; }
	public long? BookingId { get; private set; }
	public int Score { get; private set; }
	public string Review { get; private set; }
	public string Pros { get; private set; }
	public string Cons { get; private set; }
	public bool IsAnonymous { get; private set; }
	public bool IsVerified { get; private set; }
	public int HelpfulCount { get; private set; }

	// Navigation Properties
	public virtual User User { get; private set; } = null!;
	public virtual Place Place { get; private set; } = null!;
	public virtual Booking Booking { get; private set; }
	public virtual ICollection<RatingHelpful> RatingHelpfuls { get; private set; } = new List<RatingHelpful>();

	private Rating() { }

	public static Rating Create(
		long userId,
		long placeId,
		int score,
		long? bookingId = null,
		string review = null,
		string pros = null,
		string cons = null,
		bool isAnonymous = false)
	{
		if (score < 1 || score > 5)
			throw new ArgumentException("Score must be between 1 and 5", nameof(score));

		var rating = new Rating
		{
			UserId = userId,
			PlaceId = placeId,
			BookingId = bookingId,
			Score = score,
			Review = review,
			Pros = pros,
			Cons = cons,
			IsAnonymous = isAnonymous,
			IsVerified = bookingId.HasValue,
			HelpfulCount = 0
		};

		rating.AddDomainEvent(new RatingCreatedEvent(rating.Id, placeId, score));
		return rating;
	}

	public void IncrementHelpful() => HelpfulCount++;

	public void DecrementHelpful() => HelpfulCount = Math.Max(0, HelpfulCount - 1);
}