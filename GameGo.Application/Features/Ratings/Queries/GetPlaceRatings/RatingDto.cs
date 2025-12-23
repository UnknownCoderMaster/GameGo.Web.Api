using System;

namespace GameGo.Application.Features.Ratings.Queries.GetPlaceRatings;

public class RatingDto
{
	public long Id { get; set; }
	public long UserId { get; set; }
	public string UserName { get; set; }
	public string UserAvatarUrl { get; set; }
	public int Score { get; set; }
	public string Review { get; set; }
	public string Pros { get; set; }
	public string Cons { get; set; }
	public bool IsAnonymous { get; set; }
	public bool IsVerified { get; set; }
	public int HelpfulCount { get; set; }
	public DateTime CreatedAt { get; set; }
}
