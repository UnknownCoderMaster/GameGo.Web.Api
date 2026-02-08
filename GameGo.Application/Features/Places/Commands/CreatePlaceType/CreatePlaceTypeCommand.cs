using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Places.Commands.CreatePlaceType;

public class CreatePlaceTypeCommand : IRequest<Result<long>>
{
	public string Name { get; set; }
	public string Slug { get; set; }
	public string Icon { get; set; }
	public string Description { get; set; }
}
