using GameGo.Domain.Common;
using System;

namespace GameGo.Domain.Events;

public record RatingCreatedEvent(long RatingId, long PlaceId, int Score) : IDomainEvent
{
	public DateTime OccurredOn { get; } = DateTime.UtcNow;
}