using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Analytics.Queries.GetBookingStatistics;

public class GetBookingStatisticsQueryHandler : IRequestHandler<GetBookingStatisticsQuery, Result<BookingStatisticsDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public GetBookingStatisticsQueryHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<BookingStatisticsDto>> Handle(GetBookingStatisticsQuery request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result<BookingStatisticsDto>.Failure("User must be authenticated");

		var query = _context.Bookings.AsQueryable();

		// Filter by place (if place owner)
		if (request.PlaceId.HasValue)
		{
			var place = await _context.Places
				.FirstOrDefaultAsync(p => p.Id == request.PlaceId.Value, cancellationToken);

			if (place == null)
				return Result<BookingStatisticsDto>.Failure("Place not found");

			if (place.OwnerId != _currentUser.UserId.Value)
				return Result<BookingStatisticsDto>.Failure("You can only view statistics for your own places");

			query = query.Where(b => b.PlaceId == request.PlaceId.Value);
		}
		else
		{
			// User's own bookings
			query = query.Where(b => b.UserId == _currentUser.UserId.Value);
		}

		// Filter by date range
		if (request.StartDate.HasValue)
		{
			query = query.Where(b => b.BookingDate >= request.StartDate.Value);
		}

		if (request.EndDate.HasValue)
		{
			query = query.Where(b => b.BookingDate <= request.EndDate.Value);
		}

		var bookings = await query.ToListAsync(cancellationToken);

		var statistics = new BookingStatisticsDto
		{
			TotalBookings = bookings.Count,
			PendingBookings = bookings.Count(b => b.Status == BookingStatus.Pending),
			ConfirmedBookings = bookings.Count(b => b.Status == BookingStatus.Confirmed),
			CompletedBookings = bookings.Count(b => b.Status == BookingStatus.Completed),
			CancelledBookings = bookings.Count(b => b.Status == BookingStatus.Cancelled),
			TotalRevenue = bookings.Where(b => b.Status == BookingStatus.Completed).Sum(b => b.TotalPrice),
			TotalCustomers = bookings.Select(b => b.UserId).Distinct().Count()
		};

		return Result<BookingStatisticsDto>.Success(statistics);
	}
}
