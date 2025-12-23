using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Ratings.Commands.CreateRating;

public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, Result<long>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public CreateRatingCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<long>> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result<long>.Failure("User must be authenticated");

		// Check if place exists
		var place = await _context.Places
			.FirstOrDefaultAsync(p => p.Id == request.PlaceId && p.IsActive, cancellationToken);

		if (place == null)
			return Result<long>.Failure("Place not found or inactive");

		// Check if user has already rated this place
		var existingRating = await _context.Ratings
			.AnyAsync(r => r.UserId == _currentUser.UserId.Value && r.PlaceId == request.PlaceId, cancellationToken);

		if (existingRating)
			return Result<long>.Failure("You have already rated this place");

		// Validate booking if provided
		if (request.BookingId.HasValue)
		{
			var booking = await _context.Bookings
				.FirstOrDefaultAsync(b =>
					b.Id == request.BookingId.Value &&
					b.UserId == _currentUser.UserId.Value &&
					b.PlaceId == request.PlaceId,
					cancellationToken);

			if (booking == null)
				return Result<long>.Failure("Booking not found or does not belong to you");
		}

		// Create rating
		var rating = Rating.Create(
			_currentUser.UserId.Value,
			request.PlaceId,
			request.Score,
			request.BookingId,
			request.Review,
			request.Pros,
			request.Cons,
			request.IsAnonymous);

		_context.Ratings.Add(rating);
		await _context.SaveChangesAsync(cancellationToken);

		// Update place rating statistics
		await UpdatePlaceRatingStatistics(request.PlaceId, cancellationToken);

		return Result<long>.Success(rating.Id);
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
