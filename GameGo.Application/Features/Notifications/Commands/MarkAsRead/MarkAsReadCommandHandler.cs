using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Notifications.Commands.MarkAsRead;

public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public MarkAsReadCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result.Failure("User must be authenticated");

		// Find notification
		var notification = await _context.Notifications
			.FirstOrDefaultAsync(n => n.Id == request.NotificationId, cancellationToken);

		if (notification == null)
			return Result.Failure("Notification not found");

		// Check ownership
		if (notification.UserId != _currentUser.UserId.Value)
			return Result.Failure("You can only mark your own notifications as read");

		// Mark as read
		notification.MarkAsRead();
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
