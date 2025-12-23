using GameGo.Application.Features.Services.Commands.CreateService;
using GameGo.Application.Features.Services.Commands.DeleteService;
using GameGo.Application.Features.Services.Commands.UpdateService;
using GameGo.Application.Features.Services.Queries.GetPlaceServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameGo.Api.Controllers;

/// <summary>
/// Joylardagi qo'shimcha xizmatlar boshqaruvi
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
	private readonly IMediator _mediator;

	public ServicesController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Joyning barcha xizmatlarini olish
	/// </summary>
	/// <param name="placeId">Joy identifikatori</param>
	/// <param name="query">Filtr parametrlari (IsActiveOnly)</param>
	/// <returns>Joyning xizmatlari ro'yxati</returns>
	/// <response code="200">Xizmatlar muvaffaqiyatli qaytarildi</response>
	/// <response code="400">Joy topilmadi</response>
	[HttpGet("place/{placeId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetPlaceServices(long placeId, [FromQuery] GetPlaceServicesQuery query)
	{
		query.PlaceId = placeId;
		var result = await _mediator.Send(query);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}

	/// <summary>
	/// Joyga yangi xizmat qo'shish
	/// </summary>
	/// <param name="command">Xizmat ma'lumotlari (PlaceId, Name, Description, Price, Currency, DurationMinutes, Capacity)</param>
	/// <returns>Yaratilgan xizmatning ID raqami</returns>
	/// <response code="200">Xizmat muvaffaqiyatli yaratildi</response>
	/// <response code="400">Noto'g'ri ma'lumotlar yoki foydalanuvchi ruxsati yo'q</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpPost]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { serviceId = result.Data, message = "Service created successfully" });
	}

	/// <summary>
	/// Mavjud xizmatni tahrirlash
	/// </summary>
	/// <param name="serviceId">Xizmat identifikatori</param>
	/// <param name="command">Yangilangan xizmat ma'lumotlari (Name, Description, Price, DurationMinutes, Capacity)</param>
	/// <returns>Yangilanish natijasi</returns>
	/// <response code="200">Xizmat muvaffaqiyatli yangilandi</response>
	/// <response code="400">Xizmat topilmadi yoki foydalanuvchi ruxsati yo'q</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpPut("{serviceId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> UpdateService(long serviceId, [FromBody] UpdateServiceCommand command)
	{
		command.ServiceId = serviceId;
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Service updated successfully" });
	}

	/// <summary>
	/// Xizmatni o'chirish
	/// </summary>
	/// <param name="serviceId">Xizmat identifikatori</param>
	/// <returns>O'chirish natijasi</returns>
	/// <response code="200">Xizmat muvaffaqiyatli o'chirildi</response>
	/// <response code="400">Xizmat topilmadi yoki foydalanuvchi ruxsati yo'q</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpDelete("{serviceId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> DeleteService(long serviceId)
	{
		var result = await _mediator.Send(new DeleteServiceCommand { ServiceId = serviceId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Service deleted successfully" });
	}
}
