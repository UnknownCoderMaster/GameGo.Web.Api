using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Bookings.Commands.ConfirmBooking;

public class ConfirmBookingCommand : IRequest<Result>
{
	public long BookingId { get; set; }
}
