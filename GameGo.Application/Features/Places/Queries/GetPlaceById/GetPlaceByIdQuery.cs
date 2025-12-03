using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Places.Queries.GetPlaceById;

public class GetPlaceByIdQuery : IRequest<Result<PlaceDetailDto>>
{
	public long Id { get; set; }
}