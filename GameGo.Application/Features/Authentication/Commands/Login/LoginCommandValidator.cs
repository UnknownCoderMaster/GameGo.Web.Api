using FluentValidation;

namespace GameGo.Application.Features.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	public LoginCommandValidator()
	{
		//RuleFor(x => x.Email)
		//	.NotEmpty().WithMessage("Email is required")
		//	.EmailAddress().WithMessage("Invalid email format");

		RuleFor(x => x.PhoneNumber)
			.NotEmpty().WithMessage("Phone number is required")
			.Matches(@"^\+998\d{9}$").WithMessage("Phone number must be in the format +998XXXXXXXXX")
			.WithMessage("Invalid phone number format");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required");
	}
}