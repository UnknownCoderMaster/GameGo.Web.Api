using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Bookings.Commands.CancelBooking;

public class CancelBookingCommand : IRequest<Result>
{
	public long BookingId { get; set; }
	public string CancellationReason { get; set; }
}
