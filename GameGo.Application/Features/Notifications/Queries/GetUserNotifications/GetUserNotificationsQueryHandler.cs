using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Notifications.Queries.GetUserNotifications;

public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, Result<PaginatedList<NotificationDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public GetUserNotificationsQueryHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<PaginatedList<NotificationDto>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result<PaginatedList<NotificationDto>>.Failure("User must be authenticated");

		var query = _context.Notifications
			.Where(n => n.UserId == _currentUser.UserId.Value)
			.OrderByDescending(n => n.CreatedAt)
			.AsQueryable();

		// Apply filter
		if (request.IsRead.HasValue)
		{
			query = query.Where(n => n.IsRead == request.IsRead.Value);
		}

		var notificationsQuery = query.Select(n => new NotificationDto
		{
			Id = n.Id,
			Title = n.Title,
			Message = n.Message,
			Type = n.Type,
			RelatedEntityType = n.RelatedEntityType,
			RelatedEntityId = n.RelatedEntityId,
			IsRead = n.IsRead,
			CreatedAt = n.CreatedAt
		});

		var result = await PaginatedList<NotificationDto>.CreateAsync(
			notificationsQuery,
			request.PageNumber,
			request.PageSize);

		return Result<PaginatedList<NotificationDto>>.Success(result);
	}
}
