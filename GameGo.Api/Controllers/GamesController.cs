using GameGo.Application.Features.Games.Commands.CreateGame;
using GameGo.Application.Features.Games.Commands.DeleteGame;
using GameGo.Application.Features.Games.Commands.UpdateGame;
using GameGo.Application.Features.Games.Queries.GetGames;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameGo.Api.Controllers;

/// <summary>
/// O'yinlar katalogi boshqaruvi
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
	private readonly IMediator _mediator;

	public GamesController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Barcha o'yinlarni qidirish va ro'yxatini olish
	/// </summary>
	/// <param name="query">Qidiruv va filtr parametrlari (SearchTerm, CategoryId, PageNumber, PageSize)</param>
	/// <returns>O'yinlar ro'yxati sahifalash bilan</returns>
	/// <response code="200">O'yinlar muvaffaqiyatli qaytarildi</response>
	/// <response code="400">Noto'g'ri so'rov parametrlari</response>
	[HttpGet]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetGames([FromQuery] GetGamesQuery query)
	{
		var result = await _mediator.Send(query);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}

	/// <summary>
	/// Yangi o'yin qo'shish
	/// </summary>
	/// <param name="command">O'yin ma'lumotlari (Name, Description, CategoryId, MinPlayers, MaxPlayers, MinAge)</param>
	/// <returns>Yaratilgan o'yinning ID raqami</returns>
	/// <response code="200">O'yin muvaffaqiyatli yaratildi</response>
	/// <response code="400">Noto'g'ri ma'lumotlar</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpPost]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> CreateGame([FromBody] CreateGameCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { gameId = result.Data, message = "Game created successfully" });
	}

	/// <summary>
	/// Mavjud o'yinni tahrirlash
	/// </summary>
	/// <param name="gameId">O'yin identifikatori</param>
	/// <param name="command">Yangilangan o'yin ma'lumotlari (Name, Description, CategoryId, MinPlayers, MaxPlayers, MinAge)</param>
	/// <returns>Yangilanish natijasi</returns>
	/// <response code="200">O'yin muvaffaqiyatli yangilandi</response>
	/// <response code="400">O'yin topilmadi</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpPut("{gameId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> UpdateGame(long gameId, [FromBody] UpdateGameCommand command)
	{
		command.GameId = gameId;
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Game updated successfully" });
	}

	/// <summary>
	/// O'yinni o'chirish
	/// </summary>
	/// <param name="gameId">O'yin identifikatori</param>
	/// <returns>O'chirish natijasi</returns>
	/// <response code="200">O'yin muvaffaqiyatli o'chirildi</response>
	/// <response code="400">O'yin topilmadi</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpDelete("{gameId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> DeleteGame(long gameId)
	{
		var result = await _mediator.Send(new DeleteGameCommand { GameId = gameId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Game deleted successfully" });
	}
}
