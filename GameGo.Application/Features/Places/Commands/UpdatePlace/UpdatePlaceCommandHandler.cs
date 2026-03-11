using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Commands.UpdatePlace;

public class UpdatePlaceCommandHandler : IRequestHandler<UpdatePlaceCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public UpdatePlaceCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(UpdatePlaceCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result.Failure("User must be authenticated");

		var place = await _context.Places
			.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

		if (place == null)
			return Result.Failure("Place not found");

		if (place.OwnerId != _currentUser.UserId.Value)
			return Result.Failure("Only place owner can update this place");

		var placeTypeExists = await _context.PlaceTypes
			.AnyAsync(pt => pt.Id == request.PlaceTypeId && pt.IsActive, cancellationToken);

		if (!placeTypeExists)
			return Result.Failure("Place type not found or inactive");

		place.Update(
			request.PlaceTypeId,
			request.Name,
			request.Address,
			request.Latitude,
			request.Longitude,
			request.PhoneNumber,
			request.Description,
			request.Email,
			request.Website,
			request.InstagramUsername,
			request.TelegramUsername);

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
