using GameGo.Domain.Common;
using System.Collections.Generic;

public class Game : AuditableEntity
{
	public string Name { get; private set; } = null!;
	public string Slug { get; private set; } = null!;
	public string Description { get; private set; }
	public int? ReleaseYear { get; private set; }
	public string CoverImageUrl { get; private set; }
	public bool IsActive { get; private set; }

	public virtual ICollection<GameGenre> GameGenres { get; private set; } = new List<GameGenre>();
	public virtual ICollection<GameDevice> GameDevices { get; private set; } = new List<GameDevice>();
	public virtual ICollection<PlaceGame> PlaceGames { get; private set; } = new List<PlaceGame>();

	private Game() { }
}