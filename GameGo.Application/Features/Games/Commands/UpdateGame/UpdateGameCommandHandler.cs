using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Games.Commands.UpdateGame;

public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand, Result>
{
	private readonly IApplicationDbContext _context;

	public UpdateGameCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
	{
		// Find game
		var game = await _context.Games
			.FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);

		if (game == null)
			return Result.Failure("Game not found");

		// Update game
		game.Update(
			request.Name,
			request.Description,
			request.ReleaseYear,
			request.CoverImageUrl);

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
