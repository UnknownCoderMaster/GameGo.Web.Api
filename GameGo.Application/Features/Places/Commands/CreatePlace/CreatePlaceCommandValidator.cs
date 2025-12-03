using FluentValidation;

namespace GameGo.Application.Features.Places.Commands.CreatePlace;

public class CreatePlaceCommandValidator : AbstractValidator<CreatePlaceCommand>
{
	public CreatePlaceCommandValidator()
	{
		RuleFor(x => x.PlaceTypeId)
			.GreaterThan(0).WithMessage("Place type is required");

		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Name is required")
			.MaximumLength(150).WithMessage("Name must not exceed 150 characters");

		RuleFor(x => x.Address)
			.NotEmpty().WithMessage("Address is required")
			.MaximumLength(400).WithMessage("Address must not exceed 400 characters");

		RuleFor(x => x.Latitude)
			.InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");

		RuleFor(x => x.Longitude)
			.InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180");

		RuleFor(x => x.PhoneNumber)
			.NotEmpty().WithMessage("Phone number is required")
			.Matches(@"^\+998[0-9]{9}$").WithMessage("Phone number must be in format +998XXXXXXXXX");

		RuleFor(x => x.Email)
			.EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
			.WithMessage("Invalid email format");
	}
}