namespace GameGo.Application.Features.Analytics.Queries.GetPlaceOwnerDashboard;

public class PlaceOwnerDashboardDto
{
	public int TotalPlaces { get; set; }
	public int ActivePlaces { get; set; }
	public decimal AverageRating { get; set; }
	public int TotalBookings { get; set; }
	public int PendingBookings { get; set; }
	public decimal TotalRevenue { get; set; }
	public int TotalCustomers { get; set; }
	public int TotalRatings { get; set; }
}
