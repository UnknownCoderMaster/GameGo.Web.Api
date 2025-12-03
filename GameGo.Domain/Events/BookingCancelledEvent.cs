using GameGo.Domain.Common;
using System;

namespace GameGo.Domain.Events;

public record BookingCancelledEvent(long BookingId, long UserId, long PlaceId, string Reason) : IDomainEvent
{
	public DateTime OccurredOn { get; } = DateTime.UtcNow;
}