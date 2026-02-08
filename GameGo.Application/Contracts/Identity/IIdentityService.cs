using System.Threading.Tasks;

namespace GameGo.Application.Contracts.Identity;

public interface IIdentityService
{
	Task<(bool Success, long UserId)> CreateUserAsync(string email, string phoneNumber, string firstName, string lastName);

	// YANGI - PhoneNumber orqali login
	Task<(bool Success, long UserId, string Token)> LoginByPhoneAsync(string phoneNumber);
}