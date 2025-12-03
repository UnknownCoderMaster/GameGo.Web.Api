using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
	private readonly IApplicationDbContext _context;
	private readonly IIdentityService _identityService;
	private readonly ITokenService _tokenService;

	public LoginCommandHandler(
		IApplicationDbContext context,
		IIdentityService identityService,
		ITokenService tokenService)
	{
		_context = context;
		_identityService = identityService;
		_tokenService = tokenService;
	}

	public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		//var user = await _context.Users
		//	.FirstOrDefaultAsync(u => u.Email == request.Email.ToLower(), cancellationToken);

		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);

		if (user == null)
			return Result<LoginResponse>.Failure("Invalid phone number or password");

		if (!user.IsActive)
			return Result<LoginResponse>.Failure("Account is deactivated");

		var isValidPassword = await _identityService.VerifyPasswordAsync(user.Id, request.Password);

		if (!isValidPassword)
			return Result<LoginResponse>.Failure("Invalid phone number or password");

		var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email);
		var refreshToken = _tokenService.GenerateRefreshToken();

		return Result<LoginResponse>.Success(new LoginResponse
		{
			UserId = user.Id,
			Email = user.Email,
			AccessToken = accessToken,
			RefreshToken = refreshToken
		});
	}
}