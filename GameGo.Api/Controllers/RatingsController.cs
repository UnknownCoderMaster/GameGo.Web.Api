using GameGo.Application.Features.Ratings.Commands.CreateRating;
using GameGo.Application.Features.Ratings.Commands.DeleteRating;
using GameGo.Application.Features.Ratings.Commands.MarkRatingHelpful;
using GameGo.Application.Features.Ratings.Commands.UpdateRating;
using GameGo.Application.Features.Ratings.Queries.GetPlaceRatings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameGo.Api.Controllers;

/// <summary>
/// Joylar uchun baholash va sharhlar boshqaruvi
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RatingsController : ControllerBase
{
	private readonly IMediator _mediator;

	public RatingsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Joyning barcha baholarini olish
	/// </summary>
	/// <param name="placeId">Joy identifikatori</param>
	/// <param name="query">Filtr parametrlari (PageNumber, PageSize, MinScore, VerifiedOnly)</param>
	/// <returns>Joyning baholari ro'yxati sahifalash bilan</returns>
	/// <response code="200">Baholar muvaffaqiyatli qaytarildi</response>
	/// <response code="400">Noto'g'ri so'rov parametrlari</response>
	[HttpGet("place/{placeId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetPlaceRatings(long placeId, [FromQuery] GetPlaceRatingsQuery query)
	{
		query.PlaceId = placeId;
		var result = await _mediator.Send(query);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}

	/// <summary>
	/// Joyga yangi baho va sharh qo'shish
	/// </summary>
	/// <param name="command">Baho ma'lumotlari (PlaceId, Score, Review, Pros, Cons, BookingId)</param>
	/// <returns>Yaratilgan bahoning ID raqami</returns>
	/// <response code="200">Baho muvaffaqiyatli yaratildi</response>
	/// <response code="400">Noto'g'ri ma'lumotlar yoki foydalanuvchi allaqachon baho bergan</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpPost]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> CreateRating([FromBody] CreateRatingCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { ratingId = result.Data, message = "Rating created successfully" });
	}

	/// <summary>
	/// Mavjud bahoni tahrirlash
	/// </summary>
	/// <param name="ratingId">Baho identifikatori</param>
	/// <param name="command">Yangilangan baho ma'lumotlari (Score, Review, Pros, Cons)</param>
	/// <returns>Yangilanish natijasi</returns>
	/// <response code="200">Baho muvaffaqiyatli yangilandi</response>
	/// <response code="400">Noto'g'ri ma'lumotlar yoki baho topilmadi</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpPut("{ratingId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> UpdateRating(long ratingId, [FromBody] UpdateRatingCommand command)
	{
		command.RatingId = ratingId;
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Rating updated successfully" });
	}

	/// <summary>
	/// Bahoni o'chirish
	/// </summary>
	/// <param name="ratingId">Baho identifikatori</param>
	/// <returns>O'chirish natijasi</returns>
	/// <response code="200">Baho muvaffaqiyatli o'chirildi</response>
	/// <response code="400">Baho topilmadi yoki foydalanuvchi ruxsati yo'q</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpDelete("{ratingId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> DeleteRating(long ratingId)
	{
		var result = await _mediator.Send(new DeleteRatingCommand { RatingId = ratingId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Rating deleted successfully" });
	}

	/// <summary>
	/// Bahoni foydali deb belgilash yoki belgilashni bekor qilish
	/// </summary>
	/// <param name="ratingId">Baho identifikatori</param>
	/// <returns>Belgilash natijasi</returns>
	/// <response code="200">Baho foydali deb belgilandi</response>
	/// <response code="400">Baho topilmadi</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpPost("{ratingId}/helpful")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> MarkRatingHelpful(long ratingId)
	{
		var result = await _mediator.Send(new MarkRatingHelpfulCommand { RatingId = ratingId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Marked as helpful" });
	}
}
