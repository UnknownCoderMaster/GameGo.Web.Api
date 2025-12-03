using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Queries.SearchPlaces;

public class SearchPlacesQueryHandler : IRequestHandler<SearchPlacesQuery, Result<PaginatedList<PlaceListDto>>>
{
	private readonly IApplicationDbContext _context;

	public SearchPlacesQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<PaginatedList<PlaceListDto>>> Handle(SearchPlacesQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Places
			.Include(p => p.PlaceType)
			.Include(p => p.PlaceImages)
			.Where(p => p.IsActive)
			.AsQueryable();

		// Search by name or description
		if (!string.IsNullOrWhiteSpace(request.SearchTerm))
		{
			var searchTerm = request.SearchTerm.ToLower();
			query = query.Where(p =>
				p.Name.ToLower().Contains(searchTerm) ||
				p.Description.ToLower().Contains(searchTerm) ||
				p.PhoneNumber.ToLower().Contains(searchTerm));
		}

		// Filter by place type
		if (request.PlaceTypeId.HasValue)
		{
			query = query.Where(p => p.PlaceTypeId == request.PlaceTypeId.Value);
		}

		// Filter by rating
		if (request.MinRating.HasValue)
		{
			query = query.Where(p => p.AverageRating >= request.MinRating.Value);
		}

		// Order by rating
		query = query.OrderByDescending(p => p.AverageRating)
					 .ThenByDescending(p => p.TotalRatings);

		var places = await query.ToListAsync(cancellationToken);

		// Calculate distance if location provided
		List<PlaceListDto> placeDtos = new List<PlaceListDto>();

		if (request.Latitude.HasValue && request.Longitude.HasValue)
		{
			foreach (var place in places)
			{
				var distance = CalculateDistance(
					request.Latitude.Value,
					request.Longitude.Value,
					place.Latitude,
					place.Longitude);

				if (request.RadiusKm.HasValue && distance > request.RadiusKm.Value)
					continue;

				placeDtos.Add(MapToDto(place, distance));
			}

			placeDtos = placeDtos.OrderBy(p => p.DistanceKm).ToList();
		}
		else
		{
			placeDtos = places.Select(p => MapToDto(p, null)).ToList();
		}

		// Manual pagination
		var totalCount = placeDtos.Count;
		var items = placeDtos
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToList();

		var result = new PaginatedList<PlaceListDto>(items, totalCount, request.PageNumber, request.PageSize);

		return Result<PaginatedList<PlaceListDto>>.Success(result);
	}

	private PlaceListDto MapToDto(Place place, double? distance)
	{
		return new PlaceListDto
		{
			Id = place.Id,
			Name = place.Name,
			Slug = place.Slug,
			Description = place.Description,
			Address = place.Address,
			Latitude = place.Latitude,
			Longitude = place.Longitude,
			AverageRating = place.AverageRating,
			TotalRatings = place.TotalRatings,
			PlaceTypeName = place.PlaceType.Name,
			PrimaryImage = place.PlaceImages
				.Where(i => i.IsPrimary)
				.Select(i => i.ImageUrl)
				.FirstOrDefault() ?? place.PlaceImages
				.OrderBy(i => i.DisplayOrder)
				.Select(i => i.ImageUrl)
				.FirstOrDefault(),
			DistanceKm = distance
		};
	}

	private double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
	{
		const double earthRadiusKm = 6371;

		var dLat = ToRadians((double)(lat2 - lat1));
		var dLon = ToRadians((double)(lon2 - lon1));

		var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
				Math.Cos(ToRadians((double)lat1)) * Math.Cos(ToRadians((double)lat2)) *
				Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

		var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

		return earthRadiusKm * c;
	}

	private double ToRadians(double degrees)
	{
		return degrees * Math.PI / 180;
	}
}