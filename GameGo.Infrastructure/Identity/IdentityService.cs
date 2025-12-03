using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GameGo.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
	private readonly IApplicationDbContext _context;
	private readonly IPasswordHasher<object> _passwordHasher;

	public IdentityService(IApplicationDbContext context, IPasswordHasher<object> passwordHasher)
	{
		_context = context;
		_passwordHasher = passwordHasher;
	}

	public async Task<(bool Success, long UserId)> CreateUserAsync(
		string email,
		string password,
		string phoneNumber,
		string firstName,
		string lastName)
	{
		var emailExists = await _context.Users
			.AnyAsync(u => u.Email == email.ToLower());

		if (emailExists)
			return (false, 0);

		var phoneExists = await _context.Users
			.AnyAsync(u => u.PhoneNumber == phoneNumber);

		if (phoneExists)
			return (false, 0);

		var passwordHash = _passwordHasher.HashPassword(null, password);

		var user = Domain.Entities.User.Create(
			email.ToLower(),
			passwordHash,
			phoneNumber,
			firstName,
			lastName);

		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		return (true, user.Id);
	}

	public async Task<(bool Success, long UserId, string Token)> LoginAsync(string email, string password)
	{
		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.Email == email.ToLower());

		if (user == null)
			return (false, 0, string.Empty);

		if (!user.IsActive)
			return (false, 0, string.Empty);

		var result = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password);

		if (result == PasswordVerificationResult.Failed)
			return (false, 0, string.Empty);

		return (true, user.Id, string.Empty);
	}

	// YANGI - PHONE NUMBER ORQALI LOGIN
	public async Task<(bool Success, long UserId, string Token)> LoginByPhoneAsync(string phoneNumber, string password)
	{
		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

		if (user == null)
			return (false, 0, string.Empty);

		if (!user.IsActive)
			return (false, 0, string.Empty);

		var result = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password);

		if (result == PasswordVerificationResult.Failed)
			return (false, 0, string.Empty);

		return (true, user.Id, string.Empty);
	}

	public async Task<bool> VerifyPasswordAsync(long userId, string password)
	{
		var user = await _context.Users.FindAsync(userId);

		if (user == null)
			return false;

		var result = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password);

		return result != PasswordVerificationResult.Failed;
	}

	public async Task<bool> ChangePasswordAsync(long userId, string currentPassword, string newPassword)
	{
		var user = await _context.Users.FindAsync(userId);

		if (user == null)
			return false;

		var verifyResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, currentPassword);

		if (verifyResult == PasswordVerificationResult.Failed)
			return false;

		var newPasswordHash = _passwordHasher.HashPassword(null, newPassword);
		user.UpdatePassword(newPasswordHash);

		await _context.SaveChangesAsync();

		return true;
	}

	public Task<string> HashPasswordAsync(string password)
	{
		var hash = _passwordHasher.HashPassword(null, password);
		return Task.FromResult(hash);
	}
}