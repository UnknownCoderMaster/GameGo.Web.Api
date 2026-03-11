using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Places.Commands.DeletePlace;

public class DeletePlaceCommand : IRequest<Result>
{
	public long Id { get; set; }
}
