using GameGo.Domain.Enums;
using System;

namespace GameGo.Application.Features.Bookings.Queries.GetUserBookings;

public class BookingListDto
{
	public long Id { get; set; }
	public string PlaceName { get; set; }
	public DateTime BookingDate { get; set; }
	public TimeSpan StartTime { get; set; }
	public TimeSpan EndTime { get; set; }
	public int NumberOfPeople { get; set; }
	public BookingStatus Status { get; set; }
	public decimal TotalPrice { get; set; }
}