using FluentValidation;

namespace GameGo.Application.Features.Games.Commands.CreateGame;

public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
	public CreateGameCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Game name is required")
			.MaximumLength(200).WithMessage("Game name cannot exceed 200 characters");

		RuleFor(x => x.Slug)
			.NotEmpty().WithMessage("Slug is required")
			.MaximumLength(200).WithMessage("Slug cannot exceed 200 characters");

		RuleFor(x => x.ReleaseYear)
			.InclusiveBetween(1970, 2100).WithMessage("Release year must be between 1970 and 2100")
			.When(x => x.ReleaseYear.HasValue);
	}
}
