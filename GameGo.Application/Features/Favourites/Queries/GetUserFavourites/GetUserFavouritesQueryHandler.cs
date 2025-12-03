using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Favourites.Queries.GetMyFavourites;

public class GetMyFavouritesQueryHandler : IRequestHandler<GetMyFavouritesQuery, Result<PaginatedList<FavouritePlaceDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public GetMyFavouritesQueryHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<PaginatedList<FavouritePlaceDto>>> Handle(GetMyFavouritesQuery request, CancellationToken cancellationToken)
	{
		if (!_currentUser.UserId.HasValue)
			return Result<PaginatedList<FavouritePlaceDto>>.Failure("Foydalanuvchi topilmadi");

		var query = _context.Favourites
			.Where(f => f.UserId == _currentUser.UserId.Value)
			.Include(f => f.Place)
				.ThenInclude(p => p.PlaceType)
			.Include(f => f.Place)
				.ThenInclude(p => p.PlaceImages)
			.AsQueryable();

		// Filter by PlaceTypeId
		if (request.PlaceTypeId.HasValue)
		{
			query = query.Where(f => f.Place.PlaceTypeId == request.PlaceTypeId.Value);
		}

		// Order by latest added
		query = query.OrderByDescending(f => f.CreatedAt);

		// Map to DTO
		var favouritesQuery = query.Select(f => new FavouritePlaceDto
		{
			FavouriteId = f.Id,
			PlaceId = f.PlaceId,
			PlaceName = f.Place.Name,
			PlaceSlug = f.Place.Slug,
			Description = f.Place.Description,
			Address = f.Place.Address,
			Latitude = f.Place.Latitude,
			Longitude = f.Place.Longitude,
			PhoneNumber = f.Place.PhoneNumber,
			AverageRating = f.Place.AverageRating,
			TotalRatings = f.Place.TotalRatings,

			PlaceTypeId = f.Place.PlaceTypeId,
			PlaceTypeName = f.Place.PlaceType.Name,
			PlaceTypeSlug = f.Place.PlaceType.Slug,
			PlaceTypeIcon = f.Place.PlaceType.Icon,

			PrimaryImageUrl = f.Place.PlaceImages
				.Where(img => img.IsPrimary)
				.Select(img => img.ImageUrl)
				.FirstOrDefault() ??
				f.Place.PlaceImages
					.OrderBy(img => img.DisplayOrder)
					.Select(img => img.ImageUrl)
					.FirstOrDefault(),

			AddedAt = f.CreatedAt
		});

		// Pagination
		var paginatedList = await PaginatedList<FavouritePlaceDto>.CreateAsync(
			favouritesQuery,
			request.PageNumber,
			request.PageSize);

		return Result<PaginatedList<FavouritePlaceDto>>.Success(paginatedList);
	}
}