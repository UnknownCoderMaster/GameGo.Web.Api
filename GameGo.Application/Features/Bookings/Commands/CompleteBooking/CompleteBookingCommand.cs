using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Bookings.Commands.CompleteBooking;

public class CompleteBookingCommand : IRequest<Result>
{
	public long BookingId { get; set; }
}
