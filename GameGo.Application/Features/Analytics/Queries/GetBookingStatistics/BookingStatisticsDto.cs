using GameGo.Domain.Enums;

namespace GameGo.Application.Features.Analytics.Queries.GetBookingStatistics;

public class BookingStatisticsDto
{
	public int TotalBookings { get; set; }
	public int PendingBookings { get; set; }
	public int ConfirmedBookings { get; set; }
	public int CompletedBookings { get; set; }
	public int CancelledBookings { get; set; }
	public decimal TotalRevenue { get; set; }
	public int TotalCustomers { get; set; }
}
