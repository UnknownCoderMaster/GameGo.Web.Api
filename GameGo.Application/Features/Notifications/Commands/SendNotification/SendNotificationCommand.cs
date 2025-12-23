using GameGo.Application.Common.Models;
using GameGo.Domain.Enums;
using MediatR;

namespace GameGo.Application.Features.Notifications.Commands.SendNotification;

public class SendNotificationCommand : IRequest<Result<long>>
{
	public long UserId { get; set; }
	public string Title { get; set; }
	public string Message { get; set; }
	public NotificationType Type { get; set; }
	public string RelatedEntityType { get; set; }
	public long? RelatedEntityId { get; set; }
}
