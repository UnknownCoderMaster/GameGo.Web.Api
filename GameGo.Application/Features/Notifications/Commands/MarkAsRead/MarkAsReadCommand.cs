using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Notifications.Commands.MarkAsRead;

public class MarkAsReadCommand : IRequest<Result>
{
	public long NotificationId { get; set; }
}
