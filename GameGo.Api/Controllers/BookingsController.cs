using System.Threading.Tasks;
using GameGo.Application.Features.Bookings.Commands.CreateBooking;
using GameGo.Application.Features.Bookings.Queries.GetUserBookings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameGo.Api.Controllers;

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

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { bookingId = result.Data, message = "Booking created successfully" });
	}

	[HttpGet("my-bookings")]
	public async Task<IActionResult> GetMyBookings([FromQuery] GetUserBookingsQuery query)
	{
		var result = await _mediator.Send(query);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}
}