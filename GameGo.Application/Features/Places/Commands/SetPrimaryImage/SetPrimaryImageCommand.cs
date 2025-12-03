using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Places.Commands.SetPrimaryImage;

public class SetPrimaryImageCommand : IRequest<Result<bool>>
{
	public long PlaceId { get; set; }
	public long ImageId { get; set; }
}
