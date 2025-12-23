using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Ratings.Commands.CreateRating;

public class CreateRatingCommand : IRequest<Result<long>>
{
	public long PlaceId { get; set; }
	public long? BookingId { get; set; }
	public int Score { get; set; }
	public string Review { get; set; }
	public string Pros { get; set; }
	public string Cons { get; set; }
	public bool IsAnonymous { get; set; }
}
