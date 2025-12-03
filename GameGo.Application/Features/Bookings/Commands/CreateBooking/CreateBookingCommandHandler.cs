using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Entities;
using GameGo.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<long>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public CreateBookingCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<long>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result<long>.Failure("User must be authenticated");

		// Check if place exists
		var place = await _context.Places
			.Include(p => p.Services)
			.Include(p => p.WorkingHours)
			.FirstOrDefaultAsync(p => p.Id == request.PlaceId && p.IsActive, cancellationToken);

		if (place == null)
			return Result<long>.Failure("Place not found or inactive");

		// Check working hours
		var dayOfWeek = (DayOfWeek)((int)request.BookingDate.DayOfWeek == 0 ? 7 : (int)request.BookingDate.DayOfWeek);
		var workingHours = place.WorkingHours.FirstOrDefault(w => w.DayOfWeek == dayOfWeek);

		if (workingHours == null || workingHours.IsClosed)
			return Result<long>.Failure("Place is closed on this day");

		if (request.StartTime < workingHours.OpenTime || request.EndTime > workingHours.CloseTime)
			return Result<long>.Failure($"Place working hours: {workingHours.OpenTime} - {workingHours.CloseTime}");

		// Check for conflicting bookings
		var hasConflict = await _context.Bookings
			.AnyAsync(b =>
				b.PlaceId == request.PlaceId &&
				b.BookingDate == request.BookingDate &&
				b.Status != BookingStatus.Cancelled &&
				((request.StartTime >= b.StartTime && request.StartTime < b.EndTime) ||
				 (request.EndTime > b.StartTime && request.EndTime <= b.EndTime) ||
				 (request.StartTime <= b.StartTime && request.EndTime >= b.EndTime)),
				cancellationToken);

		if (hasConflict)
			return Result<long>.Failure("Time slot is not available");

		// Calculate price
		decimal totalPrice = 0;

		if (request.ServiceId.HasValue)
		{
			var service = place.Services.FirstOrDefault(s => s.Id == request.ServiceId.Value && s.IsActive);

			if (service == null)
				return Result<long>.Failure("Service not found or inactive");

			totalPrice = service.Price;
		}

		// Create booking
		var booking = Booking.Create(
			_currentUser.UserId.Value,
			request.PlaceId,
			request.BookingDate,
			request.StartTime,
			request.EndTime,
			request.NumberOfPeople,
			totalPrice,
			request.ServiceId,
			request.SpecialRequests);

		_context.Bookings.Add(booking);
		await _context.SaveChangesAsync(cancellationToken);

		place.IncrementBookingCount();
		await _context.SaveChangesAsync(cancellationToken);

		return Result<long>.Success(booking.Id);
	}
}