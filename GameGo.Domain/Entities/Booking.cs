using GameGo.Domain.Common;
using GameGo.Domain.Enums;
using GameGo.Domain.Events;
using GameGo.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace GameGo.Domain.Entities;

public class Booking : AuditableEntity
{
	public long UserId { get; private set; }
	public long PlaceId { get; private set; }
	public long? ServiceId { get; private set; }
	public DateTime BookingDate { get; private set; }
	public TimeSpan StartTime { get; private set; }
	public TimeSpan EndTime { get; private set; }
	public int NumberOfPeople { get; private set; }
	public BookingStatus Status { get; private set; }
	public decimal TotalPrice { get; private set; }
	public string SpecialRequests { get; private set; }
	public string CancellationReason { get; private set; }
	public DateTime? CancelledAt { get; private set; }
	public DateTime? ConfirmedAt { get; private set; }

	// Navigation Properties
	public virtual User User { get; private set; } = null!;
	public virtual Place Place { get; private set; } = null!;
	public virtual Service Service { get; private set; }
	public virtual ICollection<BookingHistory> History { get; private set; } = new List<BookingHistory>();

	private Booking() { }

	public static Booking Create(
		long userId,
		long placeId,
		DateTime bookingDate,
		TimeSpan startTime,
		TimeSpan endTime,
		int numberOfPeople,
		decimal totalPrice,
		long? serviceId = null,
		string specialRequests = null)
	{
		if (startTime >= endTime)
			throw new ArgumentException("Start time must be before end time");

		if (numberOfPeople <= 0)
			throw new ArgumentException("Number of people must be greater than zero");

		var booking = new Booking
		{
			UserId = userId,
			PlaceId = placeId,
			ServiceId = serviceId,
			BookingDate = bookingDate,
			StartTime = startTime,
			EndTime = endTime,
			NumberOfPeople = numberOfPeople,
			TotalPrice = totalPrice,
			SpecialRequests = specialRequests,
			Status = BookingStatus.Pending
		};

		booking.AddDomainEvent(new BookingCreatedEvent(booking.Id, userId, placeId, bookingDate));
		return booking;
	}

	public void Confirm()
	{
		if (Status != BookingStatus.Pending)
			throw new InvalidEntityStateException("Only pending bookings can be confirmed");

		Status = BookingStatus.Confirmed;
		ConfirmedAt = DateTime.UtcNow;

		AddDomainEvent(new BookingConfirmedEvent(Id, UserId, PlaceId));
	}

	public void Cancel(string reason)
	{
		if (Status == BookingStatus.Completed || Status == BookingStatus.Cancelled)
			throw new InvalidEntityStateException("Cannot cancel completed or already cancelled booking");

		Status = BookingStatus.Cancelled;
		CancellationReason = reason;
		CancelledAt = DateTime.UtcNow;

		AddDomainEvent(new BookingCancelledEvent(Id, UserId, PlaceId, reason));
	}

	public void Complete()
	{
		if (Status != BookingStatus.Confirmed)
			throw new InvalidEntityStateException("Only confirmed bookings can be completed");

		Status = BookingStatus.Completed;
	}

	public void MarkAsNoShow()
	{
		if (Status != BookingStatus.Confirmed)
			throw new InvalidEntityStateException("Only confirmed bookings can be marked as no-show");

		Status = BookingStatus.NoShow;
	}
}