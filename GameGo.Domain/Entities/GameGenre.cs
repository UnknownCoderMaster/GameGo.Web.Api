using GameGo.Domain.Common;

public class GameGenre : AuditableEntity
{
	public long GameId { get; private set; }
	public long GenreId { get; private set; }

	public virtual Game Game { get; private set; } = null!;
	public virtual Genre Genre { get; private set; } = null!;

	private GameGenre() { }
}