using FluentValidation;

namespace GameGo.Application.Features.Favourites.Commands.AddToFavourites;

public class AddToFavouritesCommandValidator : AbstractValidator<AddToFavouritesCommand>
{
	public AddToFavouritesCommandValidator()
	{
		RuleFor(x => x.PlaceId)
			.GreaterThan(0).WithMessage("Place ID noto'g'ri");
	}
}