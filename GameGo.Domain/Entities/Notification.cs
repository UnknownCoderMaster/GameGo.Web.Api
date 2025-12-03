using GameGo.Domain.Common;
using GameGo.Domain.Entities;
using GameGo.Domain.Enums;
using System;

public class Notification : AuditableEntity
{
	public long UserId { get; private set; }
	public string Title { get; private set; } = null!;
	public string Message { get; private set; } = null!;
	public NotificationType Type { get; private set; }
	public string RelatedEntityType { get; private set; }
	public long? RelatedEntityId { get; private set; }
	public bool IsRead { get; private set; }
	public DateTime? ReadAt { get; private set; }

	public virtual User User { get; private set; } = null!;

	private Notification() { }
}