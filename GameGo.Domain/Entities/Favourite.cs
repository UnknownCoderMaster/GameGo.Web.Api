using GameGo.Domain.Common;
using GameGo.Domain.Entities;

public class Favourite : AuditableEntity
{
	public long UserId { get; set; }
	public long PlaceId { get; set; }

	public virtual User User { get; set; } = null!;
	public virtual Place Place { get; set; } = null!;

	//private Favourite() { }
}