using GameGo.Domain.Common;

namespace GameGo.Domain.Entities;

public class Service : AuditableEntity
{
	public long PlaceId { get; private set; }
	public string Name { get; private set; } = null!;
	public string Description { get; private set; }
	public decimal Price { get; private set; }
	public string Currency { get; private set; } = "UZS";
	public int? DurationMinutes { get; private set; }
	public int Capacity { get; private set; }
	public bool IsActive { get; private set; }

	public virtual Place Place { get; private set; } = null!;

	private Service() { }
}