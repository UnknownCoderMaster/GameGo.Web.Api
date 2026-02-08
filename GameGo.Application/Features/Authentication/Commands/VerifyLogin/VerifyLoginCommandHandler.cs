using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Authentication.Commands.VerifyLogin;

public class VerifyLoginCommandHandler : IRequestHandler<VerifyLoginCommand, Result<VerifyLoginResponse>>
{
	private readonly IApplicationDbContext _context;
	private readonly ITokenService _tokenService;
	private readonly IDateTime _dateTime;

	public VerifyLoginCommandHandler(
		IApplicationDbContext context,
		ITokenService tokenService,
		IDateTime dateTime)
	{
		_context = context;
		_tokenService = tokenService;
		_dateTime = dateTime;
	}

	public async Task<Result<VerifyLoginResponse>> Handle(VerifyLoginCommand request, CancellationToken cancellationToken)
	{
		var verification = await _context.Verifications
			.FirstOrDefaultAsync(v =>
				v.UserId == request.UserId &&
				v.Code == request.Code &&
				v.VerificationType == VerificationType.Phone &&
				!v.IsUsed,
				cancellationToken);

		if (verification == null)
			return Result<VerifyLoginResponse>.Failure("Invalid verification code");

		if (verification.ExpiresAt < _dateTime.UtcNow)
			return Result<VerifyLoginResponse>.Failure("Verification code has expired");

		var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);

		if (user == null)
			return Result<VerifyLoginResponse>.Failure("User not found");

		if (!user.IsActive)
			return Result<VerifyLoginResponse>.Failure("Account is deactivated");

		// Mark verification as used
		verification.IsUsed = true;

		// Generate tokens
		var accessToken = _tokenService.GenerateAccessToken(user.Id, user.PhoneNumber);
		var refreshToken = _tokenService.GenerateRefreshToken();

		// Save refresh token
		user.UpdateRefreshToken(refreshToken, _dateTime.UtcNow.AddDays(7));
		await _context.SaveChangesAsync(cancellationToken);

		return Result<VerifyLoginResponse>.Success(new VerifyLoginResponse
		{
			UserId = user.Id,
			PhoneNumber = user.PhoneNumber,
			AccessToken = accessToken,
			RefreshToken = refreshToken
		});
	}
}
