using GameGo.Domain.Common;
using System;

namespace GameGo.Domain.Events;

public record PlaceCreatedEvent(long PlaceId, string Name, long OwnerId) : IDomainEvent
{
	public DateTime OccurredOn { get; } = DateTime.UtcNow;
}