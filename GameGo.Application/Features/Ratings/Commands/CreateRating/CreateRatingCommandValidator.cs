using FluentValidation;

namespace GameGo.Application.Features.Ratings.Commands.CreateRating;

public class CreateRatingCommandValidator : AbstractValidator<CreateRatingCommand>
{
	public CreateRatingCommandValidator()
	{
		RuleFor(x => x.PlaceId)
			.GreaterThan(0).WithMessage("Place ID must be greater than 0");

		RuleFor(x => x.Score)
			.InclusiveBetween(1, 5).WithMessage("Score must be between 1 and 5");

		RuleFor(x => x.Review)
			.MaximumLength(2000).WithMessage("Review cannot exceed 2000 characters")
			.When(x => !string.IsNullOrEmpty(x.Review));

		RuleFor(x => x.Pros)
			.MaximumLength(1000).WithMessage("Pros cannot exceed 1000 characters")
			.When(x => !string.IsNullOrEmpty(x.Pros));

		RuleFor(x => x.Cons)
			.MaximumLength(1000).WithMessage("Cons cannot exceed 1000 characters")
			.When(x => !string.IsNullOrEmpty(x.Cons));
	}
}
