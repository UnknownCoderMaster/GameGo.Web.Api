using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Services.Commands.CreateService;

public class CreateServiceCommand : IRequest<Result<long>>
{
	public long PlaceId { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public decimal Price { get; set; }
	public string Currency { get; set; } = "UZS";
	public int? DurationMinutes { get; set; }
	public int Capacity { get; set; } = 1;
}
