using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Places.Commands.DeletePlaceImage;

public class DeletePlaceImageCommand : IRequest<Result<bool>>
{
	public long PlaceId { get; set; }
	public long ImageId { get; set; }
}