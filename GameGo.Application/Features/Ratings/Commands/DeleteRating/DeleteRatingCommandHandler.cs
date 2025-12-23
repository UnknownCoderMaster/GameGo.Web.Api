using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Ratings.Commands.DeleteRating;

public class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public DeleteRatingCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result.Failure("User must be authenticated");

		// Find rating
		var rating = await _context.Ratings
			.FirstOrDefaultAsync(r => r.Id == request.RatingId, cancellationToken);

		if (rating == null)
			return Result.Failure("Rating not found");

		// Check ownership
		if (rating.UserId != _currentUser.UserId.Value)
			return Result.Failure("You can only delete your own ratings");

		var placeId = rating.PlaceId;

		// Delete rating
		_context.Ratings.Remove(rating);
		await _context.SaveChangesAsync(cancellationToken);

		// Update place rating statistics
		await UpdatePlaceRatingStatistics(placeId, cancellationToken);

		return Result.Success();
	}

	private async Task UpdatePlaceRatingStatistics(long placeId, CancellationToken cancellationToken)
	{
		var ratings = await _context.Ratings
			.Where(r => r.PlaceId == placeId)
			.ToListAsync(cancellationToken);

		var place = await _context.Places.FindAsync(new object[] { placeId }, cancellationToken);

		if (place != null)
		{
			if (ratings.Any())
			{
				place.UpdateRating((decimal)ratings.Average(r => r.Score), ratings.Count);
			}
			else
			{
				place.UpdateRating(0, 0);
			}

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
