using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Commands.DeletePlace;

public class DeletePlaceCommandHandler : IRequestHandler<DeletePlaceCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public DeletePlaceCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(DeletePlaceCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result.Failure("User must be authenticated");

		var place = await _context.Places
			.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

		if (place == null)
			return Result.Failure("Place not found");

		if (place.OwnerId != _currentUser.UserId.Value)
			return Result.Failure("Only place owner can delete this place");

		place.Deactivate();

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
