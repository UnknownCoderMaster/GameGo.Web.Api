using System;

namespace GameGo.Application.Features.Favourites.Queries.GetMyFavourites;

public class FavouritePlaceDto
{
	public long FavouriteId { get; set; }
	public long PlaceId { get; set; }
	public string PlaceName { get; set; }
	public string PlaceSlug { get; set; }
	public string Description { get; set; }
	public string Address { get; set; }
	public decimal Latitude { get; set; }
	public decimal Longitude { get; set; }
	public string PhoneNumber { get; set; }
	public decimal AverageRating { get; set; }
	public int TotalRatings { get; set; }

	// Place Type info
	public long PlaceTypeId { get; set; }
	public string PlaceTypeName { get; set; }
	public string PlaceTypeSlug { get; set; }
	public string PlaceTypeIcon { get; set; }

	// Primary Image
	public string PrimaryImageUrl { get; set; }

	// Added to favourites date
	public DateTime AddedAt { get; set; }
}
