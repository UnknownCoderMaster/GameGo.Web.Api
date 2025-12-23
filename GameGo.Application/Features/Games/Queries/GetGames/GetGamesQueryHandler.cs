using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Games.Queries.GetGames;

public class GetGamesQueryHandler : IRequestHandler<GetGamesQuery, Result<PaginatedList<GameDto>>>
{
	private readonly IApplicationDbContext _context;

	public GetGamesQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<PaginatedList<GameDto>>> Handle(GetGamesQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Games.AsQueryable();

		// Apply search filter
		if (!string.IsNullOrWhiteSpace(request.SearchTerm))
		{
			var searchLower = request.SearchTerm.ToLower();
			query = query.Where(g => g.Name.ToLower().Contains(searchLower) || 
									g.Description.ToLower().Contains(searchLower));
		}

		// Apply active filter
		if (request.IsActive.HasValue)
		{
			query = query.Where(g => g.IsActive == request.IsActive.Value);
		}

		var gamesQuery = query
			.OrderBy(g => g.Name)
			.Select(g => new GameDto
			{
				Id = g.Id,
				Name = g.Name,
				Slug = g.Slug,
				Description = g.Description,
				ReleaseYear = g.ReleaseYear,
				CoverImageUrl = g.CoverImageUrl,
				IsActive = g.IsActive
			});

		var result = await PaginatedList<GameDto>.CreateAsync(
			gamesQuery,
			request.PageNumber,
			request.PageSize);

		return Result<PaginatedList<GameDto>>.Success(result);
	}
}
