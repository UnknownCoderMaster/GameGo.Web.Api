using GameGo.Application.Features.WorkingHours.Commands.CreateWorkingHours;
using GameGo.Application.Features.WorkingHours.Commands.DeleteWorkingHours;
using GameGo.Application.Features.WorkingHours.Commands.UpdateWorkingHours;
using GameGo.Application.Features.WorkingHours.Queries.GetPlaceWorkingHours;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameGo.Api.Controllers;

/// <summary>
/// Joylarning ish vaqtlari boshqaruvi
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class WorkingHoursController : ControllerBase
{
	private readonly IMediator _mediator;

	public WorkingHoursController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Joyning barcha ish vaqtlarini olish
	/// </summary>
	/// <param name="placeId">Joy identifikatori</param>
	/// <returns>Hafta kunlari bo'yicha ish vaqtlari ro'yxati</returns>
	/// <response code="200">Ish vaqtlari muvaffaqiyatli qaytarildi</response>
	/// <response code="400">Joy topilmadi</response>
	[HttpGet("place/{placeId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetPlaceWorkingHours(long placeId)
	{
		var result = await _mediator.Send(new GetPlaceWorkingHoursQuery { PlaceId = placeId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}

	/// <summary>
	/// Joy uchun yangi ish vaqti qo'shish
	/// </summary>
	/// <param name="command">Ish vaqti ma'lumotlari (PlaceId, DayOfWeek, OpenTime, CloseTime, IsClosed)</param>
	/// <returns>Yaratilgan ish vaqtining ID raqami</returns>
	/// <response code="200">Ish vaqti muvaffaqiyatli yaratildi</response>
	/// <response code="400">Noto'g'ri ma'lumotlar yoki bu kun uchun ish vaqti allaqachon mavjud</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpPost]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> CreateWorkingHours([FromBody] CreateWorkingHoursCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { workingHoursId = result.Data, message = "Working hours created successfully" });
	}

	/// <summary>
	/// Mavjud ish vaqtini tahrirlash
	/// </summary>
	/// <param name="workingHoursId">Ish vaqti identifikatori</param>
	/// <param name="command">Yangilangan ish vaqti ma'lumotlari (OpenTime, CloseTime, IsClosed)</param>
	/// <returns>Yangilanish natijasi</returns>
	/// <response code="200">Ish vaqti muvaffaqiyatli yangilandi</response>
	/// <response code="400">Ish vaqti topilmadi yoki foydalanuvchi ruxsati yo'q</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpPut("{workingHoursId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> UpdateWorkingHours(long workingHoursId, [FromBody] UpdateWorkingHoursCommand command)
	{
		var updateCommand = command with { Id = workingHoursId };
		var result = await _mediator.Send(updateCommand);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Working hours updated successfully" });
	}

	/// <summary>
	/// Ish vaqtini o'chirish
	/// </summary>
	/// <param name="workingHoursId">Ish vaqti identifikatori</param>
	/// <returns>O'chirish natijasi</returns>
	/// <response code="200">Ish vaqti muvaffaqiyatli o'chirildi</response>
	/// <response code="400">Ish vaqti topilmadi yoki foydalanuvchi ruxsati yo'q</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[Authorize]
	[HttpDelete("{workingHoursId}")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> DeleteWorkingHours(long workingHoursId)
	{
		var result = await _mediator.Send(new DeleteWorkingHoursCommand { Id = workingHoursId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Working hours deleted successfully" });
	}
}
