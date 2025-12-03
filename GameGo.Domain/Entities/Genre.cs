using GameGo.Domain.Common;
using System.Collections.Generic;

public class Genre : AuditableEntity
{
	public string Name { get; set; } = null!;
	public string Slug { get; set; } = null!;

	public virtual ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

	//private Genre() { }
}