using GameGo.Domain.Common;
using System;

namespace GameGo.Domain.Events;

public record BookingCreatedEvent(long BookingId, long UserId, long PlaceId, DateTime BookingDate) : IDomainEvent
{
	public DateTime OccurredOn { get; } = DateTime.UtcNow;
}