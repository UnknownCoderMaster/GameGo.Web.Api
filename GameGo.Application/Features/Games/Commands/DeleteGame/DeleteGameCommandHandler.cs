using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Games.Commands.DeleteGame;

public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, Result>
{
	private readonly IApplicationDbContext _context;

	public DeleteGameCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
	{
		// Find game
		var game = await _context.Games
			.FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);

		if (game == null)
			return Result.Failure("Game not found");

		// Delete game
		_context.Games.Remove(game);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
