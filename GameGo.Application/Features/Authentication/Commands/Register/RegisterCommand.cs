using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Authentication.Commands.Register;

public class RegisterCommand : IRequest<Result<RegisterResponse>>
{
	public string Email { get; set; }
	public string Password { get; set; }
	public string PhoneNumber { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
}