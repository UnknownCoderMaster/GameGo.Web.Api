namespace GameGo.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenResponse
{
	public long UserId { get; set; }
	public string PhoneNumber { get; set; } = null!;
	public string AccessToken { get; set; } = null!;
	public string RefreshToken { get; set; } = null!;
}
