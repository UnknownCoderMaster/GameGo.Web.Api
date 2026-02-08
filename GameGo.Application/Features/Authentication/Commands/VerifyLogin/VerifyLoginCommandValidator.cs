using FluentValidation;

namespace GameGo.Application.Features.Authentication.Commands.VerifyLogin;

public class VerifyLoginCommandValidator : AbstractValidator<VerifyLoginCommand>
{
	public VerifyLoginCommandValidator()
	{
		RuleFor(x => x.UserId)
			.GreaterThan(0).WithMessage("Invalid user ID");

		RuleFor(x => x.Code)
			.NotEmpty().WithMessage("Verification code is required")
			.Length(4).WithMessage("Verification code must be 4 digits");
	}
}
