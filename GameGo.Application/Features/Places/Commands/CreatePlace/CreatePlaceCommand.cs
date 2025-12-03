using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Places.Commands.CreatePlace;

public class CreatePlaceCommand : IRequest<Result<long>>
{
	public long PlaceTypeId { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Address { get; set; }
	public decimal Latitude { get; set; }
	public decimal Longitude { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public string Website { get; set; }
	public string InstagramUsername { get; set; }
	public string TelegramUsername { get; set; }
}