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

	public static Game Create(
		string name,
		string slug,
		string description = null,
		int? releaseYear = null,
		string coverImageUrl = null)
	{
		return new Game
		{
			Name = name,
			Slug = slug.ToLower(),
			Description = description,
			ReleaseYear = releaseYear,
			CoverImageUrl = coverImageUrl,
			IsActive = true
		};
	}

	public void Update(
		string name,
		string description = null,
		int? releaseYear = null,
		string coverImageUrl = null)
	{
		Name = name;
		Description = description;
		ReleaseYear = releaseYear;
		CoverImageUrl = coverImageUrl;
	}

	public void Activate() => IsActive = true;

	public void Deactivate() => IsActive = false;
}