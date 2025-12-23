using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Ratings.Queries.GetPlaceRatings;

public class GetPlaceRatingsQuery : IRequest<Result<PaginatedList<RatingDto>>>
{
	public long PlaceId { get; set; }
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public int? MinScore { get; set; }
	public bool? VerifiedOnly { get; set; }
}
