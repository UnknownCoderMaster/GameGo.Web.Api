using FluentValidation;

namespace GameGo.Application.Features.Authentication.Commands.VerifyPhone;

public class VerifyPhoneCommandValidator : AbstractValidator<VerifyPhoneCommand>
{
	public VerifyPhoneCommandValidator()
	{
		RuleFor(x => x.UserId)
			.GreaterThan(0).WithMessage("Invalid user ID");

		RuleFor(x => x.Code)
			.NotEmpty().WithMessage("Verification code is required")
			.Length(6).WithMessage("Verification code must be 6 digits");
	}
}