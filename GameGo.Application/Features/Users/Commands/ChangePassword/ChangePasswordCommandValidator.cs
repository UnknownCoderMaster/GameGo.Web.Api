using FluentValidation;

namespace GameGo.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
	public ChangePasswordCommandValidator()
	{
		RuleFor(x => x.CurrentPassword)
			.NotEmpty().WithMessage("Joriy parol kiritilishi shart");

		RuleFor(x => x.NewPassword)
			.NotEmpty().WithMessage("Yangi parol kiritilishi shart")
			.MinimumLength(8).WithMessage("Parol kamida 8 belgidan iborat bo'lishi kerak")
			.Matches("[A-Z]").WithMessage("Parolda kamida 1 ta katta harf bo'lishi kerak")
			.Matches("[a-z]").WithMessage("Parolda kamida 1 ta kichik harf bo'lishi kerak")
			.Matches("[0-9]").WithMessage("Parolda kamida 1 ta raqam bo'lishi kerak");

		RuleFor(x => x.ConfirmPassword)
			.Equal(x => x.NewPassword).WithMessage("Parollar bir xil emas");
	}
}