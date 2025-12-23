namespace GameGo.Application.Features.Games.Queries.GetGames;

public class GameDto
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Slug { get; set; }
	public string Description { get; set; }
	public int? ReleaseYear { get; set; }
	public string CoverImageUrl { get; set; }
	public bool IsActive { get; set; }
}
