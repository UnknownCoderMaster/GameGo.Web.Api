using FluentValidation;

namespace GameGo.Application.Features.Places.Commands.CreatePlaceType;

public class CreatePlaceTypeCommandValidator : AbstractValidator<CreatePlaceTypeCommand>
{
	public CreatePlaceTypeCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Name is required")
			.MaximumLength(50).WithMessage("Name must not exceed 50 characters");

		RuleFor(x => x.Slug)
			.NotEmpty().WithMessage("Slug is required")
			.MaximumLength(50).WithMessage("Slug must not exceed 50 characters");
	}
}
