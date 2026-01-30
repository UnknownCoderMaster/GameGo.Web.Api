using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
	private readonly IApplicationDbContext _context;
	private readonly ITokenService _tokenService;
	private readonly IDateTime _dateTime;

	public RefreshTokenCommandHandler(
		IApplicationDbContext context,
		ITokenService tokenService,
		IDateTime dateTime)
	{
		_context = context;
		_tokenService = tokenService;
		_dateTime = dateTime;
	}

	public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);

		if (user == null)
			return Result<RefreshTokenResponse>.Failure("Invalid refresh token");

		if (user.RefreshTokenExpiresAt < _dateTime.UtcNow)
		{
			user.RevokeRefreshToken();
			await _context.SaveChangesAsync(cancellationToken);
			return Result<RefreshTokenResponse>.Failure("Refresh token has expired");
		}

		if (!user.IsActive)
			return Result<RefreshTokenResponse>.Failure("Account is deactivated");

		// Generate new tokens
		var newAccessToken = _tokenService.GenerateAccessToken(user.Id, user.PhoneNumber);
		var newRefreshToken = _tokenService.GenerateRefreshToken();

		// Update refresh token (token rotation for security)
		user.UpdateRefreshToken(newRefreshToken, _dateTime.UtcNow.AddDays(7));
		await _context.SaveChangesAsync(cancellationToken);

		return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse
		{
			UserId = user.Id,
			PhoneNumber = user.PhoneNumber,
			AccessToken = newAccessToken,
			RefreshToken = newRefreshToken
		});
	}
}
