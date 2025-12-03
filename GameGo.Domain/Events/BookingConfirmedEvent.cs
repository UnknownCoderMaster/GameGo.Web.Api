using GameGo.Domain.Common;
using System;

namespace GameGo.Domain.Events;

public record BookingConfirmedEvent(long BookingId, long UserId, long PlaceId) : IDomainEvent
{
	public DateTime OccurredOn { get; } = DateTime.UtcNow;
}