using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Ratings.Commands.MarkRatingHelpful;

public class MarkRatingHelpfulCommandHandler : IRequestHandler<MarkRatingHelpfulCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public MarkRatingHelpfulCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(MarkRatingHelpfulCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result.Failure("User must be authenticated");

		// Check if rating exists
		var rating = await _context.Ratings
			.FirstOrDefaultAsync(r => r.Id == request.RatingId, cancellationToken);

		if (rating == null)
			return Result.Failure("Rating not found");

		// Check if user has already marked this rating
		var existingMark = await _context.RatingHelpfuls
			.FirstOrDefaultAsync(rh => rh.RatingId == request.RatingId && rh.UserId == _currentUser.UserId.Value, cancellationToken);

		if (existingMark != null)
		{
			// Remove existing mark
			_context.RatingHelpfuls.Remove(existingMark);
			rating.DecrementHelpful();
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}

		// Create new helpful mark
		var ratingHelpful = RatingHelpful.Create(request.RatingId, _currentUser.UserId.Value, request.IsHelpful);

		_context.RatingHelpfuls.Add(ratingHelpful);
		rating.IncrementHelpful();

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
