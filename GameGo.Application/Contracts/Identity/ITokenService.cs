namespace GameGo.Application.Contracts.Identity;

public interface ITokenService
{
	string GenerateAccessToken(long userId, string email);
	string GenerateRefreshToken();
	long? ValidateToken(string token);
}