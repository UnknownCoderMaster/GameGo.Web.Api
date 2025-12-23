using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Services.Commands.DeleteService;

public class DeleteServiceCommand : IRequest<Result>
{
	public long ServiceId { get; set; }
}
