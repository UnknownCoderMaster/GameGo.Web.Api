namespace GameGo.Application.Common.Models.Dtos;

public class GameDto
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Slug { get; set; }
	public string Description { get; set; }
	public string CoverImageUrl { get; set; }
}