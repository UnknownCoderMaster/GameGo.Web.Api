using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Notifications.Commands.SendNotification;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Result<long>>
{
	private readonly IApplicationDbContext _context;

	public SendNotificationCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<long>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
	{
		// Verify user exists
		var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);

		if (!userExists)
			return Result<long>.Failure("User not found");

		// Create notification
		var notification = Notification.Create(
			request.UserId,
			request.Title,
			request.Message,
			request.Type,
			request.RelatedEntityType,
			request.RelatedEntityId);

		_context.Notifications.Add(notification);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<long>.Success(notification.Id);
	}
}
