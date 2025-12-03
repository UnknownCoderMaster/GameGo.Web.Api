using System.Threading.Tasks;
using GameGo.Application.Features.Users.Commands.ChangePassword;
using GameGo.Application.Features.Users.Commands.UpdateProfile;
using GameGo.Application.Features.Users.Commands.UploadAvatar;
using GameGo.Application.Features.Users.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameGo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
	private readonly IMediator _mediator;

	public UsersController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("profile")]
	public async Task<IActionResult> GetProfile()
	{
		var result = await _mediator.Send(new GetUserProfileQuery());

		if (!result.IsSuccess)
			return NotFound(new { error = result.Error });

		return Ok(result.Data);
	}

	[HttpPut("profile")]
	public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Profil muvaffaqiyatli yangilandi" });
	}

	[HttpPut("change-password")]
	public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Parol muvaffaqiyatli o'zgartirildi" });
	}

	[HttpPost("avatar")]
	[RequestSizeLimit(5_242_880)] // 5 MB limit
	public async Task<IActionResult> UploadAvatar(IFormFile file)
	{
		if (file == null || file.Length == 0)
			return BadRequest(new { error = "Fayl tanlanmadi" });

		if (file.Length > 5_242_880)
			return BadRequest(new { error = "Fayl hajmi 5 MB dan katta bo'lmasligi kerak" });

		using var stream = file.OpenReadStream();

		var command = new UploadAvatarCommand
		{
			ImageStream = stream,
			FileName = file.FileName
		};

		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { avatarUrl = result.Data });
	}
}