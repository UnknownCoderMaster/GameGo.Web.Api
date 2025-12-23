using GameGo.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace GameGo.Application.Features.Services.Queries.GetPlaceServices;

public class GetPlaceServicesQuery : IRequest<Result<List<ServiceDto>>>
{
	public long PlaceId { get; set; }
	public bool? IsActive { get; set; }
}
