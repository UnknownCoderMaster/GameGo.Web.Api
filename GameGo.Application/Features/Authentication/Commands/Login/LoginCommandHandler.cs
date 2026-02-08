using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Entities;
using GameGo.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
	private readonly IApplicationDbContext _context;
	private readonly IIdentityService _identityService;
	private readonly ITokenService _tokenService;
	private readonly ISmsService _smsService;
	private readonly IDateTime _dateTime;

	public LoginCommandHandler(
		IApplicationDbContext context,
		IIdentityService identityService,
		ITokenService tokenService,
		ISmsService smsService,
		IDateTime dateTime)
	{
		_context = context;
		_identityService = identityService;
		_tokenService = tokenService;
		_smsService = smsService;
		_dateTime = dateTime;
	}

	public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);

		if (user == null)
			return Result<LoginResponse>.Failure("Invalid phone number or password");

		if (!user.IsActive)
			return Result<LoginResponse>.Failure("Account is deactivated");

		// Verify password
		var isValidPassword = await _identityService.VerifyPasswordAsync(user.Id, request.Password);

		if (!isValidPassword)
			return Result<LoginResponse>.Failure("Invalid phone number or password");

		// Verified user — return tokens directly
		if (user.IsPhoneVerified)
		{
			var accessToken = _tokenService.GenerateAccessToken(user.Id, user.PhoneNumber);
			var refreshToken = _tokenService.GenerateRefreshToken();

			user.UpdateRefreshToken(refreshToken, _dateTime.UtcNow.AddDays(7));
			await _context.SaveChangesAsync(cancellationToken);

			return Result<LoginResponse>.Success(new LoginResponse
			{
				UserId = user.Id,
				IsVerified = true,
				PhoneNumber = user.PhoneNumber,
				AccessToken = accessToken,
				RefreshToken = refreshToken
			});
		}

		// Not verified — send SMS
		var oldVerifications = await _context.Verifications
			.Where(v => v.UserId == user.Id
				&& v.VerificationType == VerificationType.Phone
				&& !v.IsUsed)
			.ToListAsync(cancellationToken);

		foreach (var old in oldVerifications)
		{
			old.IsUsed = true;
		}

		var verificationCode = new Random().Next(1000, 9999).ToString();

		var verification = new Verification
		{
			UserId = user.Id,
			VerificationType = VerificationType.Phone,
			Code = verificationCode,
			ExpiresAt = _dateTime.UtcNow.AddMinutes(5),
			IsUsed = false
		};

		_context.Verifications.Add(verification);
		await _context.SaveChangesAsync(cancellationToken);

		await _smsService.SendVerificationCodeAsync(request.PhoneNumber, verificationCode, cancellationToken);

		return Result<LoginResponse>.Success(new LoginResponse
		{
			UserId = user.Id,
			IsVerified = false,
			Message = "Verification code sent to your phone number"
		});
	}
}
