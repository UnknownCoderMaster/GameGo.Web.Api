using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Favourites.Commands.RemoveFromFavourites;

public class RemoveFromFavouritesCommand : IRequest<Result<bool>>
{
	public long PlaceId { get; set; }
}