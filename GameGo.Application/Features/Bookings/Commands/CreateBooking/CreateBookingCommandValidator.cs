using FluentValidation;
using System;

namespace GameGo.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
	public CreateBookingCommandValidator()
	{
		RuleFor(x => x.PlaceId)
			.GreaterThan(0).WithMessage("Place ID must be greater than 0");

		RuleFor(x => x.BookingDate)
			.GreaterThanOrEqualTo(DateTime.Today).WithMessage("Booking date cannot be in the past");

		RuleFor(x => x.StartTime)
			.LessThan(x => x.EndTime).WithMessage("Start time must be before end time");

		RuleFor(x => x.NumberOfPeople)
			.GreaterThan(0).WithMessage("Number of people must be at least 1")
			.LessThanOrEqualTo(50).WithMessage("Number of people cannot exceed 50");
	}
}