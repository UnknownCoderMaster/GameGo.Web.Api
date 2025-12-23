using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Games.Commands.UpdateGame;

public class UpdateGameCommand : IRequest<Result>
{
	public long GameId { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public int? ReleaseYear { get; set; }
	public string CoverImageUrl { get; set; }
}
