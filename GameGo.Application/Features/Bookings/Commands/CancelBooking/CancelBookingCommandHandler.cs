using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Bookings.Commands.CancelBooking;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public CancelBookingCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result.Failure("User must be authenticated");

		// Find booking
		var booking = await _context.Bookings
			.Include(b => b.Place)
			.FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

		if (booking == null)
			return Result.Failure("Booking not found");

		// User can cancel their own booking, or place owner can cancel any booking
		bool canCancel = booking.UserId == _currentUser.UserId.Value ||
						booking.Place.OwnerId == _currentUser.UserId.Value;

		if (!canCancel)
			return Result.Failure("You do not have permission to cancel this booking");

		// Cancel booking (domain method handles validation)
		try
		{
			booking.Cancel(request.CancellationReason ?? "No reason provided");
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
		catch (System.Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
