namespace GameGo.Application.Contracts.Identity;

public interface ICurrentUserService
{
	long? UserId { get; }
	string Email { get; }
	bool IsAuthenticated { get; }
}