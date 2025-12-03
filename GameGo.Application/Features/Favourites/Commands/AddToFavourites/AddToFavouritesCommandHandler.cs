using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Favourites.Commands.AddToFavourites;

public class AddToFavouritesCommandHandler : IRequestHandler<AddToFavouritesCommand, Result<long>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public AddToFavouritesCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<long>> Handle(AddToFavouritesCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.UserId.HasValue)
			return Result<long>.Failure("Foydalanuvchi topilmadi");

		// Place mavjudligini tekshirish
		var placeExists = await _context.Places
			.AnyAsync(p => p.Id == request.PlaceId, cancellationToken);

		if (!placeExists)
			return Result<long>.Failure("Joy topilmadi");

		// Allaqachon sevimlilarda borligini tekshirish
		var alreadyExists = await _context.Favourites
			.AnyAsync(f => f.UserId == _currentUser.UserId.Value && f.PlaceId == request.PlaceId, cancellationToken);

		if (alreadyExists)
			return Result<long>.Failure("Bu joy allaqachon sevimlilar ro'yxatida");

		// Sevimlilar ro'yxatiga qo'shish
		var favourite = new Favourite
		{
			UserId = _currentUser.UserId.Value,
			PlaceId = request.PlaceId,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow
		};

		_context.Favourites.Add(favourite);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<long>.Success(favourite.Id);
	}
}