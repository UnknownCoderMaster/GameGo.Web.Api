using FluentValidation;

namespace GameGo.Application.Features.Favourites.Queries.GetMyFavourites;

public class GetMyFavouritesQueryValidator : AbstractValidator<GetMyFavouritesQuery>
{
	public GetMyFavouritesQueryValidator()
	{
		RuleFor(x => x.PlaceTypeId)
			.GreaterThan(0).When(x => x.PlaceTypeId.HasValue)
			.WithMessage("Place Type ID noto'g'ri");

		RuleFor(x => x.PageNumber)
			.GreaterThan(0).WithMessage("Page number 0 dan katta bo'lishi kerak");

		RuleFor(x => x.PageSize)
			.GreaterThan(0).WithMessage("Page size 0 dan katta bo'lishi kerak")
			.LessThanOrEqualTo(100).WithMessage("Page size 100 dan kichik bo'lishi kerak");
	}
}