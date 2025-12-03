using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Favourites.Commands.RemoveFromFavourites;

public class RemoveFromFavouritesCommandHandler : IRequestHandler<RemoveFromFavouritesCommand, Result<bool>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public RemoveFromFavouritesCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<bool>> Handle(RemoveFromFavouritesCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.UserId.HasValue)
			return Result<bool>.Failure("Foydalanuvchi topilmadi");

		var favourite = await _context.Favourites
			.FirstOrDefaultAsync(f => f.UserId == _currentUser.UserId.Value && f.PlaceId == request.PlaceId, cancellationToken);

		if (favourite == null)
			return Result<bool>.Failure("Bu joy sevimlilar ro'yxatida emas");

		_context.Favourites.Remove(favourite);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<bool>.Success(true);
	}
}