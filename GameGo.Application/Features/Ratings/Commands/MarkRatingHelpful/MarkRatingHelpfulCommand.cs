using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Ratings.Commands.MarkRatingHelpful;

public class MarkRatingHelpfulCommand : IRequest<Result>
{
	public long RatingId { get; set; }
	public bool IsHelpful { get; set; } = true;
}
