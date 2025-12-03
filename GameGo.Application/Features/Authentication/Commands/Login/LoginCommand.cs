using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Authentication.Commands.Login;

public class LoginCommand : IRequest<Result<LoginResponse>>
{
	public string PhoneNumber { get; set; }
	public string Password { get; set; }
}
