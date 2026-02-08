using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Authentication.Commands.VerifyLogin;

public class VerifyLoginCommand : IRequest<Result<VerifyLoginResponse>>
{
	public long UserId { get; set; }
	public string Code { get; set; }
}
