using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Games.Queries.GetGames;

public class GetGamesQuery : IRequest<Result<PaginatedList<GameDto>>>
{
	public string SearchTerm { get; set; }
	public bool? IsActive { get; set; }
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 20;
}
