namespace GameGo.Application.Features.Authentication.Commands.Login;

public class LoginResponse
{
	public long UserId { get; set; }
	public string PhoneNumber { get; set; }
	public string AccessToken { get; set; }
	public string RefreshToken { get; set; }
}