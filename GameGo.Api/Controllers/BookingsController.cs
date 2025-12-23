using System.Threading.Tasks;
using GameGo.Application.Features.Bookings.Commands.CancelBooking;
using GameGo.Application.Features.Bookings.Commands.CompleteBooking;
using GameGo.Application.Features.Bookings.Commands.ConfirmBooking;
using GameGo.Application.Features.Bookings.Commands.CreateBooking;
using GameGo.Application.Features.Bookings.Queries.GetUserBookings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameGo.Api.Controllers;

/// <summary>
/// Joylarni bron qilish boshqaruvi
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
	private readonly IMediator _mediator;

	public BookingsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Yangi bron yaratish
	/// </summary>
	/// <param name="command">Bron ma'lumotlari (PlaceId, BookingDate, StartTime, EndTime, NumberOfGuests, SpecialRequests)</param>
	/// <returns>Yaratilgan bronning ID raqami</returns>
	/// <response code="200">Bron muvaffaqiyatli yaratildi</response>
	/// <response code="400">Noto'g'ri ma'lumotlar yoki joy band</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[HttpPost]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { bookingId = result.Data, message = "Booking created successfully" });
	}

	/// <summary>
	/// Foydalanuvchining barcha bronlarini olish
	/// </summary>
	/// <param name="query">Filtr parametrlari (Status, PageNumber, PageSize)</param>
	/// <returns>Foydalanuvchi bronlari ro'yxati sahifalash bilan</returns>
	/// <response code="200">Bronlar muvaffaqiyatli qaytarildi</response>
	/// <response code="400">Noto'g'ri so'rov parametrlari</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[HttpGet("my-bookings")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetMyBookings([FromQuery] GetUserBookingsQuery query)
	{
		var result = await _mediator.Send(query);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}

	/// <summary>
	/// Bronni tasdiqlash (faqat joy egasi)
	/// </summary>
	/// <param name="bookingId">Bron identifikatori</param>
	/// <returns>Tasdiqlash natijasi</returns>
	/// <response code="200">Bron muvaffaqiyatli tasdiqlandi</response>
	/// <response code="400">Bron topilmadi yoki foydalanuvchi joy egasi emas</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[HttpPut("{bookingId}/confirm")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> ConfirmBooking(long bookingId)
	{
		var result = await _mediator.Send(new ConfirmBookingCommand { BookingId = bookingId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Booking confirmed successfully" });
	}

	/// <summary>
	/// Bronni bekor qilish (foydalanuvchi yoki joy egasi)
	/// </summary>
	/// <param name="bookingId">Bron identifikatori</param>
	/// <param name="command">Bekor qilish sababi (CancellationReason)</param>
	/// <returns>Bekor qilish natijasi</returns>
	/// <response code="200">Bron muvaffaqiyatli bekor qilindi</response>
	/// <response code="400">Bron topilmadi yoki bekor qilib bo'lmaydi</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[HttpPut("{bookingId}/cancel")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> CancelBooking(long bookingId, [FromBody] CancelBookingCommand command)
	{
		command.BookingId = bookingId;
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Booking cancelled successfully" });
	}

	/// <summary>
	/// Bronni yakunlash (faqat joy egasi)
	/// </summary>
	/// <param name="bookingId">Bron identifikatori</param>
	/// <returns>Yakunlash natijasi</returns>
	/// <response code="200">Bron muvaffaqiyatli yakunlandi</response>
	/// <response code="400">Bron topilmadi yoki foydalanuvchi joy egasi emas</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[HttpPut("{bookingId}/complete")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> CompleteBooking(long bookingId)
	{
		var result = await _mediator.Send(new CompleteBookingCommand { BookingId = bookingId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Booking completed successfully" });
	}
}
