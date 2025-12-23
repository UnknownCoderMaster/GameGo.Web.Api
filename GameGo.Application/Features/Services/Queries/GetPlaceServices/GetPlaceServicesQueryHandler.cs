using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Services.Queries.GetPlaceServices;

public class GetPlaceServicesQueryHandler : IRequestHandler<GetPlaceServicesQuery, Result<List<ServiceDto>>>
{
	private readonly IApplicationDbContext _context;

	public GetPlaceServicesQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<List<ServiceDto>>> Handle(GetPlaceServicesQuery request, CancellationToken cancellationToken)
	{
		// Check if place exists
		var placeExists = await _context.Places.AnyAsync(p => p.Id == request.PlaceId, cancellationToken);

		if (!placeExists)
			return Result<List<ServiceDto>>.Failure("Place not found");

		var query = _context.Services
			.Where(s => s.PlaceId == request.PlaceId)
			.AsQueryable();

		// Apply filter
		if (request.IsActive.HasValue)
		{
			query = query.Where(s => s.IsActive == request.IsActive.Value);
		}

		var services = await query
			.Select(s => new ServiceDto
			{
				Id = s.Id,
				Name = s.Name,
				Description = s.Description,
				Price = s.Price,
				Currency = s.Currency,
				DurationMinutes = s.DurationMinutes,
				Capacity = s.Capacity,
				IsActive = s.IsActive
			})
			.ToListAsync(cancellationToken);

		return Result<List<ServiceDto>>.Success(services);
	}
}
