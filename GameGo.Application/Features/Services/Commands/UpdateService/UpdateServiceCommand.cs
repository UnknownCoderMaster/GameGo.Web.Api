using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Services.Commands.UpdateService;

public class UpdateServiceCommand : IRequest<Result>
{
	public long ServiceId { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public decimal Price { get; set; }
	public int? DurationMinutes { get; set; }
	public int Capacity { get; set; }
}
