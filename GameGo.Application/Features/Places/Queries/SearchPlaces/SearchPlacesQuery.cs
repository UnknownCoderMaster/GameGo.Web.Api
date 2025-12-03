using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Places.Queries.SearchPlaces;

public class SearchPlacesQuery : IRequest<Result<PaginatedList<PlaceListDto>>>
{
	public string SearchTerm { get; set; }
	public long? PlaceTypeId { get; set; }
	public decimal? MinRating { get; set; }
	public decimal? Latitude { get; set; }
	public decimal? Longitude { get; set; }
	public double? RadiusKm { get; set; }
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 20;
}