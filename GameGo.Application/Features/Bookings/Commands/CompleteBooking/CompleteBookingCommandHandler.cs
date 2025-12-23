using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Bookings.Commands.CompleteBooking;

public class CompleteBookingCommandHandler : IRequestHandler<CompleteBookingCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public CompleteBookingCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(CompleteBookingCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result.Failure("User must be authenticated");

		// Find booking
		var booking = await _context.Bookings
			.Include(b => b.Place)
			.FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

		if (booking == null)
			return Result.Failure("Booking not found");

		// Only place owner can complete bookings
		if (booking.Place.OwnerId != _currentUser.UserId.Value)
			return Result.Failure("Only place owner can complete bookings");

		// Complete booking (domain method handles validation)
		try
		{
			booking.Complete();
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
		catch (System.Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
