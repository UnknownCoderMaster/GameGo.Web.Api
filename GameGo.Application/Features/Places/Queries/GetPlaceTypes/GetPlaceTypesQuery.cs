using GameGo.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace GameGo.Application.Features.Places.Queries.GetPlaceTypes;

public class GetPlaceTypesQuery : IRequest<Result<List<PlaceTypeDto>>>
{
}
