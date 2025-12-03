namespace GameGo.Application.Features.Places.Queries.SearchPlaces;

public class PlaceListDto
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Slug { get; set; }
	public string Description { get; set; }
	public string Address { get; set; }
	public decimal Latitude { get; set; }
	public decimal Longitude { get; set; }
	public decimal AverageRating { get; set; }
	public int TotalRatings { get; set; }
	public string PlaceTypeName { get; set; }
	public string PrimaryImage { get; set; }
	public double? DistanceKm { get; set; }
}