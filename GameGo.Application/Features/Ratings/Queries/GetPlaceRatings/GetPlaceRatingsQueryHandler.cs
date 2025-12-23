using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Ratings.Queries.GetPlaceRatings;

public class GetPlaceRatingsQueryHandler : IRequestHandler<GetPlaceRatingsQuery, Result<PaginatedList<RatingDto>>>
{
	private readonly IApplicationDbContext _context;

	public GetPlaceRatingsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<PaginatedList<RatingDto>>> Handle(GetPlaceRatingsQuery request, CancellationToken cancellationToken)
	{
		// Check if place exists
		var placeExists = await _context.Places.AnyAsync(p => p.Id == request.PlaceId, cancellationToken);

		if (!placeExists)
			return Result<PaginatedList<RatingDto>>.Failure("Place not found");

		var query = _context.Ratings
			.Include(r => r.User)
			.Where(r => r.PlaceId == request.PlaceId)
			.OrderByDescending(r => r.CreatedAt)
			.AsQueryable();

		// Apply filters
		if (request.MinScore.HasValue)
		{
			query = query.Where(r => r.Score >= request.MinScore.Value);
		}

		if (request.VerifiedOnly.HasValue && request.VerifiedOnly.Value)
		{
			query = query.Where(r => r.IsVerified);
		}

		var ratingsQuery = query.Select(r => new RatingDto
		{
			Id = r.Id,
			UserId = r.UserId,
			UserName = r.IsAnonymous ? "Anonymous" : (r.User.FirstName + " " + r.User.LastName).Trim(),
			UserAvatarUrl = r.IsAnonymous ? null : r.User.AvatarUrl,
			Score = r.Score,
			Review = r.Review,
			Pros = r.Pros,
			Cons = r.Cons,
			IsAnonymous = r.IsAnonymous,
			IsVerified = r.IsVerified,
			HelpfulCount = r.HelpfulCount,
			CreatedAt = r.CreatedAt
		});

		var result = await PaginatedList<RatingDto>.CreateAsync(
			ratingsQuery,
			request.PageNumber,
			request.PageSize);

		return Result<PaginatedList<RatingDto>>.Success(result);
	}
}
