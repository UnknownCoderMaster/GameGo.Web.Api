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

		var user = Domain.Entities.User.Create(
			email.ToLower(),
			phoneNumber,
			firstName,
			lastName);

		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		return (true, user.Id);
	}

	// YANGI - PHONE NUMBER ORQALI LOGIN
	public async Task<(bool Success, long UserId, string Token)> LoginByPhoneAsync(string phoneNumber)
	{
		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

		if (user == null)
			return (false, 0, string.Empty);

		if (!user.IsActive)
			return (false, 0, string.Empty);

		return (true, user.Id, string.Empty);
	}
}