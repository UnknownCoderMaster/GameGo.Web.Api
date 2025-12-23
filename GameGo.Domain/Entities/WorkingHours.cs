using GameGo.Domain.Common;
using GameGo.Domain.Enums;

namespace GameGo.Domain.Entities;

public class WorkingHours : AuditableEntity
{
	public long PlaceId { get; private set; }
	public DayOfWeek DayOfWeek { get; private set; }
	public System.TimeSpan OpenTime { get; private set; }
	public System.TimeSpan CloseTime { get; private set; }
	public bool IsClosed { get; private set; } 

	public virtual Place Place { get; private set; } = null!;

	private WorkingHours() { }

	public static WorkingHours Create(
		long placeId,
		DayOfWeek dayOfWeek,
		System.TimeSpan openTime,
		System.TimeSpan closeTime,
		bool isClosed = false)
	{
		return new WorkingHours
		{
			PlaceId = placeId,
			DayOfWeek = dayOfWeek,
			OpenTime = openTime,
			CloseTime = closeTime,
			IsClosed = isClosed
		};
	}

	public void Update(
		System.TimeSpan openTime,
		System.TimeSpan closeTime,
		bool isClosed)
	{
		OpenTime = openTime;
		CloseTime = closeTime;
		IsClosed = isClosed;
	}

	public void MarkAsClosed() => IsClosed = true;

	public void MarkAsOpen() => IsClosed = false;
}