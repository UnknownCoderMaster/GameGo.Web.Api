using GameGo.Application.Features.Favourites.Commands.AddToFavourites;
using GameGo.Application.Features.Favourites.Commands.RemoveFromFavourites;
using GameGo.Application.Features.Favourites.Queries.CheckIsFavourite;
using GameGo.Application.Features.Favourites.Queries.GetMyFavourites;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameGo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavouritesController : ControllerBase
{
	private readonly IMediator _mediator;

	public FavouritesController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IActionResult> GetMyFavourites([FromQuery] GetMyFavouritesQuery query)
	{
		var result = await _mediator.Send(query);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}

	[HttpPost("{placeId}")]
	public async Task<IActionResult> AddToFavourites(long placeId)
	{
		var result = await _mediator.Send(new AddToFavouritesCommand { PlaceId = placeId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { favouriteId = result.Data, message = "Sevimlilar ro'yxatiga qo'shildi" });
	}

	[HttpDelete("{placeId}")]
	public async Task<IActionResult> RemoveFromFavourites(long placeId)
	{
		var result = await _mediator.Send(new RemoveFromFavouritesCommand { PlaceId = placeId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Sevimlilar ro'yxatidan o'chirildi" });
	}

	[HttpGet("check/{placeId}")]
	public async Task<IActionResult> CheckIsFavourite(long placeId)
	{
		var result = await _mediator.Send(new CheckIsFavouriteQuery { PlaceId = placeId });

		return Ok(new { isFavourite = result.Data });
	}
}
