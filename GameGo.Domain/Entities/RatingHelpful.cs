using GameGo.Domain.Common;
using GameGo.Domain.Entities;

public class RatingHelpful : AuditableEntity
{
	public long RatingId { get; private set; }
	public long UserId { get; private set; }
	public bool IsHelpful { get; private set; }

	public virtual Rating Rating { get; private set; } = null!;
	public virtual User User { get; private set; } = null!;

	private RatingHelpful() { }

	public static RatingHelpful Create(long ratingId, long userId, bool isHelpful = true)
	{
		return new RatingHelpful
		{
			RatingId = ratingId,
			UserId = userId,
			IsHelpful = isHelpful
		};
	}
}