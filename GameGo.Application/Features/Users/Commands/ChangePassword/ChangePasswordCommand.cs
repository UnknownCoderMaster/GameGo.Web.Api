using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Result<bool>>
{
	public string CurrentPassword { get; set; }
	public string NewPassword { get; set; }
	public string ConfirmPassword { get; set; }
}