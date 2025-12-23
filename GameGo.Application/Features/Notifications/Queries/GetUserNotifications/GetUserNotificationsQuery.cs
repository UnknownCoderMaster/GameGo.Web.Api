using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Notifications.Queries.GetUserNotifications;

public class GetUserNotificationsQuery : IRequest<Result<PaginatedList<NotificationDto>>>
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public bool? IsRead { get; set; }
}
