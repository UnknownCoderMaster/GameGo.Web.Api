using System;

namespace GameGo.Application.Features.WorkingHours.Queries.GetPlaceWorkingHours;

public class WorkingHoursDto
{
	public long Id { get; set; }
	public GameGo.Domain.Enums.DayOfWeek DayOfWeek { get; set; }
	public TimeSpan OpenTime { get; set; }
	public TimeSpan CloseTime { get; set; }
	public bool IsClosed { get; set; }
}
