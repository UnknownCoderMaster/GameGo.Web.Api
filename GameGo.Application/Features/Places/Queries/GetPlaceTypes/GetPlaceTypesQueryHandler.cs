using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Queries.GetPlaceTypes;

public class GetPlaceTypesQueryHandler : IRequestHandler<GetPlaceTypesQuery, Result<List<PlaceTypeDto>>>
{
	private readonly IApplicationDbContext _context;

	public GetPlaceTypesQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<List<PlaceTypeDto>>> Handle(GetPlaceTypesQuery request, CancellationToken cancellationToken)
	{
		var placeTypes = await _context.PlaceTypes
			.Where(pt => pt.IsActive)
			.OrderBy(pt => pt.Name)
			.Select(pt => new PlaceTypeDto
			{
				Id = pt.Id,
				Name = pt.Name,
				Slug = pt.Slug,
				Icon = pt.Icon,
				Description = pt.Description
			})
			.ToListAsync(cancellationToken);

		return Result<List<PlaceTypeDto>>.Success(placeTypes);
	}
}