using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Games.Commands.CreateGame;

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Result<long>>
{
	private readonly IApplicationDbContext _context;

	public CreateGameCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<long>> Handle(CreateGameCommand request, CancellationToken cancellationToken)
	{
		// Check if game with same slug exists
		var exists = await _context.Games
			.AnyAsync(g => g.Slug == request.Slug.ToLower(), cancellationToken);

		if (exists)
			return Result<long>.Failure("Game with this slug already exists");

		// Create game
		var game = Game.Create(
			request.Name,
			request.Slug,
			request.Description,
			request.ReleaseYear,
			request.CoverImageUrl);

		_context.Games.Add(game);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<long>.Success(game.Id);
	}
}
