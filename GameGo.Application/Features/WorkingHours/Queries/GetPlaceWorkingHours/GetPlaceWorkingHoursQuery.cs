using GameGo.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace GameGo.Application.Features.WorkingHours.Queries.GetPlaceWorkingHours;

public class GetPlaceWorkingHoursQuery : IRequest<Result<List<WorkingHoursDto>>>
{
	public long PlaceId { get; set; }
}
