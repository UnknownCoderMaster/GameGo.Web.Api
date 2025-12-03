using FluentValidation;

namespace GameGo.Application.Features.Favourites.Commands.RemoveFromFavourites;

public class RemoveFromFavouritesCommandValidator : AbstractValidator<RemoveFromFavouritesCommand>
{
	public RemoveFromFavouritesCommandValidator()
	{
		RuleFor(x => x.PlaceId)
			.GreaterThan(0).WithMessage("Place ID noto'g'ri");
	}
}