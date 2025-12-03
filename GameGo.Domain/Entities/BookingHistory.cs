using GameGo.Domain.Common;
using GameGo.Domain.Entities;
using GameGo.Domain.Enums;

public class BookingHistory : AuditableEntity
{
	public long BookingId { get; private set; }
	public BookingStatus Status { get; private set; }
	public long? ChangedBy { get; private set; }
	public string Note { get; private set; }

	public virtual Booking Booking { get; private set; } = null!;

	private BookingHistory() { }
}