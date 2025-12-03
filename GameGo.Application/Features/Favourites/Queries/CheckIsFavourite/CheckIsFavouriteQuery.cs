using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Favourites.Queries.CheckIsFavourite;

public class CheckIsFavouriteQuery : IRequest<Result<bool>>
{
	public long PlaceId { get; set; }
}