using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Authentication.Commands.VerifyPhone;

public class VerifyPhoneCommand : IRequest<Result<VerifyPhoneResponse>>
{
	public string PhoneNumber { get; set; }
	public string Code { get; set; }
}
