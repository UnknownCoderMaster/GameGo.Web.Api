namespace GameGo.Application.Features.Authentication.Commands.Register;

public class RegisterResponse
{
	public long UserId { get; set; }
	public string Email { get; set; }
	public string Message { get; set; }
}