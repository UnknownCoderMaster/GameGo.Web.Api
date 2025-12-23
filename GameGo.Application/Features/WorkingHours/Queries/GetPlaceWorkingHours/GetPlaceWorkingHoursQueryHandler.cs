using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.WorkingHours.Queries.GetPlaceWorkingHours;

public class GetPlaceWorkingHoursQueryHandler : IRequestHandler<GetPlaceWorkingHoursQuery, Result<List<WorkingHoursDto>>>
{
	private readonly IApplicationDbContext _context;

	public GetPlaceWorkingHoursQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<List<WorkingHoursDto>>> Handle(GetPlaceWorkingHoursQuery request, CancellationToken cancellationToken)
	{
		var placeExists = await _context.Places.AnyAsync(p => p.Id == request.PlaceId, cancellationToken);

		if (!placeExists)
			return Result<List<WorkingHoursDto>>.Failure("Place not found");

		var workingHours = await _context.WorkingHours
			.Where(w => w.PlaceId == request.PlaceId)
			.OrderBy(w => w.DayOfWeek)
			.Select(w => new WorkingHoursDto
			{
				Id = w.Id,
				DayOfWeek = w.DayOfWeek,
				OpenTime = w.OpenTime,
				CloseTime = w.CloseTime,
				IsClosed = w.IsClosed
			})
			.ToListAsync(cancellationToken);

		return Result<List<WorkingHoursDto>>.Success(workingHours);
	}
}
