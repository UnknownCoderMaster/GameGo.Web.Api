using FluentValidation;
using System;
using System.Linq;

namespace GameGo.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
	public UpdateProfileCommandValidator()
	{
		RuleFor(x => x.FirstName)
			.NotEmpty().WithMessage("Ism kiritilishi shart")
			.MaximumLength(50).WithMessage("Ism 50 belgidan oshmasligi kerak");

		RuleFor(x => x.LastName)
			.NotEmpty().WithMessage("Familiya kiritilishi shart")
			.MaximumLength(50).WithMessage("Familiya 50 belgidan oshmasligi kerak");

		RuleFor(x => x.DateOfBirth)
			.LessThan(DateTime.Today).WithMessage("Tug'ilgan sana kelajakda bo'lishi mumkin emas")
			.GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Tug'ilgan sana noto'g'ri")
			.When(x => x.DateOfBirth.HasValue);

		RuleFor(x => x.Gender)
			.Must(g => string.IsNullOrEmpty(g) || new[] { "Male", "Female", "Other" }.Contains(g))
			.WithMessage("Gender qiymati Male, Female yoki Other bo'lishi kerak")
			.When(x => !string.IsNullOrEmpty(x.Gender));

		RuleFor(x => x.PhoneNumber)
			.Matches(@"^\+998[0-9]{9}$").WithMessage("Telefon raqam formati noto'g'ri. +998XXXXXXXXX formatida bo'lishi kerak")
			.When(x => !string.IsNullOrEmpty(x.PhoneNumber));
	}
}