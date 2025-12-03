using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Authentication.Commands.VerifyPhone;

public class VerifyPhoneCommandHandler : IRequestHandler<VerifyPhoneCommand, Result<string>>
{
	private readonly IApplicationDbContext _context;

	public VerifyPhoneCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<string>> Handle(VerifyPhoneCommand request, CancellationToken cancellationToken)
	{
		var verification = await _context.Verifications
			.FirstOrDefaultAsync(v =>
				v.UserId == request.UserId &&
				v.Code == request.Code &&
				v.VerificationType == VerificationType.Phone &&
				!v.IsUsed,
				cancellationToken);

		if (verification == null)
			return Result<string>.Failure("Invalid verification code");

		if (verification.ExpiresAt < DateTime.UtcNow)
			return Result<string>.Failure("Verification code has expired");

		var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);

		if (user == null)
			return Result<string>.Failure("User not found");

		user.VerifyPhone();
		verification.IsUsed = true;

		await _context.SaveChangesAsync(cancellationToken);

		return Result<string>.Success("Phone number verified successfully");
	}
}