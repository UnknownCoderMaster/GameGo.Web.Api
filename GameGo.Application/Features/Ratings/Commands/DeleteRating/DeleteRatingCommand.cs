using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Ratings.Commands.DeleteRating;

public class DeleteRatingCommand : IRequest<Result>
{
	public long RatingId { get; set; }
}
