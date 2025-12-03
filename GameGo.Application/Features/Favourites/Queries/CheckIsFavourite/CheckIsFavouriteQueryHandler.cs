using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Favourites.Queries.CheckIsFavourite;

public class CheckIsFavouriteQueryHandler : IRequestHandler<CheckIsFavouriteQuery, Result<bool>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public CheckIsFavouriteQueryHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<bool>> Handle(CheckIsFavouriteQuery request, CancellationToken cancellationToken)
	{
		if (!_currentUser.UserId.HasValue)
			return Result<bool>.Success(false);

		var isFavourite = await _context.Favourites
			.AnyAsync(f => f.UserId == _currentUser.UserId.Value && f.PlaceId == request.PlaceId, cancellationToken);

		return Result<bool>.Success(isFavourite);
	}
}