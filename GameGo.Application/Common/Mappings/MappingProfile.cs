using AutoMapper;
using GameGo.Application.Common.Models.Dtos;
using GameGo.Application.Features.Bookings.Queries.GetBookingById;
using GameGo.Application.Features.Bookings.Queries.GetUserBookings;
using GameGo.Application.Features.Places.Queries.GetPlaceById;
using GameGo.Application.Features.Places.Queries.SearchPlaces;
using GameGo.Application.Features.Ratings.Queries.GetPlaceRatings;
using GameGo.Domain.Entities;

namespace GameGo.Application.Common.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		// User mappings
		CreateMap<User, UserDto>();
		CreateMap<User, UserProfileDto>();

		// Place mappings
		CreateMap<Place, PlaceListDto>();
		CreateMap<Place, PlaceDetailDto>();

		// Booking mappings
		CreateMap<Booking, BookingListDto>();
		CreateMap<Booking, BookingDetailDto>();

		// Rating mappings
		CreateMap<Rating, RatingDto>();

		// Game mappings
		CreateMap<Game, GameDto>();

		// Service mappings
		CreateMap<Service, ServiceDto>();

		// WorkingHours mappings
		CreateMap<WorkingHours, WorkingHoursDto>();
	}
}