using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Entities;
using GameGo.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
	private readonly IApplicationDbContext _context;
	private readonly ISmsService _smsService;

	public RegisterCommandHandler(
		IApplicationDbContext context,
		ISmsService smsService)
	{
		_context = context;
		_smsService = smsService;
	}

	public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
	{
		if (!string.IsNullOrWhiteSpace(request.Email))
		{
			// Check if email already exists
			var emailExists = await _context.Users
				.AnyAsync(u => u.Email == request.Email, cancellationToken);

			if (emailExists)
				return Result<RegisterResponse>.Failure("Email already registered");
		}
		else
		{
			request.Email = $"unknown_{Guid.NewGuid()}_@email.com";
		}

		// Check if phone number already exists
		var phoneExists = await _context.Users
			.AnyAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);

		if (phoneExists)
			return Result<RegisterResponse>.Failure("Phone number already registered");

		// Create user
		var user = User.Create(
			request.Email,
			request.PhoneNumber,
			request.FirstName,
			request.LastName);

		_context.Users.Add(user);
		await _context.SaveChangesAsync(cancellationToken);

		var verificationCode = "7777";
		//var verificationCode = new Random().Next(1000, 9999).ToString();

		var verification = new Verification
		{
			UserId = user.Id,
			VerificationType = VerificationType.Phone,
			Code = verificationCode,
			ExpiresAt = DateTime.UtcNow.AddMinutes(5),
			IsUsed = false
		};

		_context.Verifications.Add(verification);
		await _context.SaveChangesAsync(cancellationToken);

		// Send SMS
		await _smsService.SendVerificationCodeAsync(request.PhoneNumber, verificationCode, cancellationToken);

		return Result<RegisterResponse>.Success(new RegisterResponse
		{
			UserId = user.Id,
			Email = user.Email,
			Message = "Registration successful. Please verify your phone number."
		});
	}
}
