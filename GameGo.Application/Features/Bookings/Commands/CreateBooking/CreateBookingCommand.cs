using GameGo.Application.Common.Models;
using MediatR;
using System;

namespace GameGo.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommand : IRequest<Result<long>>
{
	public long PlaceId { get; set; }
	public long? ServiceId { get; set; }
	public DateTime BookingDate { get; set; }
	public TimeSpan StartTime { get; set; }
	public TimeSpan EndTime { get; set; }
	public int NumberOfPeople { get; set; }
	public string SpecialRequests { get; set; }
}