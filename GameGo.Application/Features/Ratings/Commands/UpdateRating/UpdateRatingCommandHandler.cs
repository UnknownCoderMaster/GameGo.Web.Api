using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Ratings.Commands.UpdateRating;

public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public UpdateRatingCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
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
			return Result.Failure("You can only update your own ratings");

		// Update rating
		rating.Update(request.Score, request.Review, request.Pros, request.Cons);
		await _context.SaveChangesAsync(cancellationToken);

		// Update place rating statistics
		await UpdatePlaceRatingStatistics(rating.PlaceId, cancellationToken);

		return Result.Success();
	}

	private async Task UpdatePlaceRatingStatistics(long placeId, CancellationToken cancellationToken)
	{
		var ratings = await _context.Ratings
			.Where(r => r.PlaceId == placeId)
			.ToListAsync(cancellationToken);

		if (ratings.Any())
		{
			var place = await _context.Places.FindAsync(new object[] { placeId }, cancellationToken);

			if (place != null)
			{
				place.UpdateRating((decimal)ratings.Average(r => r.Score), ratings.Count);
				await _context.SaveChangesAsync(cancellationToken);
			}
		}
	}
}
