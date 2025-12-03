using GameGo.Application.Common.Models;
using GameGo.Application.Common.Models.Dtos;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Queries.GetPlaceById;

public class GetPlaceByIdQueryHandler : IRequestHandler<GetPlaceByIdQuery, Result<PlaceDetailDto>>
{
	private readonly IApplicationDbContext _context;

	public GetPlaceByIdQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<PlaceDetailDto>> Handle(GetPlaceByIdQuery request, CancellationToken cancellationToken)
	{
		var place = await _context.Places
			.Include(p => p.PlaceType)
			.Include(p => p.PlaceImages)
			.Include(p => p.PlaceFeatures)
			.Include(p => p.WorkingHours)
			.Include(p => p.Services)
			.Include(p => p.PlaceGames)
				.ThenInclude(pg => pg.Game)
			.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

		if (place == null)
			return Result<PlaceDetailDto>.Failure("Place not found");

		var dto = new PlaceDetailDto
		{
			Id = place.Id,
			Name = place.Name,
			Slug = place.Slug,
			Description = place.Description,
			Address = place.Address,
			Latitude = place.Latitude,
			Longitude = place.Longitude,
			PhoneNumber = place.PhoneNumber,
			Email = place.Email,
			Website = place.Website,
			InstagramUsername = place.InstagramUsername,
			TelegramUsername = place.TelegramUsername,
			AverageRating = place.AverageRating,
			TotalRatings = place.TotalRatings,
			TotalBookings = place.TotalBookings,
			IsActive = place.IsActive,
			IsVerified = place.IsVerified,
			PlaceTypeName = place.PlaceType.Name,
			Images = place.PlaceImages
				.OrderBy(i => i.DisplayOrder)
				.Select(i => i.ImageUrl)
				.ToList(),
			Features = place.PlaceFeatures
				.Where(f => f.IsAvailable)
				.Select(f => f.FeatureName)
				.ToList(),
			WorkingHours = place.WorkingHours
				.OrderBy(w => w.DayOfWeek)
				.Select(w => new WorkingHoursDto
				{
					DayOfWeek = w.DayOfWeek,
					OpenTime = w.OpenTime,
					CloseTime = w.CloseTime,
					IsClosed = w.IsClosed
				})
				.ToList(),
			Services = place.Services
				.Where(s => s.IsActive)
				.Select(s => new ServiceDto
				{
					Id = s.Id,
					Name = s.Name,
					Description = s.Description,
					Price = s.Price,
					Currency = s.Currency,
					DurationMinutes = s.DurationMinutes,
					Capacity = s.Capacity
				})
				.ToList(),
			Games = place.PlaceGames
				.Where(pg => pg.IsAvailable)
				.Select(pg => new GameDto
				{
					Id = pg.Game.Id,
					Name = pg.Game.Name,
					Slug = pg.Game.Slug,
					Description = pg.Game.Description,
					CoverImageUrl = pg.Game.CoverImageUrl
				})
				.ToList()
		};

		return Result<PlaceDetailDto>.Success(dto);
	}
}