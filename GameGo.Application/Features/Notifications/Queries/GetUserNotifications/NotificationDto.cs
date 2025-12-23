using GameGo.Domain.Enums;
using System;

namespace GameGo.Application.Features.Notifications.Queries.GetUserNotifications;

public class NotificationDto
{
	public long Id { get; set; }
	public string Title { get; set; }
	public string Message { get; set; }
	public NotificationType Type { get; set; }
	public string RelatedEntityType { get; set; }
	public long? RelatedEntityId { get; set; }
	public bool IsRead { get; set; }
	public DateTime CreatedAt { get; set; }
}
