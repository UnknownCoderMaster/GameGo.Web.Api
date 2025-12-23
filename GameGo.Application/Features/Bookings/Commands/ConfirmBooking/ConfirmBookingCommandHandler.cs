using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Bookings.Commands.ConfirmBooking;

public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public ConfirmBookingCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result.Failure("User must be authenticated");

		// Find booking
		var booking = await _context.Bookings
			.Include(b => b.Place)
			.FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

		if (booking == null)
			return Result.Failure("Booking not found");

		// Only place owner can confirm bookings
		if (booking.Place.OwnerId != _currentUser.UserId.Value)
			return Result.Failure("Only place owner can confirm bookings");

		// Confirm booking (domain method handles validation)
		try
		{
			booking.Confirm();
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
		catch (System.Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
