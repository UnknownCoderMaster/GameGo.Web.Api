namespace GameGo.Application.Features.Authentication.Commands.Login;

public class LoginResponse
{
	public long UserId { get; set; }
	public bool IsVerified { get; set; }
	public string Message { get; set; }

	// Verified user uchun
	public string PhoneNumber { get; set; }
	public string AccessToken { get; set; }
	public string RefreshToken { get; set; }
}
