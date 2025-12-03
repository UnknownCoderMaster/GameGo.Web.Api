using GameGo.Domain.Enums;

namespace GameGo.Application.Common.Models.Dtos;

public class WorkingHoursDto
{
	public DayOfWeek DayOfWeek { get; set; }
	public System.TimeSpan OpenTime { get; set; }
	public System.TimeSpan CloseTime { get; set; }
	public bool IsClosed { get; set; }
}