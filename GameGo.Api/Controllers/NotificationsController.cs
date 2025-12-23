using GameGo.Application.Features.Notifications.Commands.MarkAsRead;
using GameGo.Application.Features.Notifications.Commands.SendNotification;
using GameGo.Application.Features.Notifications.Queries.GetUserNotifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameGo.Api.Controllers;

/// <summary>
/// Foydalanuvchi bildirishnomalari boshqaruvi
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController : ControllerBase
{
	private readonly IMediator _mediator;

	public NotificationsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Foydalanuvchining barcha bildirishnomalarini olish
	/// </summary>
	/// <param name="query">Filtr parametrlari (PageNumber, PageSize, IsRead)</param>
	/// <returns>Bildirishnomalar ro'yxati sahifalash bilan</returns>
	/// <response code="200">Bildirishnomalar muvaffaqiyatli qaytarildi</response>
	/// <response code="400">Noto'g'ri so'rov parametrlari</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[HttpGet]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetMyNotifications([FromQuery] GetUserNotificationsQuery query)
	{
		var result = await _mediator.Send(query);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}

	/// <summary>
	/// Yangi bildirishnoma yuborish
	/// </summary>
	/// <param name="command">Bildirishnoma ma'lumotlari (UserId, Title, Message, Type, RelatedEntityType, RelatedEntityId)</param>
	/// <returns>Yaratilgan bildirishnomaning ID raqami</returns>
	/// <response code="200">Bildirishnoma muvaffaqiyatli yuborildi</response>
	/// <response code="400">Noto'g'ri ma'lumotlar</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[HttpPost]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> SendNotification([FromBody] SendNotificationCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { notificationId = result.Data, message = "Notification sent successfully" });
	}

	/// <summary>
	/// Bildirishnomani o'qilgan deb belgilash
	/// </summary>
	/// <param name="notificationId">Bildirishnoma identifikatori</param>
	/// <returns>Belgilash natijasi</returns>
	/// <response code="200">Bildirishnoma o'qilgan deb belgilandi</response>
	/// <response code="400">Bildirishnoma topilmadi</response>
	/// <response code="401">Autentifikatsiya talab qilinadi</response>
	[HttpPut("{notificationId}/mark-read")]
	[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> MarkAsRead(long notificationId)
	{
		var result = await _mediator.Send(new MarkAsReadCommand { NotificationId = notificationId });

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Notification marked as read" });
	}
}
