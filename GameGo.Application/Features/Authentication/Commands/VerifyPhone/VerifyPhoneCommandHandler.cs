using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Authentication.Commands.VerifyPhone;

public class VerifyPhoneCommandHandler : IRequestHandler<VerifyPhoneCommand, Result<VerifyPhoneResponse>>
{
	private readonly IApplicationDbContext _context;
	private readonly ITokenService _tokenService;
	private readonly IDateTime _dateTime;

	public VerifyPhoneCommandHandler(
		IApplicationDbContext context,
		ITokenService tokenService,
		IDateTime dateTime)
	{
		_context = context;
		_tokenService = tokenService;
		_dateTime = dateTime;
	}

	public async Task<Result<VerifyPhoneResponse>> Handle(VerifyPhoneCommand request, CancellationToken cancellationToken)
	{
		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);

		if (user == null)
			return Result<VerifyPhoneResponse>.Failure("User not found");

		var verification = await _context.Verifications
			.FirstOrDefaultAsync(v =>
				v.UserId == user.Id &&
				v.Code == request.Code &&
				v.VerificationType == VerificationType.Phone &&
				!v.IsUsed,
				cancellationToken);

		if (verification == null)
			return Result<VerifyPhoneResponse>.Failure("Invalid verification code");

		if (verification.ExpiresAt < _dateTime.UtcNow)
			return Result<VerifyPhoneResponse>.Failure("Verification code has expired");

		user.VerifyPhone();
		verification.IsUsed = true;

		// Generate tokens
		var accessToken = _tokenService.GenerateAccessToken(user.Id, user.PhoneNumber);
		var refreshToken = _tokenService.GenerateRefreshToken();

		user.UpdateRefreshToken(refreshToken, _dateTime.UtcNow.AddDays(7));
		await _context.SaveChangesAsync(cancellationToken);

		return Result<VerifyPhoneResponse>.Success(new VerifyPhoneResponse
		{
			UserId = user.Id,
			PhoneNumber = user.PhoneNumber,
			AccessToken = accessToken,
			RefreshToken = refreshToken
		});
	}
}
