namespace GameGo.Application.Common.Models.Dtos;

public class ServiceDto
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public decimal Price { get; set; }
	public string Currency { get; set; }
	public int? DurationMinutes { get; set; }
	public int Capacity { get; set; }
}