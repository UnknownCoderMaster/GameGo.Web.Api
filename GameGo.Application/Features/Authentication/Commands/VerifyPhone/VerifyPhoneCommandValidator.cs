using FluentValidation;

namespace GameGo.Application.Features.Authentication.Commands.VerifyPhone;

public class VerifyPhoneCommandValidator : AbstractValidator<VerifyPhoneCommand>
{
	public VerifyPhoneCommandValidator()
	{
		RuleFor(x => x.PhoneNumber)
			.NotEmpty().WithMessage("Phone number is required")
			.Matches(@"^\+998\d{9}$").WithMessage("Phone number must be in the format +998XXXXXXXXX");

		RuleFor(x => x.Code)
			.NotEmpty().WithMessage("Verification code is required")
			.Length(4).WithMessage("Verification code must be 4 digits");
	}
}
