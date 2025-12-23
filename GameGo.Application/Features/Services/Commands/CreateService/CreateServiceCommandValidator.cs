using FluentValidation;

namespace GameGo.Application.Features.Services.Commands.CreateService;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
	public CreateServiceCommandValidator()
	{
		RuleFor(x => x.PlaceId)
			.GreaterThan(0).WithMessage("Place ID must be greater than 0");

		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Service name is required")
			.MaximumLength(200).WithMessage("Service name cannot exceed 200 characters");

		RuleFor(x => x.Price)
			.GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");

		RuleFor(x => x.Capacity)
			.GreaterThan(0).WithMessage("Capacity must be at least 1");
	}
}
