using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Games.Commands.CreateGame;

public class CreateGameCommand : IRequest<Result<long>>
{
	public string Name { get; set; }
	public string Slug { get; set; }
	public string Description { get; set; }
	public int? ReleaseYear { get; set; }
	public string CoverImageUrl { get; set; }
}
