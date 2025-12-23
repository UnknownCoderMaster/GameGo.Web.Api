using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Ratings.Commands.UpdateRating;

public class UpdateRatingCommand : IRequest<Result>
{
	public long RatingId { get; set; }
	public int Score { get; set; }
	public string Review { get; set; }
	public string Pros { get; set; }
	public string Cons { get; set; }
}
