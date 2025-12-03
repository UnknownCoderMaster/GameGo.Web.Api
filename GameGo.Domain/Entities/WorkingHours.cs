using GameGo.Domain.Common;
using GameGo.Domain.Entities;
using GameGo.Domain.Enums;

public class WorkingHours : AuditableEntity
{
	public long PlaceId { get; private set; }
	public DayOfWeek DayOfWeek { get; private set; }
	public System.TimeSpan OpenTime { get; private set; }
	public System.TimeSpan CloseTime { get; private set; }
	public bool IsClosed { get; private set; } 

	public virtual Place Place { get; private set; } = null!;

	private WorkingHours() { }
}