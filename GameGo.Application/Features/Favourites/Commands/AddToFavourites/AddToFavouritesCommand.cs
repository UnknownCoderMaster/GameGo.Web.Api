using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Favourites.Commands.AddToFavourites;

public class AddToFavouritesCommand : IRequest<Result<long>>
{
	public long PlaceId { get; set; }
}