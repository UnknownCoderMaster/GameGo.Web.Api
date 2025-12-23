using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Analytics.Queries.GetPlaceOwnerDashboard;

public class GetPlaceOwnerDashboardQueryHandler : IRequestHandler<GetPlaceOwnerDashboardQuery, Result<PlaceOwnerDashboardDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public GetPlaceOwnerDashboardQueryHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<PlaceOwnerDashboardDto>> Handle(GetPlaceOwnerDashboardQuery request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result<PlaceOwnerDashboardDto>.Failure("User must be authenticated");

		var places = await _context.Places
			.Where(p => p.OwnerId == _currentUser.UserId.Value)
			.ToListAsync(cancellationToken);

		if (!places.Any())
			return Result<PlaceOwnerDashboardDto>.Failure("No places found for this owner");

		var placeIds = places.Select(p => p.Id).ToList();

		var bookings = await _context.Bookings
			.Where(b => placeIds.Contains(b.PlaceId))
			.ToListAsync(cancellationToken);

		var ratings = await _context.Ratings
			.Where(r => placeIds.Contains(r.PlaceId))
			.ToListAsync(cancellationToken);

		var dashboard = new PlaceOwnerDashboardDto
		{
			TotalPlaces = places.Count,
			ActivePlaces = places.Count(p => p.IsActive),
			AverageRating = places.Any() ? (decimal)places.Average(p => p.AverageRating) : 0,
			TotalBookings = bookings.Count,
			PendingBookings = bookings.Count(b => b.Status == BookingStatus.Pending),
			TotalRevenue = bookings.Where(b => b.Status == BookingStatus.Completed).Sum(b => b.TotalPrice),
			TotalCustomers = bookings.Select(b => b.UserId).Distinct().Count(),
			TotalRatings = ratings.Count
		};

		return Result<PlaceOwnerDashboardDto>.Success(dashboard);
	}
}
