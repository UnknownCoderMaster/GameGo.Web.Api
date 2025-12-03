using GameGo.Domain.Common;
using GameGo.Domain.Entities;

public class PlaceGame : AuditableEntity
{
	public long PlaceId { get; private set; }
	public long GameId { get; private set; }
	public bool IsAvailable { get; private set; }

	public virtual Place Place { get; private set; } = null!;
	public virtual Game Game { get; private set; } = null!;

	private PlaceGame() { }
}