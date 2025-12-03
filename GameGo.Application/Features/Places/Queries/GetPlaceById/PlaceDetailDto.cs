using GameGo.Application.Common.Models.Dtos;
using System.Collections.Generic;

namespace GameGo.Application.Features.Places.Queries.GetPlaceById;

public class PlaceDetailDto
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Slug { get; set; }
	public string Description { get; set; }
	public string Address { get; set; }
	public decimal Latitude { get; set; }
	public decimal Longitude { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public string Website { get; set; }
	public string InstagramUsername { get; set; }
	public string TelegramUsername { get; set; }
	public decimal AverageRating { get; set; }
	public int TotalRatings { get; set; }
	public int TotalBookings { get; set; }
	public bool IsActive { get; set; }
	public bool IsVerified { get; set; }
	public string PlaceTypeName { get; set; }
	public List<string> Images { get; set; }
	public List<string> Features { get; set; }
	public List<WorkingHoursDto> WorkingHours { get; set; }
	public List<ServiceDto> Services { get; set; }
	public List<GameDto> Games { get; set; }
}