using System.Threading.Tasks;

namespace GameGo.Application.Contracts.Identity;

public interface IIdentityService
{
	Task<(bool Success, long UserId)> CreateUserAsync(string email, string password, string phoneNumber, string firstName, string lastName);
	Task<(bool Success, long UserId, string Token)> LoginAsync(string email, string password);

	// YANGI - PhoneNumber orqali login
	Task<(bool Success, long UserId, string Token)> LoginByPhoneAsync(string phoneNumber, string password);

	Task<bool> VerifyPasswordAsync(long userId, string password);
	Task<bool> ChangePasswordAsync(long userId, string currentPassword, string newPassword);
	Task<string> HashPasswordAsync(string password);
}