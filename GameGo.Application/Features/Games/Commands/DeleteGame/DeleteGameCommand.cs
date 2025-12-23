using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Games.Commands.DeleteGame;

public class DeleteGameCommand : IRequest<Result>
{
	public long GameId { get; set; }
}
