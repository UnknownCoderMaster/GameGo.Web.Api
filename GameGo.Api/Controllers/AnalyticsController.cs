using GameGo.Application.Features.Analytics.Queries.GetBookingStatistics;
using GameGo.Application.Features.Analytics.Queries.GetPlaceOwnerDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameGo.Api.Controllers;

/// <summary>
/// Joy egalari uchun analitika va hisobotlar
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnalyticsController : ControllerBase
{
	private readonly IMediator _mediator;

	public AnalyticsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Bron qilishlar statistikasi va hisobotlarini olish
	/// </summary>
	/// <param name="query">Hisobot parametrlari (PlaceId, StartDate, EndDate, GroupBy)</param>
	/// <returns>Tanlangan davr bo'yicha bron qilishlar statistikasi (soni, daromad, o'rtacha narx)</returns>
	/// <response code="200">Statistika muvaffaqiyatli qaytarildi</response>
	/// <response code="400">Noto'g'ri parametrlar yoki joy topilmadi</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[HttpGet("booking-statistics")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetBookingStatistics([FromQuery] GetBookingStatisticsQuery query)
	{
		var result = await _mediator.Send(query);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}

	/// <summary>
	/// Joy egasi uchun umumiy dashboard ma'lumotlarini olish
	/// </summary>
	/// <returns>Joy egasining barcha joylari bo'yicha umumiy statistika (jami joylar, jami bron qilishlar, jami daromad, faol joylar soni)</returns>
	/// <response code="200">Dashboard ma'lumotlari muvaffaqiyatli qaytarildi</response>
	/// <response code="400">Xatolik yuz berdi</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[HttpGet("owner-dashboard")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetPlaceOwnerDashboard()
	{
		var result = await _mediator.Send(new GetPlaceOwnerDashboardQuery());

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}
}
