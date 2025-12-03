using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Bookings.Queries.GetUserBookings;

public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, Result<PaginatedList<BookingListDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public GetUserBookingsQueryHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<PaginatedList<BookingListDto>>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result<PaginatedList<BookingListDto>>.Failure("User must be authenticated");

		var query = _context.Bookings
			.Where(b => b.UserId == _currentUser.UserId.Value)
			.OrderByDescending(b => b.BookingDate)
			.ThenByDescending(b => b.StartTime)
			.AsQueryable();

		if (request.Status.HasValue)
		{
			query = query.Where(b => b.Status == request.Status.Value);
		}

		var bookingsQuery = query.Select(b => new BookingListDto
		{
			Id = b.Id,
			PlaceName = b.Place.Name,
			BookingDate = b.BookingDate,
			StartTime = b.StartTime,
			EndTime = b.EndTime,
			NumberOfPeople = b.NumberOfPeople,
			Status = b.Status,
			TotalPrice = b.TotalPrice
		});

		var result = await PaginatedList<BookingListDto>.CreateAsync(
			bookingsQuery,
			request.PageNumber,
			request.PageSize);

		return Result<PaginatedList<BookingListDto>>.Success(result);
	}
}