using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<bool>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public UpdateProfileCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.UserId.HasValue)
			return Result<bool>.Failure("Foydalanuvchi topilmadi");

		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.Id == _currentUser.UserId.Value, cancellationToken);

		if (user == null)
			return Result<bool>.Failure("Foydalanuvchi topilmadi");

		// Parse gender
		Gender? gender = null;
		if (!string.IsNullOrEmpty(request.Gender))
		{
			gender = Enum.Parse<Gender>(request.Gender);
		}

		// Update profile
		user.UpdateProfile(
			request.FirstName,
			request.LastName,
			request.DateOfBirth,
			gender,
			request.PhoneNumber);

		await _context.SaveChangesAsync(cancellationToken);

		return Result<bool>.Success(true);
	}
}