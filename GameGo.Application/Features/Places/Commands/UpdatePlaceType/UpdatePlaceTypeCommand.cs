using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Places.Commands.UpdatePlaceType;

public class UpdatePlaceTypeCommand : IRequest<Result<bool>>
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Slug { get; set; }
	public string Icon { get; set; }
	public string Description { get; set; }
}
