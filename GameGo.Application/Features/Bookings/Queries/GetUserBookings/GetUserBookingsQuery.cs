using GameGo.Application.Common.Models;
using GameGo.Domain.Enums;
using MediatR;

namespace GameGo.Application.Features.Bookings.Queries.GetUserBookings;

public class GetUserBookingsQuery : IRequest<Result<PaginatedList<BookingListDto>>>
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public BookingStatus? Status { get; set; }
}