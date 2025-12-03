using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Favourites.Queries.GetMyFavourites;

public class GetMyFavouritesQuery : IRequest<Result<PaginatedList<FavouritePlaceDto>>>
{
	public long? PlaceTypeId { get; set; }  // Filter bo'yicha
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}